using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyAI : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Transform player;
    public Transform[] escapePoints; // Các điểm để bỏ chạy
    public Animator animator;

    [Header("Settings")]
    public float attackRange;
    public float escapeDuration = 10f;

    [Header("State")]
    public bool isRunningAway = false;
    private bool hasATKThisCall = false;

    private float escapeTimer;
    private int currentEscapePoint = 0;
    public bool canActive;

    public GameObject theGang;

    private bool gangActivated = false; // Trạng thái để tránh kích hoạt gang nhiều lần
    private Vector3 lastDestination;   // Lưu tọa độ cuối cùng của agent để tránh gọi SetDestination liên tục

    private void Start()
    {
        escapeTimer = escapeDuration;
    }

    private void Update()
    {
        if (player == null || agent == null) return; // Kiểm tra null cho player và agent

        if (isRunningAway)
        {
            RunAway();
        }
        else
        {
            ChasePlayer();

            if (canActive && !gangActivated)
            {
                StartCoroutine(ActiveGang());
                gangActivated = true; // Đảm bảo chỉ kích hoạt một lần
            }

            if (Vector3.Distance(transform.position, player.position) <= attackRange)
            {
                AttackPlayer();
                animator.SetBool("run", false);
            }
        }
    }

    private IEnumerator ActiveGang()
    {
        yield return new WaitForSeconds(10f);
        theGang.SetActive(true);
    }

    private void RunAway()
    {
        StartCoroutine(SetSpeed());

        if (escapeTimer > 0)
        {
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                currentEscapePoint = (currentEscapePoint + 1) % escapePoints.Length;
                agent.SetDestination(escapePoints[currentEscapePoint].position);
            }

            escapeTimer -= Time.deltaTime;
        }
        else
        {
            isRunningAway = false;
            escapeTimer = escapeDuration;
        }

        animator.SetBool("isRunningAway", true);
    }

    private void ChasePlayer()
    {
        if (lastDestination != player.position)
        {
            agent.SetDestination(player.position);
            lastDestination = player.position;
        }

        animator.SetBool("isRunningAway", false);
        animator.SetBool("run", true);
    }

    private void AttackPlayer()
    {
        if (!hasATKThisCall)
        {
            StartCoroutine(PerformRandomAttack());
        }
    }

    private IEnumerator PerformRandomAttack()
    {
        while (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            if (!hasATKThisCall)
            {
                int randomAttack = Random.Range(1, 4); // Chọn 1 trong 3 đòn đánh
                animator.SetTrigger("Attack" + randomAttack);
                hasATKThisCall = true;
                yield return new WaitForSeconds(2f);
                hasATKThisCall = false;
            }

            yield return null; // Thêm lệnh này để tránh treo Unity
        }
    }

    private IEnumerator SetSpeed()
    {
        yield return new WaitForSeconds(10f);
        agent.speed = 15f;
    }
}
