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
    public const int MAXSLOT = 18;
    public const int SHIELD_SLOT = 16;
    public const int WEAPON_SLOT = 17;

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
        if (PartyManager.instance.SelectChars.Count == 0)
            return;

        PartyManager.instance.SelectChars[0].InventoryItems[index] = item;

        if (index == SHIELD_SLOT && item.Type == ItemType.Shield)
        {
            GameObject prefab = itemPrefabs[item.PrefabID];
            PartyManager.instance.SelectChars[0].EquipShield(item, prefab);
        }
        else if (index == WEAPON_SLOT && item.Type == ItemType.Weapon)
        {
            GameObject prefab = itemPrefabs[item.PrefabID];
            PartyManager.instance.SelectChars[0].EquipWeapon(item, prefab);
        }
    }

    public void RemoveItemInBag(int index)
    {
        if (PartyManager.instance.SelectChars.Count == 0)
            return;

        if (index == SHIELD_SLOT)
            PartyManager.instance.SelectChars[0].UnequipShield();
        else if (index == WEAPON_SLOT)
            PartyManager.instance.SelectChars[0].UnequipWeapon();

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
    public void DrinkConsumableItem(Item item, int slotId)
    {
        string s = string.Format("Drink: {0}", item.ItemName);
        Debug.Log(s);

        if (PartyManager.instance.SelectChars.Count > 0)
        {
            PartyManager.instance.SelectChars[0].Recover(item.Power);
            RemoveItemInBag(slotId);
        }
    }
}
