using UnityEngine;

public class HealthPotion : MonoBehaviour
{

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Truy cập vào script của player và tăng biến currentPotions
            collision.GetComponent<AttributesManager>().IncreasePotionCount();
            Destroy(gameObject); // Tiêu hủy bình máu
        }
    }
}
