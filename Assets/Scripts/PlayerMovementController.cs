using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SimpleRigidbody))]
public class PlayerMovementController : MonoBehaviour
{
    // Start is called before the first frame update
    public float Speed = 1.0f;
    public string inputHorizontalName;
    public float sneakFactor;
    public float sneakNoiseReduction;
    public KeyCode sneakButton;
    public string inputVerticalName;
    SimpleRigidbody collisionHandler;
    public float footStepsNoise;
    [HideInInspector]
    public bool IsInteractingWithComputer { get; set; }
    void Start()
    {
        collisionHandler = gameObject.GetComponent<SimpleRigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw(inputHorizontalName), Input.GetAxisRaw(inputVerticalName));
        input.Normalize();
        if(!IsInteractingWithComputer)
        {
            if (Input.GetKey(sneakButton)) { collisionHandler.SetVelocity((input * Speed) / sneakFactor); }
            else { collisionHandler.SetVelocity(input * Speed); }
            if (collisionHandler.GetVelocity() != Vector2.zero)
            {
                
            }
        }
    }
}
