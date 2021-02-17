using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public enum Type { KeyCard, Body}
    public Type myType;
    public string id;
    private GameObject bodyController;
    private bool IsBeingControlled;
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteracted(GameObject other)
    {
        if(myType == Type.KeyCard)
        {
            other.gameObject.GetComponent<PlayerSystem>().addItem(id);
            gameObject.SetActive(false);
        }
        else if(myType == Type.Body)
        {
            
        }
    }
}
