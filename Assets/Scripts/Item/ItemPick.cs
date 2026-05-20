using UnityEngine;

public class ItemPick : MonoBehaviour
{
    [SerializeField]
        private Item item;
        public Item Item
        { get { return item; } }

        private InventoryManage inventoryManager;
        private PartyManager partyManager;

        // เมธอด Init เพื่อเซ็ตค่าเริ่มต้น
        public void Init(Item item, InventoryManage invManager, PartyManager ptyManager)
        {
            this.item = item;
            inventoryManager = invManager;
            partyManager = ptyManager;
        }

        // เมธอด PickUpItem เพื่อใส่เข้ากระเป๋า Hero และทำลายตัวมันเองออกจากฉาก
        private void PickUpItem(Characters hero)
        {
            // ถ้าเพิ่มไอเทมเข้า Inventory สำเร็จ ให้ทำลาย GameObject นี้ทิ้ง
            if (inventoryManager.AddItem(hero, item.ID))
            {
                Destroy(gameObject);
            }
        }

        // เมธอด OnMouseDown เพื่อเช็คการคลิกเมาส์เก็บไอเทมจากผู้เล่น
        private void OnMouseDown()
        {
            Debug.Log("Pick Up");

            // ตรวจสอบว่ามีตัวละครในปาร์ตี้หรือไม่ ถ้ามีให้ตัวละครหลัก (ตัวที่ 0) เป็นคนเก็บ
            if (partyManager.SelectChars.Count > 0)
            {
                Debug.Log("Add to party");
                PickUpItem(partyManager.SelectChars[0]);
            }
        }
}
