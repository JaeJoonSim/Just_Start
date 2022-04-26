using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsHandPresence : MonoBehaviour
{
    public Transform target;
    private Rigidbody rb;
    public Renderer nonPhysicalHand;
    public float showNonPhysicalHandDistance = 0.05f;
    private Collider[] handColliders;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        handColliders = GetComponentsInChildren<Collider>();

        rb.maxAngularVelocity = 200;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > showNonPhysicalHandDistance)
        {
            nonPhysicalHand.enabled = true;
        }
        else
            nonPhysicalHand.enabled = false;
    }

    void FixedUpdate()
    {
        rb.velocity = (target.position - transform.position) / Time.fixedDeltaTime;

        Quaternion rotationDifference = target.rotation * Quaternion.Inverse(transform.rotation);
        rotationDifference.ToAngleAxis(out float angleInDegree, out Vector3 rotationAxis);

        Vector3 rotationDifferenceInDegee = angleInDegree * rotationAxis;
        rb.angularVelocity = (rotationDifferenceInDegee * Mathf.Deg2Rad / Time.fixedDeltaTime);
    }

    public void EnableHandCollider()
    {
        foreach(var item in handColliders)
        {
            item.enabled = true;
        }
    }

    public void EnableHandColliderDelay(float delay)
    {
        Invoke("EnableHandCollider", delay);
    }

    public void DisableHandCollider()
    {
        foreach (var item in handColliders)
        {
            item.enabled = false;
        }
    }
}
