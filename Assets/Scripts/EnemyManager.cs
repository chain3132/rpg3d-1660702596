using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private List<Enemy> monsters;
    public List<Enemy> Monsters
    { get { return monsters; } }

    public static EnemyManager instance;

    void Awake()
    {
        // กำหนด Singleton instance
        instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // ทำการตั้งค่าเริ่มต้น (Initialize) ให้กับมอนสเตอร์ทุกตัวในลิสต์
        foreach (Characters m in monsters)
        {
            m.CharInit(VFXManager.instance, UIManager.instance, InventoryManage.instance);
        }

        // ตัวอย่างการเพิ่มไอเทมเริ่มต้นให้กับมอนสเตอร์ตัวแรก (ดัชนีที่ 0)
        InventoryManage.instance.AddItem(monsters[0], 0); // Health Potion
        InventoryManage.instance.AddItem(monsters[0], 1); // Sword
        InventoryManage.instance.AddItem(monsters[0], 2); // Shield
    }
}
