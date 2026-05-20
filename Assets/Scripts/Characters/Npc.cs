using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Npc : Characters
{
    [SerializeField]
    private List<Quest> questToGive = new List<Quest>();
    public List<Quest> QuestToGive
    {
        get { return questToGive; }
        set { questToGive = value; }
    }

    // ค้นหา Quest ตาม Status
    public Quest CheckQuestList(QuestStatus status)
    {
        foreach (Quest quest in questToGive)
        {
            if (quest.Status == status)
                return quest;
        }
        return null;
    }

    // NPC ไม่ตายจากระบบปกติ
    protected override void Die() { }
    protected override IEnumerator DestroyObject() { yield break; }
}
