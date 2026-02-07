using System;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject doubleRingMarker;
    public GameObject DoubleRingMarker
    {
        get { return doubleRingMarker; }
    }

    public static VFXManager instance;

    private void Start()
    {
        instance = this;
    }

    
}
