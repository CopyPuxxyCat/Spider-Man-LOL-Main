using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class AttributesManager : MonoBehaviour
{
    public int mau = 100;
    public int tancong;
    public float chimang = 1.5f;
    public float chimangq = 0.5f;
    public int giap;
    public Animator animator;
    public Slider thanhmauquai;
    public Slider thanhmaungchoi;


    private GameObject raycastOrigin;
    private GameObject currentEnemy;

    public int currentKill = 0;


    private void Start()
    {
        //thanhmau.value = 100;
        animator = GetComponent<Animator>();
        raycastOrigin = GameObject.Find("CheckHealth");

    }
    private void FixedUpdate()
    {       
            
    }

    void Update()
    {
        CheckHealthBar();
        if(Input.GetKeyDown(KeyCode.H))
        {
            UsingHealth();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            currentPotions = currentPotions + 5;
        }
    }

    public void CheckHealthBar()
    {
        
        Vector3 rayOrigin = raycastOrigin.transform.position;
        Vector3 rayDirection = raycastOrigin.transform.forward;
        Ray ray = new Ray(rayOrigin, rayDirection);

        // Thực hiện Raycast
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.tag == "enemy")
            {
                currentEnemy = hit.collider.gameObject;
                var atm = currentEnemy.GetComponent<AttributesManager>();
                thanhmauquai.value = atm.mau;
            }
            else
            {
                currentEnemy = null;
            }
        }
    }
        

    public void TakeDamage(int soluong)
    {
        mau -= soluong-(soluong *giap/100);
        if (gameObject.CompareTag("Player"))
            thanhmaungchoi.value = mau;
        else if(gameObject.CompareTag("enemy"))
            thanhmauquai.value = mau;
        DamagePopUpGenerator.current.Createpopup(transform.position, soluong.ToString(), Color.red);
        
        if (mau <= 0)
        {
            animator.SetTrigger("die");
            if(gameObject.CompareTag("Player"))
            {
                StartCoroutine(PauseGameAfterDelay());
            }
            else
            {
                currentKill = currentKill + 1;
                DropHealth();
                GetComponent<Collider>().enabled = false;
                Destroy(gameObject, 2f);
            }
        }
        else
        {
            //Debug.Log("quai mat mau");
            animator.SetTrigger("damage");
        }
    }


    IEnumerator PauseGameAfterDelay()
    {
        // Chờ 1 giây
        yield return new WaitForSeconds(2f);

        // Tạm dừng game bằng cách đặt Time.timeScale = 0
        gameObject.SetActive(false);
        Time.timeScale = 0f;
        Debug.Log("Game Paused");
    }

    public void dealdamage(GameObject target)
    {
        var atm=target.GetComponent<AttributesManager>();
        if (atm != null)
        {
            float tongdamage = tancong;
            if(Random.Range(0f,1f) < chimangq)
            
                tongdamage = chimang;
                atm.TakeDamage((int)tongdamage);
            
            
        }
    }


    [Header("Health")]
    public int currentPotions = 0; // Biến đếm số lượng bình máu
    public GameObject healthPotionPrefab;

    public void IncreasePotionCount()
    {
        currentPotions++;
    }

    private void DropHealth()
    {
        int randomDrop = Random.Range(1, 3);
        if (randomDrop == 1 || randomDrop == 2)
        {
            Instantiate(healthPotionPrefab, transform.position, Quaternion.identity);
        }
    }

    private void UsingHealth()
    {
        if(currentPotions > 0)
        {
            mau = mau + 50;
            thanhmaungchoi.value = mau;
            currentPotions--;
        }
    }
        
}
