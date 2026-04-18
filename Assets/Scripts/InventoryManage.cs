using UnityEngine;
using UnityEngine.TextCore.Text;

public class InventoryManage : MonoBehaviour
{
    [SerializeField]
    private GameObject[] itemPrefabs;
    public GameObject[] ItemPrefabs
    {
        get { return itemPrefabs; }
        set { itemPrefabs = value; }
    }

    [SerializeField]
    private ItemData[] itemData;
    public ItemData[] ItemData
    {
        get { return itemData; }
        set { itemData = value; }
    }

    public static InventoryManage instance;
    public const int MAXSLOT = 16;

    void Awake()
    {
        instance = this;
    }

    public bool AddItem(Characters character, int id)
    {
        Debug.Log("tryAddItem");
        Item item = new Item(itemData[id]);

        for (int i = 0; i < character.InventoryItems.Length; i++)
        {
            if (character.InventoryItems[i] == null)
            {
                character.InventoryItems[i] = item;
                return true;
            }
        }

        Debug.Log("Inventory Full");
        return false;
    }
    public void SaveItemInBag(int index, Item item)
    {
        // ตรวจสอบว่ามีตัวละครในปาร์ตี้หรือไม่
        if (PartyManager.instance.SelectChars.Count == 0)
            return;

        // บันทึกไอเทมลงในลิสต์ InventoryItems ของตัวละครหลัก (ตัวที่ 0)
        PartyManager.instance.SelectChars[0].InventoryItems[index] = item;
    }

    public void RemoveItemInBag(int index)
    {
        // ตรวจสอบว่ามีตัวละครในปาร์ตี้หรือไม่
        if (PartyManager.instance.SelectChars.Count == 0)
            return;

        // ลบไอเทมออกจากลิสต์ (ตั้งค่าเป็น null)
        PartyManager.instance.SelectChars[0].InventoryItems[index] = null;
    }
    private void SpawnDropItem(Item item, Vector3 pos)
    {
        int id;

        // ตรวจสอบประเภทไอเทมเพื่อเลือก Prefab ที่จะสร้าง (0: ทั่วไป, 1: ของใช้)
        switch (item.Type)
        {
            case ItemType.Consumable:
                id = 1;
                break;
            default:
                id = 0;
                break;
        }

        // สร้างไอเทมขึ้นมาบนโลก 3D
        GameObject itemObj = Instantiate(ItemPrefabs[id], pos, Quaternion.identity);
    
        // เพิ่มสคริปต์ ItemPick ให้กับไอเทมที่สร้างขึ้นใหม่
        itemObj.AddComponent<ItemPick>();

        // ตั้งค่าข้อมูลเริ่มต้นให้กับ ItemPick เพื่อให้ผู้เล่นกลับมาเก็บได้
        ItemPick itemPick = itemObj.GetComponent<ItemPick>();
        itemPick.Init(item, instance, PartyManager.instance);
        itemPick.transform.position += new Vector3(0, 0.5f, 0);
    }

    public void SpawnDropInventory(Item[] items, Vector3 pos)
    {
        // วนลูปไอเทมทั้งหมดในกระเป๋า (หรือที่ส่งมา)
        for (int i = 0; i < items.Length; i++)
        {
            // ถ้าช่องนั้นมีไอเทมอยู่ ให้ทำการ Spawn ลงบนพื้นทีละชิ้น
            if (items[i] != null)
            {
                SpawnDropItem(items[i], pos);
            }
        }
    }
}
