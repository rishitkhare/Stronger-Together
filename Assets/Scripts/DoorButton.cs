using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DoorButton : MonoBehaviour
{
    public UnityEvent opened;
    void Start()
    {
        if(opened == null)
        {
            opened = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteracted(GameObject other)
    {
        if (other.gameObject.GetComponent<PlayerSystem>().getInventory().Contains("KeyCard"))
        {
            other.gameObject.GetComponent<PlayerSystem>().getInventory().Remove("KeyCard");
            opened.Invoke();
        }
    }
}