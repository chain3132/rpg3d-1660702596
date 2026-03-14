using System;
using UnityEngine;
using System.Collections.Generic;

[Serializable]
public class Magic

{
    
    [SerializeField] private int id;
    public int ID
    {
        get { return id; }
    }
    [SerializeField] private string name;
    public String Name
    {
        get { return name; }
    }
    [SerializeField] private Sprite icon;
    public Sprite Icon
    {
        get { return icon; }
    }
    [SerializeField] private float range;
    public float Range
    {
        get { return range; }
    }
    [SerializeField] private int power;
    public int Power
    {
        get { return power; }
    }
    [SerializeField] private float loadTime;
    public float LoadTime
    {
        get { return loadTime; }
    }
    [SerializeField] private float shootTime;
    public float ShootTime
    {
        get { return shootTime; }
    }
    [SerializeField] private int loadId;
    public int LoadId
    {
        get { return loadId; }
    }
    [SerializeField] private int shootId;
    public int ShootId
    {
        get { return shootId; }
    }

    public Magic(MagicData data)
    {
        this.id = data.id;
        this.name = data.magicName;
        this.icon = data.icon;
        this.range = data.range;
        this.power = data.power;
        this.loadTime = data.loadTime;
        this.shootTime = data.shootTime;
        this.loadId = data.loadId;
        this.shootId = data.shootId;

    }
    
    
    
}
