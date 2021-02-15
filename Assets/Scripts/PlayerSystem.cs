using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    private List<string> Inventory;
    public string otherPlayerName;
    void Start()
    {
        Inventory = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnInteracted(GameObject other)
    {

    }

    public void addItem(string item) { Inventory.Add(item); }
    public List<string> getInventory() { return Inventory; }
}
