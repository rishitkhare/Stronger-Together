using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class DoorButton : MonoBehaviour
{
    public Sprite openButton;
    private SpriteRenderer sr;
    public UnityEvent opened;
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();

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
        if(other.GetComponent<PlayerScript>().myInventory().Contains("KeyCard"))
        {
            Debug.Log(other.GetComponent<PlayerScript>().myInventory().Remove("KeyCard"));
            opened.Invoke();
            sr.sprite = openButton;
        }
    }
}
