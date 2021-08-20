using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarouselePiece : MonoBehaviour
{
    [SerializeField] private GameObject body;
    Rigidbody rb;
    [Range(0f, 1f)][SerializeField] private float drag = 0.5f;
    private Vector3 dragDirection = Vector3.zero;
    private float dragMagnitude = 0;


    void Awake()
    {
    rb = body.GetComponent<Rigidbody>();
    }    

    public void AddImpulse(Vector3 imp)
    {
        rb.AddForce(imp, ForceMode.Impulse);
    }


    void FixedUpdate()
    {
        dragDirection = - rb.velocity.normalized;
        dragMagnitude = rb.velocity.sqrMagnitude*drag;
        rb.AddForce(dragMagnitude*dragDirection);
    }
}
