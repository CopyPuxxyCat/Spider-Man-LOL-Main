using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    [Header("UI Components")]
    public GameObject questPanel; // Panel hiển thị thông tin nhiệm vụ
    public TMP_Text questTitleText; // Text hiển thị tiêu đề nhiệm vụ
    public TMP_Text questDescriptionText; // Text hiển thị thông tin nhiệm vụ
    public Button acceptQuestButton; // Nút nhận nhiệm vụ
    public Button cancelQuestButton; // Nút hủy nhiệm vụ
    public TMP_Text questStatusText; // Text hiển thị trạng thái nhiệm vụ
    public Button questOverviewButton; // Button chính hiển thị trạng thái và mở Panel
    public TMP_Text potionText; // text chua so luong potion

    [Header("Quest Data")]
    public List<Quest> quests; // Danh sách nhiệm vụ
    private Quest currentQuest;
    private bool OpenYet = false;


    private void Start()
    {
        questPanel.SetActive(false); // Panel nhiệm vụ bắt đầu ẩn

        quests = new List<Quest>
    {
        new Quest { id = 1, title = "Nhiệm vụ 1", description = "Thu thập 5 bình máu.", status = QuestStatus.NotAccepted },
        new Quest { id = 2, title = "Nhiệm vụ 2", description = "Tiêu diệt 5 con quái vật.", status = QuestStatus.NotAccepted },
        new Quest { id = 3,title = "Nhiệm vụ 3", description = "Tiêu diệt Boss.", status = QuestStatus.NotAccepted }
    };

        foreach (Quest quest in quests)
        {
            AddQuestButton(quest); // Thêm nút nhiệm vụ vào danh sách
        }

        // Thêm sự kiện vào nút chính để mở Panel
        questOverviewButton.onClick.AddListener(() => ToggleQuestPanel());

        // Ẩn thông tin nếu chưa có nhiệm vụ nào
        UpdateQuestOverviewButton(null);
    }

    private void Update()
    {
        PotionToText();

        if(Input.GetKeyDown(KeyCode.M))
        {
            if (OpenYet == false)
            {
                questPanel.SetActive(true);
                OpenYet = true;
            }
            else
            {
                questPanel.SetActive(false);
                OpenYet = false;
            }    
        }
        
        if(Quest1Progress() == true)
        {
            Quest curentQ;
            foreach (Quest item in quests)
            {
                if(item.id == 1)
                {
                   curentQ = item;
                    curentQ.status = QuestStatus.Completed;
                    SetCurrentQuest(curentQ);
                }
                    
            }
            
        }

        if (Quest2Progress() == true)
        {
            Quest curentQ;
            foreach (Quest item in quests)
            {
                if (item.id == 2)
                {
                    curentQ = item;
                    curentQ.status = QuestStatus.Completed;
                    SetCurrentQuest(curentQ);
                }

            }

        }

        if (Quest3Progress() == true)
        {
            Quest curentQ;
            foreach (Quest item in quests)
            {
                if (item.id == 3)
                {
                    curentQ = item;
                    curentQ.status = QuestStatus.Completed;
                    SetCurrentQuest(curentQ);
                }

            }

        }
    }

    private void ToggleQuestPanel()
    {
        if (OpenYet == false)
        {
            questPanel.SetActive(!questPanel.activeSelf);
            OpenYet = true;
        }
    }

    public void SelectQuest(Quest quest)
    {
        //Debug.Log("goi select quest");
        currentQuest = quest;
        questTitleText.text = quest.title;
        questDescriptionText.text = quest.description;
        UpdateQuestStatusText(quest.status);

        acceptQuestButton.onClick.RemoveAllListeners();
        acceptQuestButton.onClick.AddListener(() => AcceptQuest(quest));

        cancelQuestButton.onClick.RemoveAllListeners();
        cancelQuestButton.onClick.AddListener(() => CancelQuest(quest));
    }

    public void AcceptQuest(Quest quest)
    {
        quest.status = QuestStatus.InProgress;
        UpdateQuestStatusText(quest.status);
        UpdateQuestOverviewButton(quest);
        UpdateQuestListUI(); // Cập nhật màu sắc nút

        // Gọi SelectQuest để đảm bảo giao diện được cập nhật đúng sau khi nhận nhiệm vụ
        SelectQuest(quest);
    }

    public void CancelQuest(Quest quest)
    {
        quest.status = QuestStatus.Failed;
        UpdateQuestStatusText(quest.status);
        UpdateQuestOverviewButton(quest);
        UpdateQuestListUI(); // Cập nhật màu sắc nút

        // Gọi SelectQuest để đảm bảo giao diện được cập nhật đúng sau khi nhận nhiệm vụ
        SelectQuest(quest);
    }

    private void UpdateQuestStatusText(QuestStatus status)
    {
        switch (status)
        {
            case QuestStatus.NotAccepted:
                questStatusText.text = "Chưa nhận";
                questStatusText.color = Color.gray;
                break;
            case QuestStatus.Accepted:
                questStatusText.text = "Đã nhận";
                questStatusText.color = new Color(1f, 0.9f, 0.6f); // Màu vàng nhạt
                break;
            case QuestStatus.InProgress:
                questStatusText.text = "Đang thực hiện";
                questStatusText.color = Color.blue;
                break;
            case QuestStatus.Completed:
                questStatusText.text = "Hoàn thành";
                questStatusText.color = Color.green;
                break;
            case QuestStatus.Failed:
                questStatusText.text = "Thất bại";
                questStatusText.color = Color.red;
                break;
        }
    }

    public void OnClickExit()
    {
        questPanel.SetActive(false);
        OpenYet = false;
    }    

    private void UpdateQuestOverviewButton(Quest quest)
    {
        if (quest == null)
        {
            questOverviewButton.GetComponentInChildren<TMP_Text>().text = "Không có nhiệm vụ";
            return;
        }

        questOverviewButton.GetComponentInChildren<TMP_Text>().text = $"{quest.title}";
        questOverviewButton.GetComponentInChildren<TMP_Text>().color = GetStatusColor(quest.status);
    }

    private string GetStatusString(QuestStatus status)
    {
        switch (status)
        {
            case QuestStatus.NotAccepted:
                return "Chưa nhận";
            case QuestStatus.Accepted:
                return "Đã nhận";
            case QuestStatus.InProgress:
                return "Đang thực hiện";
            case QuestStatus.Completed:
                return "Hoàn thành";
            case QuestStatus.Failed:
                return "Thất bại";
            default:
                return "Không xác định";
        }
    }

    [Header("UI Components for Quest List")]
    public Transform questListParent; // Scroll Panel chứa các nút nhiệm vụ
    public GameObject questButtonPrefab; // Prefab của nút nhiệm vụ
    public AttributesManager am;

    private void AddQuestButton(Quest quest)
    {
        // Tạo button mới
        GameObject newButton = Instantiate(questButtonPrefab, questListParent.transform);

        // Thiết lập parent và reset local position
        newButton.transform.SetParent(questListParent.transform, false);
        newButton.transform.localPosition = Vector3.zero; // Reset vị trí theo hệ tọa độ của Scroll Panel
        newButton.transform.localScale = Vector3.one;    // Đảm bảo tỷ lệ không bị thay đổi

        // Lấy TMP_Text từ button
        TMP_Text buttonText = newButton.GetComponentInChildren<TMP_Text>();


        // Thiết lập thông tin cho nút
        if (buttonText != null)
        {
            buttonText.text = quest.title;
            buttonText.color = GetStatusColor(quest.status);
        }
        else
        {
            Debug.LogError("Không tìm thấy TMP_Text trên button!");
        }

        // Gán sự kiện OnClick cho nút
        Button buttonComponent = newButton.GetComponent<Button>();
        if (buttonComponent != null)
        {
            // Thêm gọi SelectQuest vào sự kiện
            buttonComponent.onClick.AddListener(() => SelectQuest(quest));
            buttonComponent.onClick.AddListener(() => SetCurrentQuest(quest));
        }
        else
        {
            Debug.LogError("Không tìm thấy Button component trên nút mới!");
        }
    }

    private Color GetStatusColor(QuestStatus status)
    {
        switch (status)
        {
            case QuestStatus.NotAccepted: return Color.gray;
            case QuestStatus.Accepted: return new Color(1f, 0.9f, 0.6f); // Vàng nhạt
            case QuestStatus.InProgress: return Color.blue;
            case QuestStatus.Completed: return Color.green;
            case QuestStatus.Failed: return Color.red;
            default: return Color.white;
        }
    }

    private void SetCurrentQuest(Quest quest)
    {
        // Thiết lập nhiệm vụ hiện tại và cập nhật trạng thái
        currentQuest = quest;
        questDescriptionText.text = quest.description;
        UpdateQuestStatusText(quest.status);

        // Gọi SelectQuest để cập nhật giao diện
        SelectQuest(quest);
    }

    private void UpdateQuestListUI()
    {
        foreach (Transform child in questListParent)
        {
            Button button = child.GetComponent<Button>();
            TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();

            Quest quest = quests.Find(q => q.title == buttonText.text);
            if (quest != null)
            {
                buttonText.color = GetStatusColor(quest.status);
            }
        }
    }

    private void PotionToText()
    {
        //Debug.Log("goi text mau");
        int curentP = am.GetComponent<AttributesManager>().currentPotions;
        potionText.text = $"{curentP}";
    }

    private bool Quest1Progress()
    {
        int curentP = am.GetComponent<AttributesManager>().currentPotions;
        //Debug.Log("curentP: " + curentP);
        if ( curentP >= 5)
        {
            return true;
        }  
        else
        {
            return false;
        }    

    }    

    private bool Quest2Progress()
    {
        int currentKill = am.GetComponent<AttributesManager>().currentKill;
        if (currentKill >= 5)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool Quest3Progress()
    {
        int currentKill = am.GetComponent<AttributesManager>().currentKill;
        if (currentKill >= 7)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}

[System.Serializable]
public class Quest
{
    public int id;
    public string title; // Tiêu đề nhiệm vụ
    public string description; // Mô tả nhiệm vụ
    public QuestStatus status; // Trạng thái nhiệm vụ
}

public enum QuestStatus
{
    NotAccepted,
    Accepted,
    InProgress,
    Completed,
    Failed
}

