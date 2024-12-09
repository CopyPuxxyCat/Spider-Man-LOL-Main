using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class BossBehavior : MonoBehaviour
{
    [Header("NavMesh")]
    public NavMeshAgent agent;
    public Transform player;

    [Header("Shooting Settings")]
    public GameObject singleShotProjectile;
    public GameObject burstShotProjectile;
    public GameObject continuousShotProjectile;
    public Transform firePoint;
    public Transform findingPoint;

    public float reloadTime = 5f;
    public float overheatTime = 30f;
    public Material defaultMaterial;
    public Material overheatMaterial;

    private int burstAmmo = 30;
    private int singleShotAmmo = 5;
    private int continuousShotAmmo = 60;

    private int shotCount = 0; // Count the number of shot rounds
    private bool isOverheating = false;

    [Header("Attack Zone")]
    public BoxCollider attackZone; // Vùng tấn công của Boss
    public GameObject rifle;
    public bool isPlayerInAttackZone;
    public Rigidbody playerRigidbody;
    private Rigidbody rb;
    private Animator animator;
    private bool isRunning;
    private bool isFindingNewPosition = false;

    private Coroutine shootingCoroutine;
    private bool hasStartedCoroutine = false;

    private enum BossState { Idle, Moving, Shooting, Reloading, Overheat }
    private BossState currentState = BossState.Idle;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        IsPathBlocked();
        if (!isPlayerInAttackZone)
        {
            MoveToPosition();
        }  
        else if(IsPathBlocked() == true /*&& !isFindingNewPosition*/)
        {
            /*isFindingNewPosition = true;
            // bi block va player van trong zone
            Vector3 newPosition = FindNewPosition();
            agent.isStopped = false; // Đảm bảo agent không bị dừng
            agent.speed = 3.5f;
            agent.SetDestination(newPosition);
            isFindingNewPosition = false;*/
            agent.SetDestination(player.position);
            animator.SetBool("isRunning", true);
            currentState = BossState.Moving;

        }    
        else
        {
            isRunning = false;
            animator.SetBool("isRunning", false);
            agent.SetDestination(transform.position);
            RotateTowardsPlayer();
            if (!hasStartedCoroutine)
            {
                StartCoroutine(BossBehaviorCycle());
                hasStartedCoroutine = true;
            }
        }
        Debug.Log("Agent Destination: " + agent.destination);
        Debug.Log("Agent Velocity: " + agent.velocity);

        //Debug.Log("state: " + currentState);
        //Debug.Log("bool: " + isPlayerInAttackZone);
        
    }

    private IEnumerator BossBehaviorCycle()
    {
        while (isPlayerInAttackZone == true && IsPathBlocked() == false)
        {
            if (isOverheating)
            {
                //agent.SetDestination(transform.position);
                currentState = BossState.Overheat;
                ChangeMaterial(overheatMaterial);
                yield return new WaitForSeconds(overheatTime);
                isOverheating = false;
                shotCount = 0;
                ChangeMaterial(defaultMaterial);
                hasStartedCoroutine = false;
            }
            else
            {
                //agent.SetDestination(transform.position);
                currentState = BossState.Shooting;
                yield return ExecuteShootingSequence();

                currentState = BossState.Reloading;
                yield return new WaitForSeconds(reloadTime);

                shotCount++;
                if (shotCount >= 3)
                {
                    isOverheating = true;
                }
            }
        }
    }

    private void MoveToPosition()
    {
        isRunning = true;
        animator.SetBool("isRunning", true);
        currentState = BossState.Moving;

        // Nếu không có vật cản, Boss sẽ di chuyển về phía Player
        if (!IsPathBlocked())
        {
            agent.SetDestination(player.position);
        }
        
        hasStartedCoroutine = false;
    }

    private bool IsPathBlocked()
    {
        Ray ray = new Ray(findingPoint.position, (player.position - findingPoint.position).normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, Vector3.Distance(findingPoint.position, player.position)))
        {
            
            // Nếu vật thể trúng không phải Player, tức là bị chắn
            if (hit.transform != player)
            {
                //Debug.Log("bi can ");
                return true;
            }
        }
        //Debug.Log(" k bi can ");
        return false;
    }

    // Tìm một vị trí mới mà Boss có thể nhìn thấy Player
    private Vector3 FindNewPosition()
    {
        animator.SetBool("isRunning", true);
        currentState = BossState.Moving;
        //Debug.Log("tim duong di");
        Vector3 bestPosition = transform.position;

        // Tạo nhiều điểm xung quanh Boss
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 5f;
            randomDirection.y = 0;

            Vector3 candidatePosition = transform.position + randomDirection;

            // Kiểm tra xem vị trí có nằm trên NavMesh không
            if (NavMesh.SamplePosition(candidatePosition, out NavMeshHit hit, 1f, NavMesh.AllAreas))
            {
                Vector3 directionToPlayer = (player.position - hit.position).normalized;
                if (!Physics.Raycast(hit.position, directionToPlayer, Vector3.Distance(hit.position, player.position)))
                {
                    bestPosition = hit.position; // Chọn vị trí tốt nhất
                    Debug.Log("Found new valid position: " + bestPosition);
                }
            }
            else
            {
                Debug.LogWarning("Invalid candidate position on NavMesh.");
            }
        }

        return bestPosition;
    }

    private void RotateTowardsPlayer()
    {
        //Debug.Log("goi rotation");
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Đảm bảo Boss chỉ xoay trên trục Y
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        //Debug.Log(""+ transform.rotation);
    }

    private IEnumerator ExecuteShootingSequence()
    {
        // Shooting sequence: Burst -> Single -> Continuous
        if (shotCount % 3 == 0)
        {
            yield return StartCoroutine(ShootBurst());
        }
        else if (shotCount % 3 == 1)
        {
            yield return StartCoroutine(ShootSingle());
        }
        else if (shotCount % 3 == 2)
        {
            yield return StartCoroutine(ShootContinuous());
        }
    }

    private IEnumerator ShootBurst()
    {
        for (int i = 0; i < burstAmmo / 5; i++)
        {
            if(!isPlayerInAttackZone || IsPathBlocked() == true)
            {
                break;
            }
            for (int j = 0; j < 5; j++)
            {
                animator.SetTrigger("burst");
                ShootProjectile(burstShotProjectile);
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator ShootSingle()
    {
        for (int i = 0; i < singleShotAmmo; i++)
        {
            animator.SetTrigger("single");
            ShootProjectile(singleShotProjectile);
            yield return new WaitForSeconds(2f);
        }
    }

    private IEnumerator ShootContinuous()
    {
        for (int i = 0; i < continuousShotAmmo; i++)
        {
            animator.SetTrigger("auto");
            ShootProjectile(continuousShotProjectile);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void ShootProjectile(GameObject projectile)
    {
        // Dự đoán vị trí của Player
        Vector3 predictedPosition = PredictPlayerPosition(player.position, playerRigidbody.velocity, firePoint.position);

        GameObject bullet = Instantiate(projectile, firePoint.position, firePoint.rotation);

        // Gán vị trí mục tiêu cho viên đạn
        bullet.GetComponent<BossBullet>().Initialize(predictedPosition);
    }

    private void ChangeMaterial(Material material)
    {
        rifle.GetComponent<Renderer>().material = material;
    }

    private Vector3 PredictPlayerPosition(Vector3 playerPosition, Vector3 playerVelocity, Vector3 firePosition)
    {
        // Tính thời gian dự đoán để viên đạn đến Player
        Vector3 toPlayer = playerPosition - firePosition;
        float distance = toPlayer.magnitude;
        float timeToTarget = distance / 10f;

        // Dự đoán vị trí dựa trên vận tốc và thời gian
        return playerPosition + playerVelocity * timeToTarget;
    }
}

