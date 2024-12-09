using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public float speed = 20f; // Tốc độ của viên đạn
    public float lifeTime = 5f; // Thời gian tồn tại tối đa
    public int dmg;
    private Vector3 targetPosition; // Vị trí mục tiêu
    private Rigidbody rb; // Rigidbody của viên đạn

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Tính hướng bay
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Gán vận tốc cho viên đạn
        rb.velocity = direction * speed;

        // Xoay viên đạn theo hướng bay (trục Z là hướng chuyển động)
        RotateTowardsDirection(direction);

        // Tự hủy sau khi hết thời gian tồn tại
        Destroy(gameObject, lifeTime);
    }

    public void Initialize(Vector3 targetPos)
    {
        targetPosition = targetPos;
    }

    private void RotateTowardsDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Bù góc xoay nếu cần (để sửa lỗi xoay -90 độ trên Y)
        Quaternion adjustment = Quaternion.Euler(0, 90, 0);

        transform.rotation = targetRotation * adjustment;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<AttributesManager>().TakeDamage(dmg);
        }
    }
}
