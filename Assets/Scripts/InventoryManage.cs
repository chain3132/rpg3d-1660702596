using UnityEngine;
using UnityEngine.TextCore.Text;
using System.Collections.Generic;

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

        switch (index)
        {
            case SHIELD_SLOT:
                PartyManager.instance.SelectChars[0].EquipShield(item);
                break;
            case WEAPON_SLOT:
                PartyManager.instance.SelectChars[0].EquipWeapon(item);
                break;
        }
    }

    public void RemoveItemInBag(int index)
    {
        if (PartyManager.instance.SelectChars.Count == 0)
            return;

        PartyManager.instance.SelectChars[0].InventoryItems[index] = null;

        switch (index)
        {
            case SHIELD_SLOT:
                PartyManager.instance.SelectChars[0].UnequipShield();
                break;
            case WEAPON_SLOT:
                PartyManager.instance.SelectChars[0].UnequipWeapon();
                break;
        }
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
    // (31.23) เช็คว่า party มีไอเทมสำหรับส่ง Quest หรือไม่ (ค้นทุกคนใน party)
    public bool CheckPartyForItem(int id)
    {
        Item item = new Item(itemData[id]);
        Debug.Log(item.ItemName);

        List<Characters> party = PartyManager.instance.Members;

        foreach (Characters hero in party)
        {
            for (int i = 0; i < hero.InventoryItems.Length; i++)
            {
                if (hero.InventoryItems[i] == null)
                    continue;

                Debug.Log(hero.InventoryItems[i].ItemName);

                if (hero.InventoryItems[i].ID == item.ID)
                    return true;
            }
        }
        return false;
    }

    // (33.14) เอาไอเทม Quest ออกจากกระเป๋าของ party
    public bool RemoveItemFromParty(int id)
    {
        Item item = new Item(itemData[id]);
        Debug.Log($"Finding {item.ItemName}");

        List<Characters> party = PartyManager.instance.Members;

        foreach (Characters hero in party)
        {
            for (int i = 0; i < hero.InventoryItems.Length; i++)
            {
                if (hero.InventoryItems[i] == null)
                    continue;

                if (hero.InventoryItems[i].ID == item.ID)
                {
                    Debug.Log($"Removing {hero.InventoryItems[i].ItemName}");
                    hero.InventoryItems[i] = null;
                    Debug.Log($"Removed {hero.InventoryItems[i]}");
                    return true;
                }
            }
        }
        return false;
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
