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

    private void Start()
    {
        foreach (var c in members)
        {
            c.CharInit(VFXManager.instance,UIManager.instance);
        }
        SelectSingleHero(0);
        members[0].MagicSkills.Add(new Magic(0,"Power Glow",10f,20,3f,1f,2,2));
        members[1].MagicSkills.Add(new Magic(0,"Fire Ball",10f,35,3f,4f,0,1));
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
