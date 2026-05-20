using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    private Npc[] npcPerson;
    public Npc[] NPCPerson { get { return npcPerson; } set { npcPerson = value; } }

    [SerializeField]
    private QuestData[] questData;
    public QuestData[] QuestData { get { return questData; } set { questData = value; } }

    [SerializeField]
    private Npc curNpc;
    public Npc CurNPC { get { return curNpc; } set { curNpc = value; } }

    [SerializeField]
    private Quest curQuest;
    public Quest CurQuest { get { return curQuest; } set { curQuest = value; } }

    public static QuestManager instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        AddQuestToNPC(npcPerson[0], questData[0]); // Give Golem - Give Potion Quest
    }

    private void AddQuestToNPC(Npc npc, QuestData questData)
    {
        Quest quest = new Quest(questData);
        npc.QuestToGive.Add(quest);
    }

    // เช็คว่า NPC มี Quest ที่มี Status ที่ต้องการหรือไม่
    public Quest CheckForQuest(Npc npc, QuestStatus status)
    {
        curNpc = npc;

        Quest quest = npc.CheckQuestList(status);
        curQuest = quest;

        return quest;
    }

    // เช็คว่ามีไอเทมสำหรับ Delivery หรือไม่ (private)
    private bool CheckItemToDelivery()
    {
        return InventoryManage.instance.CheckPartyForItem(curQuest.QuestItemId);
    }

    // เช็คว่าสามารถ Finish Quest ได้หรือไม่
    public bool CheckIfFinishQuest()
    {
        bool success = false;
        Debug.Log(curQuest.Type);

        switch (curQuest.Type)
        {
            case QuestType.Delivery:
                success = CheckItemToDelivery();
                break;
        }
        return success;
    }

    // เช็คว่าเป็น Dialogue บรรทัดสุดท้ายหรือไม่
    public bool CheckLastDialogue(int i)
    {
        if (i == curQuest.QuestDialogue.Length - 1)
            return true;
        else
            return false;
    }

    // ดึง Dialogue บรรทัดที่ i
    public string NextDialogue(int i) // map with ButtonNext
    {
        if (i < curQuest.QuestDialogue.Length)
            return curQuest.QuestDialogue[i];
        else
            return "";
    }

    // รับ Quest
    public void AcceptQuest()
    {
        curQuest.Status = QuestStatus.InProgess;
        PartyManager.instance.QuestList.Add(curQuest);
    }

    // ปฏิเสธ Quest
    public void RejectQuest()
    {
        curQuest.Status = QuestStatus.Reject;
    }

    // เอาไอเทมออกจากกระเป๋า (ส่ง Quest)
    public bool DeliverItem()
    {
        return InventoryManage.instance.RemoveItemFromParty(curQuest.QuestItemId);
    }

    // NPC แจกรางวัล Item ให้ party
    public bool NpcGiveReward()
    {
        if (PartyManager.instance.SelectChars.Count == 0)
            return false;

        Characters hero = PartyManager.instance.SelectChars[0];

        Item item = new Item(InventoryManage.instance.ItemData[curQuest.RewardItemId]);

        for (int i = 0; i < 16; i++)
        {
            if (hero.InventoryItems[i] == null)
            {
                hero.InventoryItems[i] = item;
                curQuest.Status = QuestStatus.Finish;
                return true;
            }
        }
        return false;
    }
}
