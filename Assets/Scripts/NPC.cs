using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string ItemHeld { get; set; }
    public string myClothes;
    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnPickPocket(GameObject prisoner)
    {
        if(ItemHeld != null)
        {
            prisoner.GetComponent<PlayerScript>().addItem(ItemHeld);
            ItemHeld = null;
        }
    }

    public void OnKO(GameObject prisoner)
    {
        GameObject myCorpse = new GameObject();
        myCorpse.AddComponent<Corpse>();
        myCorpse.transform.position = transform.position;
        if (ItemHeld != null) 
        {
            prisoner.GetComponent<PlayerScript>().addItem(ItemHeld);
            ItemHeld = null;
        }
        myCorpse.GetComponent<Corpse>().setClothing(myClothes);
        Destroy(gameObject);
    }
}
