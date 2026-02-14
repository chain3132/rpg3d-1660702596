using System;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform selectionBox;
    [SerializeField] private Toggle togglePauseUnpause;
    public RectTransform SelectionBox
    {
        get { return selectionBox; }
    }

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
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
}
