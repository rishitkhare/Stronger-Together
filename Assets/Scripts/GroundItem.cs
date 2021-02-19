using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundItem : MonoBehaviour
{
    public string id;
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
        other.GetComponent<PlayerScript>().addItem(id);
        Debug.Log("added");
        Destroy(gameObject);
    }
}
