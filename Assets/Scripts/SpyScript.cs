using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpyScript : MonoBehaviour
{
    private List<string> Inventory;
    private string myClothing;

    void Start()
    {
        Inventory = new List<string>();
        myClothing = "spy";
    }

    
    void Update()
    {
        
    }

    public void changeClothing(string newClothes)
    {
        myClothing = newClothes;
    }

    public void addItem(string item)
    {
        Inventory.Add(item);
    }
}
