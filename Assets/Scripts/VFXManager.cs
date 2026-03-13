using System;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject doubleRingMarker;
    [SerializeField] private GameObject[] magicVFX;
    [SerializeField] private float spawnYOffset = 1.5f;
    
    [SerializeField] private MagicData[] magicData;
    public MagicData[] MagicData {get{return magicData;}}
    public GameObject[] MagicVFX
    {
        get { return magicVFX; }
    }
    
    public GameObject DoubleRingMarker
    {
        get { return doubleRingMarker; }
    }

    public static VFXManager instance;

    private void Awake()
    {
        instance = this;

    }

    public void LoadMagic(int id, Vector3 posA, float time)
    {
        if (magicVFX[id] == null)
        {
            return;
        }

        Vector3 spawnPos = posA + Vector3.up * spawnYOffset;
        GameObject objLoad = Instantiate(magicVFX[id], spawnPos, Quaternion.identity);
        Destroy(objLoad,time);
    }

    public void ShootMagic(int id, Vector3 posA, Vector3 posB, float time)
    {
        if (magicVFX[id] == null)
        {
          return;  
        }

        GameObject objShoot = Instantiate(magicVFX[id], posA, Quaternion.identity);
        
        objShoot.transform.position = Vector3.LerpUnclamped(posA, posB+ new Vector3(0,1.5f,0), time);
        Destroy(objShoot,time);
    }

    
}
