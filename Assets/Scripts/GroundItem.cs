using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
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
        
    }
}
