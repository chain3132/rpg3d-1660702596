using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Toggle[] toggleMagic;
    public Toggle[] ToggleMagic
    {
        get { return toggleMagic; }
    }

    [SerializeField] private int curToggleMagicID = -1;
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private Toggle togglePauseUnpause;
    
    [SerializeField] private GameObject blackImage;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject grayImage;
    [SerializeField] private GameObject itemDialog;
    
    [SerializeField]
     private ItemDrag curItemDrag;

     [SerializeField] private int curSlotID;
    [SerializeField]
    private GameObject itemUIPrefab;

    [SerializeField]
    private GameObject[] slots;

    // (32.9) Dialogue Panel — Quest System
    [Header("Dialogue")]
    [SerializeField] private GameObject downPanel;
    [SerializeField] private GameObject npcDialoguePanel;
    [SerializeField] private Image npcImage;
    [SerializeField] private TMP_Text npcNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private int index; // dialogue step

    [SerializeField] private GameObject btnNext;
    [SerializeField] private TMP_Text btnNextText;
    [SerializeField] private GameObject btnAccept;
    [SerializeField] private TMP_Text btnAcceptText;
    [SerializeField] private GameObject btnReject;
    [SerializeField] private TMP_Text btnRejectText;
    [SerializeField] private GameObject btnFinish;
    [SerializeField] private TMP_Text btnFinishText;
    [SerializeField] private GameObject btnNotFinish;
    [SerializeField] private TMP_Text btnNotFinishText;
    
    
    public RectTransform SelectionBox
    {
        get { return selectionBox; }
    }

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        InitSlots();
        if (grayImage != null) grayImage.SetActive(false);
        if (itemDialog != null) itemDialog.SetActive(false);
        if (npcDialoguePanel != null) npcDialoguePanel.SetActive(false);
    }

    private void InitSlots()
    {
        for (int i = 0; i < InventoryManage.MAXSLOT; i++)
        {
            // ทำการใส่เลข ID ให้กับ Component InventorySlot ในแต่ละช่อง
            slots[i].GetComponent<InventorySlot>().ID = i;
        }
    }

    

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            togglePauseUnpause.isOn = !togglePauseUnpause.isOn;
        }
    }

    public void ToggleAI(bool isOn)
    {
        foreach (var member in PartyManager.instance.Members)
        {
            AttackAI ai = member.gameObject.GetComponent<AttackAI>();
            if (ai != null)
            {
                ai.enabled = isOn;
            }
        }
    }
    public void ToggleInventoryPanel()
    {
        if (!inventoryPanel.activeInHierarchy)
        {
            inventoryPanel.SetActive(true);
            blackImage.SetActive(true);
            ShowInventory();
        }
        else
        {
            inventoryPanel.SetActive(false);
            blackImage.SetActive(false);
            ClearInventory();
        }
    }
    public void ShowMagicToggle()
    {
        if (PartyManager.instance.SelectChars.Count <= 0)
        {
            return;
        }

        Characters hero = PartyManager.instance.SelectChars[0];
        for (int i = 0; i < hero.MagicSkills.Count; i++)
        {
            toggleMagic[i].interactable = true;
            toggleMagic[i].isOn = false;
            toggleMagic[i].GetComponentInChildren<Text>().text = hero.MagicSkills[i].Name;
            toggleMagic[i].targetGraphic.GetComponent<Image>().sprite = hero.MagicSkills[i].Icon;
        }
    }

    public void SelectMagicSkill(int i)
    {
        curToggleMagicID = i;
        PartyManager.instance.HeroSelectMagicSkill(i);
    }

    public void IsOnCurToggleMagic(bool flag)
    {
        toggleMagic[curToggleMagicID].isOn = flag;
    }
    public void PauseUpdate(bool isOn)
    {
        Time.timeScale = isOn ? 0 : 1;
    }
    public void SelectAll()
    {
        PartyManager.instance.SelectChars.Clear();

        foreach (Characters member in PartyManager.instance.Members)
        {
            if (member.CurHP > 0)
            {
                member.ToggleRingSelection(true);
                PartyManager.instance.SelectChars.Add(member);
            }
        }
    }
    public void ClearInventory()
    {
        // Clear Slots
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].transform.childCount > 0)
            {
                Transform child = slots[i].transform.GetChild(0);
                Destroy(child.gameObject);
            }
        }
    }

    public void ShowInventory()
    {
        if (PartyManager.instance.SelectChars.Count <= 0)
            return;

        Characters hero = PartyManager.instance.SelectChars[0];

        for (int i = 0; i < InventoryManage.MAXSLOT; i++)
        {
            if (hero.InventoryItems[i] != null)
            {
                GameObject itemObj = Instantiate(itemUIPrefab, slots[i].transform);
                ItemDrag itemDrag = itemObj.GetComponent<ItemDrag>();

                itemDrag.Item = hero.InventoryItems[i];
                itemDrag.IconParent = slots[i].transform;
                itemDrag.Image.sprite = hero.InventoryItems[i].Icon;
                itemDrag.UiManager = this; // (29.11) Link UIManager เข้า ItemDrag
            }
        }
    }

    // ══════════════════════════════════════════════════
    // DIALOGUE / QUEST SYSTEM  (32.11, 33.4, 33.9, 33.16)
    // ══════════════════════════════════════════════════

    // (32.11) เคลียร์ข้อมูลทั้งหมดใน Dialogue Box
    private void ClearDialogueBox()
    {
        npcImage.sprite    = null;
        npcNameText.text   = "";
        dialogueText.text  = "";

        btnNextText.text   = "";  btnNext.SetActive(false);
        btnAcceptText.text = "";  btnAccept.SetActive(false);
        btnRejectText.text = "";  btnReject.SetActive(false);
        btnFinishText.text = "";  btnFinish.SetActive(false);
        btnNotFinishText.text = ""; btnNotFinish.SetActive(false);
    }

    // (32.11) เปิด Dialogue ครั้งแรก (Quest ใหม่)
    private void StartQuestDialogue(Quest quest)
    {
        dialogueText.text  = quest.QuestDialogue[index];

        btnNext.SetActive(true);
        btnNextText.text   = quest.AnswerNext[index];

        btnAccept.SetActive(false);
        btnReject.SetActive(false);
    }

    // (32.11) เช็ค NPC ว่ามีเควส InProgess หรือ New แล้วตั้งค่า Dialogue
    private void SetupDialoguePanel(Npc npc)
    {
        index = 0;

        npcImage.sprite  = npc.AvatarPic;
        npcNameText.text = npc.CharName;

        Quest inProgressQuest = QuestManager.instance.CheckForQuest(npc, QuestStatus.InProgess);

        if (inProgressQuest != null) // There is an In-Progress Quest going on
        {
            Debug.Log($"in-progress: {inProgressQuest}");
            dialogueText.text = inProgressQuest.QuestionInProgress;

            bool hasItem = QuestManager.instance.CheckIfFinishQuest();
            Debug.Log(hasItem);

            if (hasItem) // has item to finish quest
            {
                btnFinishText.text = inProgressQuest.AnswerFinish;
                btnFinish.SetActive(true);
            }
            else
            {
                btnNotFinishText.text = inProgressQuest.AnswerNotFinish;
                btnNotFinish.SetActive(true);
            }
        }
        else // Check for New Quest
        {
            Quest newQuest = QuestManager.instance.CheckForQuest(npc, QuestStatus.New);
            //Debug.Log(newQuest);

            if (newQuest != null) // There is a new Quest
                StartQuestDialogue(newQuest);
        }
    }

    // (32.11) เปิด/ปิด Dialogue Panel + Pause เกมผ่าน Toggle
    private void ToggleDialogueBox(bool flag)
    {
        downPanel.SetActive(!flag);
        npcDialoguePanel.SetActive(flag);
        togglePauseUnpause.isOn = flag;
    }

    // (32.11) เรียกทั้ง 3 เมธอดเพื่อเปิด Dialogue (public — เรียกจาก Hero)
    public void PrepareDialogueBox(Npc npc)
    {
        ClearDialogueBox();
        SetupDialoguePanel(npc);
        ToggleDialogueBox(true);
    }

    // (33.4) ปุ่ม ButtonNext → ไป Dialogue Step ถัดไป
    public void AnswerNext() // map with ButtonNext
    {
        index++;
        dialogueText.text = QuestManager.instance.NextDialogue(index);

        if (QuestManager.instance.CheckLastDialogue(index)) // last dialogue
        {
            btnNext.SetActive(false);

            btnAcceptText.text = QuestManager.instance.CurQuest.AnswerAccept;
            btnAccept.SetActive(true);

            btnRejectText.text = QuestManager.instance.CurQuest.AnswerReject;
            btnReject.SetActive(true);
        }
        else
        {
            btnNext.SetActive(true);
            btnNextText.text = QuestManager.instance.CurQuest.AnswerNext[index];
        }
    }

    // (33.9) ปุ่ม ButtonReject → ปฏิเสธ Quest
    public void AnswerReject() // map with ButtonReject
    {
        QuestManager.instance.RejectQuest();
        ToggleDialogueBox(false);
    }

    // (33.9) ปุ่ม ButtonAccept → รับ Quest
    public void AnswerAccept() // map with ButtonAccept
    {
        QuestManager.instance.AcceptQuest();
        ToggleDialogueBox(false);
    }

    // (33.16) ปุ่ม ButtonFinish → ส่งของและรับรางวัล
    public void AnswerFinish() // map with ButtonFinish
    {
        Debug.Log("Can Finish Quest");
        bool success = QuestManager.instance.DeliverItem();

        if (success)
        {
            if (QuestManager.instance.NpcGiveReward())
            {
                Debug.Log("Quest Completed");
                ToggleDialogueBox(false);
            }
        }
    }

    // (33.16) ปุ่ม ButtonNotFinish → ยังไม่พร้อมส่ง
    public void AnswerNotFinish() // map with ButtonNotFinish
    {
        ToggleDialogueBox(false);
    }

    // ══════════════════════════════════════════════════
    // ITEM DIALOG  (29.9)
    // ══════════════════════════════════════════════════

    // (29.9) รับค่าไอเทมที่คลิกขวา และ Slot ที่อยู่
    public void SetCurItemInUse(ItemDrag drag, int slotId)
    {
        curItemDrag = drag;
        curSlotID = slotId;
    }

    // (29.9) เปิด/ปิด ItemDialog และ GrayImage — รับ flag เพื่อเปิด (true) หรือปิด (false)
    public void ToggleItemDialog(bool flag)
    {
        if (grayImage != null) grayImage.SetActive(flag);
        if (itemDialog != null) itemDialog.SetActive(flag);
    }

    // (29.9) ลบไอคอนไอเทมออกจาก Inventory หลังดื่มยา
    public void DeleteItemIcon()
    {
        if (curItemDrag != null)
        {
            Destroy(curItemDrag.gameObject);
            curItemDrag = null;
        }
    }

    // (29.9) ปุ่ม Use — สั่งดื่มยาแล้วปิด Dialog
    public void ClickDrinkConsumable()
    {
        if (curItemDrag == null) return;
        InventoryManage.instance.DrinkConsumableItem(curItemDrag.Item, curSlotID);
        DeleteItemIcon();
        ToggleItemDialog(false);
    }
}
