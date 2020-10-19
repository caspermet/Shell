using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FireMode { Auto, Burst, Single };

[CreateAssetMenu(menuName = "FPS/New Gun")]
public class GunInfo : ItemInfo
{
    public FireMode fireMode;
    public bool canPlayerChangeFireMode = false;

    public float msBetweenShots = 100;
    public float muzzleVelocity = 35;
    public int bursCount;
    public int projectilesPerMag;
    public float reloadTime = 0.3f;
    public float range = 100;
    public float damage = 1;
    [Range(0, 15)]
    public float aimingDeviation = 1;

    [Header("Effects")]
    public AudioClip shootAudio;
    public AudioClip reloadAudio;

    private void Awake()
    {
        itemType = ItemType.Gun;
    }
}
