using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SimpleRigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed = 1.0f;
    SimpleRigidbody collisionHandler;
    void Start()
    {
        collisionHandler = gameObject.GetComponent<SimpleRigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();
        collisionHandler.SetVelocity(input * Speed);
        Debug.Log(collisionHandler.GetVelocity());
    }
}
