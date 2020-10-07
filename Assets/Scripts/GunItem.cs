using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GunItem : Item
{
    public GunInfo gunInfo;
    public Transform[] projectileSpawn;

    [Header("Recoil")]
    public Vector2 kickMinMax = new Vector2(0.05f, 0.2f);
    public Vector2 recoilAngleMinMax = new Vector2(3, 5);
    public float recoilMoveSettleTime = 0.1f;
    public float recoilRotationSettleTime = 0.1f;

    [Header("Effects")]
    public Transform shell;
    public Transform shellEjection;
    public Transform impactEffect;
    MuzzleFlash muzzleFlash;
    float nextShotTime;

    private Vector3 localPosition;
    private Quaternion localRotation;

    bool triggerReleaseSinceLastShot;
    int shotRemainingInBurst;
    int projectilesRemainingInMag;
    bool isReloading;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;
    float xRotation = 0f;
    float damage = 1;

    private void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();
        shotRemainingInBurst = gunInfo.bursCount;
        projectilesRemainingInMag = gunInfo.projectilesPerMag;
        itemType = ItemType.Gun;
    }

    private void LateUpdate()
    {

        //anime recoil
        transform.localPosition = Vector3.SmoothDamp(transform.localPosition, Vector3.zero, ref recoilSmoothDampVelocity, recoilMoveSettleTime);
        recoilAngle = Mathf.SmoothDamp(recoilAngle, 0, ref recoilRotSmoothDampVelocity, recoilRotationSettleTime);
        //transform.localEulerAngles = transform.localEulerAngles + Vector3.left * recoilAngle;

        if (!isReloading && projectilesRemainingInMag == 0)
        {
            Reload();
        }

    }

    void Shoot(Transform view)
    {
        if (!isReloading && Time.time > nextShotTime && projectilesRemainingInMag > 0)
        {


            if (gunInfo.fireMode == FireMode.Burst)
            {
                if (shotRemainingInBurst == 0)
                {
                    return;
                }
                shotRemainingInBurst--;
            }
            else if (gunInfo.fireMode == FireMode.Single)
            {
                if (!triggerReleaseSinceLastShot)
                {
                    return;
                }
            }
            for (int i = 0; i < projectileSpawn.Length; i++)
            {
                if (projectilesRemainingInMag == 0)
                {
                    break;
                }
                projectilesRemainingInMag--;
                nextShotTime = Time.time + gunInfo.msBetweenShots / 1000;

                RaycastHit hit;
                if (Physics.Raycast(view.position + view.forward * 0.1f, view.forward, out hit, gunInfo.range))
                {
                    OnHitObject(hit.collider, hit.point, hit.normal);
                    
                }
            }
            Instantiate(shell, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();
            transform.localPosition -= Vector3.forward * Random.Range(kickMinMax.x, kickMinMax.y);
            recoilAngle += Random.Range(recoilAngleMinMax.x, recoilAngleMinMax.y);
            recoilAngle = Mathf.Clamp(recoilAngle, 0, 30);
            AudioManager.instance.PlaySound(gunInfo.shootAudio, projectileSpawn[0].position);


        }
    }

    public void Reload()
    {
        if (!isReloading && projectilesRemainingInMag != gunInfo.projectilesPerMag)
        {
            StartCoroutine(AnimateReload());

            if (AudioManager.instance != null)
            {
                AudioManager.instance.PlaySound(gunInfo.reloadAudio, transform.position);
            }
        }
    }

    IEnumerator AnimateReload()
    {
        isReloading = true;
        yield return new WaitForSeconds(0.2f);

        float reloadSpeed = 1 / gunInfo.reloadTime;
        float percent = 0;
        Vector3 initialRot = transform.localEulerAngles;
        float maxReloadAngle = 30;

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;
            float interpolation = (-percent * percent + percent) * 4;
            float reloadAngle = Mathf.Lerp(0, maxReloadAngle, interpolation);
            transform.localEulerAngles = initialRot + Vector3.left * reloadAngle;

            yield return null;
        }

        isReloading = false;
        projectilesRemainingInMag = gunInfo.projectilesPerMag;
    }

    public override void OnTriggerHold(Transform view)
    {
        Shoot(view);
        triggerReleaseSinceLastShot = false;
    }

    public override void OnTriggerRelease()
    {
        triggerReleaseSinceLastShot = true;
        shotRemainingInBurst = gunInfo.bursCount;
    }

    void OnHitObject(Collider c, Vector3 hitPoint, Vector3 normal)
    {
        IDamageable damagableObject = c.GetComponent<IDamageable>();
        if (damagableObject != null)
        {
            //damagableObject.TakeHit(damage, hitPoint, transform.forward);
            c.GetComponent<PhotonView>().RPC("TakeDamage", RpcTarget.All, 1f, PhotonNetwork.LocalPlayer.NickName);
        }

        var effect = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "impactFlesh"), hitPoint, Quaternion.LookRotation(normal));
        effect.transform.parent = c.transform;
    }
}
