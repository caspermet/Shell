using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask CollisionMask;
    float speed = 10;
    float damage = 1;

    float livetime = 3;
    float skinWidth = 0.1f;

    private void Start()
    {
        Destroy(gameObject, livetime);

        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, 0.1f, CollisionMask);

        if(initialCollisions.Length > 0)
        {
            OnHitObject(initialCollisions[0]);
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    void Update()
    {
        float moveDistance = speed * Time.deltaTime;
        ChechCollisions(moveDistance);
        transform.Translate(Vector3.forward * moveDistance);
    }

    void ChechCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, moveDistance + skinWidth, CollisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        IDamageable damagableObject = hit.collider.GetComponent<IDamageable>();
        if(damagableObject != null)
        {
            damagableObject.TakeHit(damage, hit);
        }
        GameObject.Destroy(gameObject);
    }

    void OnHitObject(Collider c)
    {
        IDamageable damagableObject = c.GetComponent<IDamageable>();
        if (damagableObject != null)
        {
            damagableObject.TakeDamage(damage);
        }
        GameObject.Destroy(gameObject);
    }
}
