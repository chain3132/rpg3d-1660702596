using System;
using UnityEngine;

public class LeftClick : MonoBehaviour
{
    public static LeftClick instance;

    private Camera cam;

    [SerializeField] private Characters curChar;
    public Characters CurCur
    {
        get { return curChar; }
    }

    [SerializeField] private LayerMask layerMask;

    private void Start()
    {
        instance = this;
        cam = Camera.main;
        layerMask  = LayerMask.GetMask("Ground","Character","Building","Item");
        
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClearEverything();
        }
        if (Input.GetMouseButtonUp(0))
        {
            TrySelect(Input.mousePosition);
        }
    }

    private void SelectCharacter(RaycastHit hit)
    {
        curChar = hit.collider.GetComponent<Characters>();
        Debug.Log("Selected Char : "+ hit.collider.gameObject);
        if (curChar != null)
        {
            curChar.ToggleRingSelection(true);
        }
    }

    private void TrySelect(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray,out hit,1000,layerMask))
        {
            switch (hit.collider.tag)
            {
                case "Player":
                case "Hero":
                    SelectCharacter(hit);
                    break;
            }
        }
    }

    private void ClearRingSelection()
    {
        if (curChar != null)
        {
            curChar.ToggleRingSelection(false);
        }
    }

    private void ClearEverything()
    {
        ClearRingSelection();
        curChar = null;
    }
}
