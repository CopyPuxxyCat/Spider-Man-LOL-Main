using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamagePopUpGenerator : MonoBehaviour
{
    public static DamagePopUpGenerator current;
    public GameObject prefab;
    
    public void Awake()
    {
        current = this;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.T))
        {
            Createpopup(new Vector3(1,1,1), Random.Range(0, 100).ToString(), Color.red);
        }
       
    }
    public void Createpopup(Vector3 position, string text, Color color)
    {
        var popup = Instantiate(prefab, position, Quaternion.identity);
        var temp = popup.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = text;
        temp.faceColor = color;

        Destroy(popup, 1f);
        Destroy(temp, 1f);
    }

    public IEnumerator DisableEnemy(GameObject enemy)
    {
        if (enemy.TryGetComponent<MonoBehaviour>(out MonoBehaviour script))
            script.enabled = false;  // Tắt logic của kẻ địch

        // Chờ trong 3 giây
        yield return new WaitForSeconds(3f);

        // Kích hoạt lại kẻ địch
        if (script != null) script.enabled = true;
    }

    [Header("Read Content")]
    public GameObject NPC_Panel;
    public TextMeshProUGUI NpcTextContext;
    public string[] content;

    Coroutine coroutine;

    private void Start()
    {
        coroutine = StartCoroutine(ReadContent());
        NpcTextContext.text = "";
    }

    IEnumerator ReadContent()
    {

        foreach (var item in content)
        {
            NpcTextContext.text = "";
            for (int i = 0; i < item.Length; i++)
            {
                NpcTextContext.text += item[i];
                yield return new WaitForSeconds(0.1f);
            }
            yield return new WaitForSeconds(1f);
        }
        StopCoroutine(coroutine);
        //Destroy(NPC_Panel);
    }

}
