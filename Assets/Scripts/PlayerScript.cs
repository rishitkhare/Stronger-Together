using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    private List<string> Inventory;
    private string myClothing;
    public GameObject myWinCondition;
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

    public string wearing()
    {
        return myClothing;
    }

    public List<string> myInventory()
    {
        return Inventory;
    }

}
