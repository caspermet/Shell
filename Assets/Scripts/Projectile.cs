using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public LayerMask CollisionMask;
    public Color trailColour;
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
            OnHitObject(initialCollisions[0], transform.position);
        }

        GetComponent<TrailRenderer>().startColor = trailColour;
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
            OnHitObject(hit.collider, hit.point);
        }
    }

    

    void OnHitObject(Collider c, Vector3 hitPoint)
    {
        IDamageable damagableObject = c.GetComponent<IDamageable>();
        if (damagableObject != null)
        {
            damagableObject.TakeHit(damage, hitPoint, transform.forward);
        }
        GameObject.Destroy(gameObject);
    }
}
