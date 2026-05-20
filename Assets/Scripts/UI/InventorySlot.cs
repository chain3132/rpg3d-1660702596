using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField]
    private int id;
    public int ID
    { get { return id; } set { id = value; } }

    // (30.5) ประเภทไอเทมที่ Slot นี้รับได้ (Other = รับทุกประเภท)
    [SerializeField]
    private ItemType slotType = ItemType.Other;
    public ItemType SlotType
    { get { return slotType; } set { slotType = value; } }

    [SerializeField]
    private InventoryManage inventoryManager;

    void Start()
    {
        inventoryManager = InventoryManage.instance;
    }

    public void OnDrop(PointerEventData eventData)
    {
        // Get Item A (ไอเทมที่เราลากมา)
        GameObject objA = eventData.pointerDrag;
        ItemDrag itemDragA = objA.GetComponent<ItemDrag>();
        InventorySlot slotA = itemDragA.IconParent.GetComponent<InventorySlot>();

        // (30.10) เช็คว่า Slot นี้มีประเภทจำกัดไหม เช่น Shield Slot รับเฉพาะ Shield
        if (slotType != ItemType.Other && itemDragA.Item.Type != slotType)
            return;

        // Remove Item A from Slot A (เอาข้อมูลออกจากช่องเดิม — ถ้า Slot A คือ Shield Slot จะ Unequip ด้วย)
        inventoryManager.RemoveItemInBag(slotA.ID);

        // There is an Item B in Slot B (ถ้าช่องที่จะวางมีไอเทม B อยู่แล้ว)
        if (transform.childCount > 0)
        {
            GameObject objB = transform.GetChild(0).gameObject;
            ItemDrag itemDragB = objB.GetComponent<ItemDrag>();

            // Set Item B on Slot A
            itemDragB.transform.SetParent(itemDragA.IconParent);
            itemDragB.IconParent = itemDragA.IconParent;
            inventoryManager.SaveItemInBag(slotA.ID, itemDragB.Item);

            // (30.10) ถอด Item B ออกจาก Slot B (ถ้า Slot B เป็น Equipment Slot จะ Unequip ด้วย)
            inventoryManager.RemoveItemInBag(id);
        }

        // Set Item A on Slot B (วางไอเทม A ลงในช่องใหม่ — ถ้า Slot B คือ Shield/Weapon Slot จะ Equip ด้วย)
        itemDragA.IconParent = transform;
        inventoryManager.SaveItemInBag(id, itemDragA.Item);
    }
}
