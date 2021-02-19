using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public enum Role { spy, prisoner }
    public Role character;
    private List<string> Inventory;
    private string myClothing;
    //public GameObject myWinCondition;
    void Start()
    {
        Inventory = new List<string>();
        if (character == Role.spy) { myClothing = "spy"; }
        else { myClothing = "prisoner"; }
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

    public void OnInteracted(GameObject other)
    {
        if(character == Role.prisoner)
        {
            foreach(string item in Inventory)
            {
                other.gameObject.GetComponent<PlayerScript>().addItem(item);
                Inventory.Remove(item);
            }
        }
    }
}
