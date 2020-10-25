using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GunItem : Item
{
    public GunInfo gunInfo;
    public Transform[] projectileSpawn;
    public Animator animator;

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
    int totalprojectiles;
    bool isReloading;

    Vector3 recoilSmoothDampVelocity;
    float recoilRotSmoothDampVelocity;
    float recoilAngle;
    bool isScoped = false;
    int numberOfAmmo;

    private void Start()
    {
        muzzleFlash = GetComponent<MuzzleFlash>();
        shotRemainingInBurst = gunInfo.bursCount;
        projectilesRemainingInMag = gunInfo.projectilesPerMag;
        totalprojectiles = gunInfo.totalProjectiles;
        itemType = ItemType.Gun;
        animator.SetFloat("ShootSpeed", gunInfo.msBetweenShots / 10f);

        GunAmmoUI.instance.UpdateAmmoUI(projectilesRemainingInMag, totalprojectiles);
    }

    private void LateUpdate()
    {
        if (!isReloading && projectilesRemainingInMag == 0 && totalprojectiles != 0)
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
                Vector3 sideKick = view.forward;
                if (!isScoped)
                {
                    var x = Random.Range(-gunInfo.aimingDeviation, gunInfo.aimingDeviation);
                    var radius = Random.Range(Mathf.Abs(x), gunInfo.aimingDeviation);
                    var y = Mathf.Sqrt(Mathf.Pow(radius, 2) - Mathf.Pow(x, 2));
                    y = Random.Range(0, 2) == 0 ? y : -y;

                    sideKick = Quaternion.AngleAxis(y, view.right) * Quaternion.AngleAxis(x, view.up) * view.forward;
                }

                if (Physics.Raycast(view.position + view.forward * 0.1f , sideKick, out hit, gunInfo.range))
                {
                    OnHitObject(hit.collider, hit.point, hit.normal);
                }

            }
            //Instantiate(shell, shellEjection.position, shellEjection.rotation);
            muzzleFlash.Activate();
            var verticalLookRotation = Random.Range(kickMinMax.x, kickMinMax.y);
            playerController.xRotation -= verticalLookRotation;

            GunAmmoUI.instance.UpdateAmmoUI(projectilesRemainingInMag, totalprojectiles);
            AudioManager.instance.PlaySound(gunInfo.shootAudio, projectileSpawn[0].position);


        }
    }

    public void Reload()
    {
        if (!isReloading && projectilesRemainingInMag != gunInfo.projectilesPerMag && totalprojectiles > 0)
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

        while (percent < 1)
        {
            percent += Time.deltaTime * reloadSpeed;
            yield return null;
        }

        isReloading = false;

        if (totalprojectiles >= gunInfo.projectilesPerMag )
        {
            projectilesRemainingInMag = gunInfo.projectilesPerMag;
        }
        else
        {
            projectilesRemainingInMag = totalprojectiles;
        }

        totalprojectiles -= projectilesRemainingInMag;
        GunAmmoUI.instance.UpdateAmmoUI(projectilesRemainingInMag, totalprojectiles);
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

        animator.SetTrigger("Shoot2");

        var effect = PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "impact/impactFlesh"), hitPoint, Quaternion.LookRotation(normal));
        effect.transform.parent = c.transform;
    }

    void scope(bool aim)
    {
        isScoped = aim;
        animator.SetBool("IsScoped", aim);
    }

    public override void OnTriggetReleaseFire2()
    {
        scope(false);
    }

    public override void OnTriggetHoldFire2()
    {
        scope(true);
    }
}
