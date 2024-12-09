using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithCamera : MonoBehaviour
{
    public Transform cameraTransform; // Gán camera vào đây
    public Transform capsuleTransform; // gan capsule
    public Transform capsuleTransform_2; // gan capsule
    public float maxRotationSpeed = 10f; // Tốc độ quay tối đa
    public float minRotationSpeed = 1f; // Tốc độ quay tối thiểu
    public Animator animator;
    public GameObject punchBox;
    public GameObject kickBox;

    private WallRunning wr;

    private void Start()
    {
        wr = GetComponent<WallRunning>();
    }

    void Update()
    {
        if (cameraTransform == null)
        {
            // Tìm camera chính nếu chưa gán
            cameraTransform = Camera.main?.transform;
            if (cameraTransform == null) return;
        }

        // Lấy hướng hiện tại của nhân vật
        Vector3 characterForward = transform.forward;

        // Lấy hướng của camera (chỉ trên mặt phẳng ngang)
        Vector3 cameraDirection = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;

        // Tính góc giữa hướng của nhân vật và hướng camera
        float angleDifference = Vector3.Angle(characterForward, cameraDirection);

        // Điều chỉnh tốc độ xoay dựa trên góc lệch (góc càng lớn, tốc độ càng cao)
        float rotationSpeed = Mathf.Lerp(minRotationSpeed, maxRotationSpeed, angleDifference / 180f);

        // Tính toán góc quay mục tiêu
        Quaternion targetRotation = Quaternion.LookRotation(cameraDirection);

        // Xoay nhân vật dần dần
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // giu nv luon nam cung capsule
        //transform.position = capsule.bounds.center;
        if(animator.GetBool("isOnWall") == true)
        {
            transform.position = capsuleTransform_2.position;

            //animator.transform.rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
            Quaternion offsetRotation = Quaternion.Euler(-90, 0, 0);
            Quaternion targetRotation_anim = Quaternion.LookRotation(cameraDirection) * offsetRotation;

            // Xoay nhân vật dần dần
            animator.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation_anim, Time.deltaTime * rotationSpeed);
        }
        else if(animator.GetBool("isWallRunning") == true)
        {
            
            animator.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            animator.transform.position = capsuleTransform.position;
        }

        else
        transform.position = capsuleTransform.position;

        punchBox.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        //kickBox.transform.position = capsuleTransform.position;
    }
}
