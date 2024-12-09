using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReadSecondContent : MonoBehaviour
{
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
                yield return new WaitForSeconds(0.06f);
            }
            yield return new WaitForSeconds(1f);
        }
        StopCoroutine(coroutine);
        //Destroy(NPC_Panel);
    }
}
