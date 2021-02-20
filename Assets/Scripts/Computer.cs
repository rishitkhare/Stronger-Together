using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : MonoBehaviour 
{
    public GameObject myCamera;
    public KeyCode endInteractionKey;
    [HideInInspector]
    public bool interacting;
    private GameObject currentlyInteracting;
    private Interactable myInt;

    private int interactableLeewayFrames;

    void Start()
    {
        myInt = gameObject.GetComponent<Interactable>();
        interactableLeewayFrames = 0;
    }

    // Update is called once per frame
    void Update()
    {
        myInt.Interacting = interacting;
        if (interactableLeewayFrames < 0 && interacting && Input.GetKeyDown(endInteractionKey)) 
        { 
            interacting = false;
            currentlyInteracting.GetComponent<PlayerMovementController>().IsInteractingWithComputer = false;
            currentlyInteracting = null;
        }

        if(interactableLeewayFrames >= 0) { interactableLeewayFrames--; }
        myCamera.gameObject.GetComponent<CameraHandler>().playerIsControlling = interacting;
    }
    public void OnInteracted(GameObject other)
    {
        AudioManager.instance.Play("Hack");
        interacting = true;
        interactableLeewayFrames = 25;
        other.GetComponent<PlayerMovementController>().IsInteractingWithComputer = true;
        currentlyInteracting = other;
    }
}
