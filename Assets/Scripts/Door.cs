using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Door : MonoBehaviour
{
    public enum DoorState { open, closed}

    private DoorState myDoorState;

    private Collider2D col;
    private Collider2D cameraObstructor;
    private ShadowCaster2D shadow;

    private Animator anim;
    private readonly int doorIsOpenHash = Animator.StringToHash("IsUnlocked");

    void Start()
    {
        myDoorState = DoorState.closed;
        col = gameObject.GetComponent<Collider2D>();
        cameraObstructor = transform.Find("Camera Collider").GetComponent<Collider2D>();
        anim = gameObject.GetComponent<Animator>();
        shadow = gameObject.GetComponent<ShadowCaster2D>();
    }


    void Update()
    {
        bool open = IsOpened(myDoorState);
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Open")) {
            col.enabled = false;
            cameraObstructor.enabled = false;
            shadow.enabled = false;
        }
        else {
            col.enabled = true;
            cameraObstructor.enabled = true;
            shadow.enabled = true;
        }
        anim.SetBool(doorIsOpenHash, open);

        if(Input.GetKeyDown("n")) {
            OnOpened();
        }
    }

    public void OnOpened()
    {
        myDoorState = DoorState.open;
    }

    private bool IsOpened(DoorState state) {
        switch (state) {
            case (DoorState.open):
                return true;
            case (DoorState.closed):
                return false;
        }

        throw new System.ArgumentException();
    }
}
