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
    void Start()
    {
        myInt = gameObject.GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        myInt.Interacting = interacting;
        if (interacting && Input.GetKeyDown(endInteractionKey)) 
        { 
            interacting = false;
            currentlyInteracting.GetComponent<PlayerMovementController>().IsInteractingWithComputer = false;
            currentlyInteracting = null;
        }
        myCamera.gameObject.GetComponent<CameraHandler>().playerIsControlling = interacting;
    }
    public void OnInteracted(GameObject other)
    {
        interacting = true;
        other.GetComponent<PlayerMovementController>().IsInteractingWithComputer = true;
        currentlyInteracting = other;
    }
}
