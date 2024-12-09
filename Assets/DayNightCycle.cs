using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Light Settings")]
    public Light directionalLight; // Đèn dùng để làm mặt trời
    public Gradient lightColorGradient; // Màu sắc ánh sáng theo thời gian
    public AnimationCurve lightIntensityCurve; // Cường độ ánh sáng theo thời gian

    [Header("Day Settings")]
    public float dayLengthInMinutes = 6f; // Một ngày bằng 6 phút
    private float dayLengthInSeconds;

    private float timeOfDay = 0f; // Thời gian trong ngày (0-1)
    private float rotationSpeed; // Tốc độ quay của ánh sáng

    void Start()
    {
        // Tính toán thời gian một ngày bằng giây
        dayLengthInSeconds = dayLengthInMinutes * 60f;
        rotationSpeed = 360f / dayLengthInSeconds; // 360 độ quay trong 1 ngày
    }

    void Update()
    {
        // Cập nhật thời gian trong ngày
        timeOfDay += Time.deltaTime / dayLengthInSeconds;
        if (timeOfDay > 1f)
        {
            timeOfDay -= 1f; // Reset thời gian trong ngày
        }

        UpdateLight();
    }

    void UpdateLight()
    {
        // Cập nhật góc quay của ánh sáng
        directionalLight.transform.rotation = Quaternion.Euler(new Vector3((timeOfDay * 360f) - 90f, 170f, 0f));

        // Cập nhật màu sắc ánh sáng
        directionalLight.color = lightColorGradient.Evaluate(timeOfDay);

        // Cập nhật cường độ ánh sáng
        directionalLight.intensity = lightIntensityCurve.Evaluate(timeOfDay);
    }
}

