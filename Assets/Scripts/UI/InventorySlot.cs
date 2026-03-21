using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private int id;
    public int ID
    { get { return id; } set { id = value; } }

    [SerializeField]
    private InventoryManage inventoryManager;

    void Start()
    {
        // เชื่อมต่อกับ Instance ของ InventoryManager เมื่อเริ่มเกม
        inventoryManager = InventoryManage.instance;
    }
    public void OnDrop(PointerEventData eventData)
    {
        // //Get Item A (ไอเทมที่เราลากมา)
        GameObject objA = eventData.pointerDrag;
        ItemDrag itemDragA = objA.GetComponent<ItemDrag>();
        InventorySlot slotA = itemDragA.IconParent.GetComponent<InventorySlot>();
    
        // //Remove Item A from Slot A (เอาข้อมูลออกจากช่องเดิมในกระเป๋า)
        inventoryManager.RemoveItemInBag(slotA.ID);

        // //There is an Item B in Slot B (ถ้าช่องที่เราจะวางมีไอเทม B อยู่แล้ว)
        if (transform.childCount > 0)
        {
            GameObject objB = transform.GetChild(0).gameObject;
            ItemDrag itemDragB = objB.GetComponent<ItemDrag>();

            // //Set Item B on Slot A (ย้ายไอเทม B ไปที่ช่องเดิมของ A ทั้ง UI และข้อมูล)
            itemDragB.transform.SetParent(itemDragA.IconParent);
            itemDragB.IconParent = itemDragA.IconParent;
            inventoryManager.SaveItemInBag(slotA.ID, itemDragB.Item);
        }

        // //Set Item A on Slot B (วางไอเทม A ลงในช่องใหม่นี้)
        itemDragA.IconParent = transform;
        inventoryManager.SaveItemInBag(id, itemDragA.Item);
    }
}
