using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LivingEntity : MonoBehaviourPunCallbacks, IPunObservable, IDamageable
{
    public int startingHealth;
    public  int health;
    protected bool dead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        health = startingHealth;
    }
    void Update()
    {
        if (health <= 0)
        {
            OnDeath();
        }
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {

        TakeDamage(damage, "");
    }

    [PunRPC]
    public virtual void TakeDamage(float damage, string enemyName)
    {
        if (dead) return;
        if (photonView.IsMine)
        {
            health -= (int)damage;

            if (health <= 0 && !dead)
            {
                photonView.RPC("Die", RpcTarget.All, enemyName);
            }
        }
    }

    [PunRPC]
    public virtual void Die(string enemyName)
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }
        GameObject.Destroy(gameObject);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else if (stream.IsReading)
        {
            health = (int)stream.ReceiveNext();
        }
    }
}
