using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public enum DoorState { open, closed}
    public DoorState state;

    void Start()
    {
        
    }


    void Update()
    {
        
    }

    public void OnOpened()
    {
        state = DoorState.open;
    }
}
