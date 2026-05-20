using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PartyManager : MonoBehaviour
{
    [SerializeField] private List<Characters> selectChars = new List<Characters>();
    [SerializeField] private List<Characters> members = new List<Characters>();
    public List<Characters> Members
    {
        get { return members; }
    }
    public List<Characters> SelectChars
    {
        get { return selectChars; }
    }

    public static PartyManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        foreach (Characters c in members)
        {
            c.CharInit(VFXManager.instance, UIManager.instance,InventoryManage.instance);
        }

        SelectSingleHero(0);

        members[0].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[0]));
        members[0].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[1]));
        members[0].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[2]));

        members[1].MagicSkills.Add(new Magic(VFXManager.instance.MagicData[1]));

        InventoryManage.instance.AddItem(members[0], 0); // Health Potion
        InventoryManage.instance.AddItem(members[0], 1); // Sword

        InventoryManage.instance.AddItem(members[1], 0); // Health Potion
        InventoryManage.instance.AddItem(members[1], 1); // Sword
        InventoryManage.instance.AddItem(members[1], 2); // Shield
        InventoryManage.instance.AddItem(members[1], 3); 
        InventoryManage.instance.AddItem(members[1], 4); 


        UIManager.instance.ShowMagicToggle();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (selectChars.Count > 0)
            {
                Debug.Log("M");
                selectChars[0].IsMagicMode = true;
                selectChars[0].CurMagicCast = selectChars[0].MagicSkills[0];
            }
        }
    }

    public void SelectSingleHero(int i)
    {
        foreach (var c in selectChars)
        {
            c.ToggleRingSelection(false);
        }
        selectChars.Clear();
        
        selectChars.Add(members[i]);
        selectChars[0].ToggleRingSelection(true);
    }

    public void HeroSelectMagicSkill(int i)
    {
        if (selectChars.Count <= 0)
        {
            return;
        }

        selectChars[0].IsMagicMode = true;
        selectChars[0].CurMagicCast = selectChars[0].MagicSkills[i];
    }
}
