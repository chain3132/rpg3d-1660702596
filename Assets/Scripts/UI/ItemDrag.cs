using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [SerializeField]
    private Item item;
    public Item Item
    { get { return item; } set { item = value; } }

    [SerializeField]
    private Transform iconParent;
    public Transform IconParent
    { get { return iconParent; } set { iconParent = value; } }

    [SerializeField]
    private Image image;
    public Image Image
    { get { return image; } set { image = value; } }

    // (29.10) UIManager สำหรับเปิด ItemDialog เมื่อคลิกขวา
    private UIManager uiManager;
    public UIManager UiManager
    { get { return uiManager; } set { uiManager = value; } }

    public void OnBeginDrag(PointerEventData eventData)
    {
        iconParent = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(iconParent);
        image.raycastTarget = true;
    }

    // (29.10) ค้นหา index ของ Slot ที่เป็น Parent ของไอคอนนี้
    private int FindIndexOfSlotParent()
    {
        InventorySlot slot = iconParent.GetComponent<InventorySlot>();
        if (slot != null)
            return slot.ID;
        return -1;
    }

    // (29.10) ดักจับคลิกขวา — ถ้าเป็น Consumable ให้เปิด ItemDialog
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (item != null && item.Type == ItemType.Consumable)
            {
                int slotId = FindIndexOfSlotParent();
                if (uiManager != null && slotId >= 0)
                {
                    uiManager.SetCurItemInUse(this, slotId);
                    uiManager.ToggleItemDialog();
                }
            }
        }
    }
}
