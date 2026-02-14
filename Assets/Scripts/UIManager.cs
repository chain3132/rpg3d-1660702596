using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform selectionBox;
    public RectTransform SelectionBox
    {
        get { return selectionBox; }
    }

    public static UIManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void ToggleAI(bool isOn)
    {
        foreach (var member in PartyManager.instance.Members)
        {
            AttackAI ai = member.gameObject.GetComponent<AttackAI>();
            Debug.Log(ai.gameObject.name);
            if (ai != null)
            {
                ai.enabled = isOn;
                Debug.Log(ai.enabled);
            }
        }
    }
}
