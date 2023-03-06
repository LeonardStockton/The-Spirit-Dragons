using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class gunStats : ScriptableObject
{
    public static gunStats instance;

    public float shootRate;
    public string gunName;
    public int shootDist;
    public int shootDamage;
    public int weaponAmmo;
    public GameObject weaponSkin;
}
