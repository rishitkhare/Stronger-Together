using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SimpleRigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed = 1.0f;
    public string inputHorizontalName;
    public string inputVerticalName;
    SimpleRigidbody collisionHandler;
    NoiseManager noiseSystem;
    public float footStepsNoise;
    void Start()
    {
        collisionHandler = gameObject.GetComponent<SimpleRigidbody>();
        noiseSystem = GameObject.Find("NoiseManager").gameObject.GetComponent<NoiseManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw(inputHorizontalName), Input.GetAxisRaw(inputVerticalName));
        input.Normalize();
        collisionHandler.SetVelocity(input * Speed);
        if(collisionHandler.GetVelocity() != Vector2.zero)
        {
            noiseSystem.AddNoise(transform.position, footStepsNoise);
        }
    }
}
