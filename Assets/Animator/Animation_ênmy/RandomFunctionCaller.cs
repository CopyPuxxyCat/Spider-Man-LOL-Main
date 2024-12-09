using UnityEngine;

public class RandomFunctionCaller : MonoBehaviour
{
    public Animator cuopanim;
    public void Function1()
    {
        // Hàm 1: Thực hiện tác vụ gì đó
        cuopanim.SetTrigger("da");
    }

    public void Function2()
    {
        // Hàm 2: Thực hiện tác vụ gì đó
        cuopanim.SetTrigger("dam");
    }

    public void Function3()
    {
        // Hàm 3: Thực hiện tác vụ gì đó
        cuopanim.SetTrigger("spin");
    }

    public void Function4()
    {
        // Hàm 4: Thực hiện tác vụ gì đó
        cuopanim.SetTrigger("hit");
    }

    public void Function5()
    {
        // Hàm 4: Thực hiện tác vụ gì đó
        cuopanim.SetTrigger("die");
    }

    void Update()
    {
        // Gọi hàm ngẫu nhiên mỗi khung hình (bạn có thể thay đổi thành các sự kiện khác)
        int randomIndex = Random.Range(0, 3);

        switch (randomIndex)
        {
            case 0:
                Function1();
                break;
            case 1:
                Function2();
                break;
            case 2:
                Function3();
                break;
        }
    }
}
