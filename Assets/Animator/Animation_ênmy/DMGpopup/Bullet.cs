using Unity.Burst.Intrinsics;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 1f; // Tốc độ bay của viên đạn
    public float disableTime = 3f; // Thời gian vô hiệu hóa kẻ địch
    public GameObject spiderWebPrefab;


    private void Update()
    {
        // Đẩy viên đạn tiến lên phía trước
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Kiểm tra xem viên đạn có va chạm với đối tượng có tag "Enemy"
        if (other.CompareTag("enemy"))
        {
            DamagePopUpGenerator.current.StartCoroutine(DisableEnemy(other.gameObject));
            //StartCoroutine(DisableEnemy(other.gameObject));

            // Chuyển viên đạn thành mạng nhện
            Instantiate(spiderWebPrefab, transform.position, Quaternion.identity);

            // Phá hủy viên đạn
            Destroy(gameObject, 2f);
        }
        else
        {

            // Phá hủy viên đạn sau khi va chạm
            Destroy(gameObject, 2f);
        }
    }

    private System.Collections.IEnumerator DisableEnemy(GameObject enemy)
    {
        if (enemy.TryGetComponent<MonoBehaviour>(out MonoBehaviour script))
            script.enabled = false;  // Tắt logic của kẻ địch

        // Chờ trong 3 giây
        yield return new WaitForSeconds(disableTime);

        // Kích hoạt lại kẻ địch
        if (script != null) script.enabled = true;
    }

   
}

