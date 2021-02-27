using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string ItemHeld { get; set; }
    public string myClothes;
    public GameObject BodyPrefab;
    private Animator anim;
    private int IsKOHash = Animator.StringToHash("IsKOed");
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
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
        anim.SetBool(IsKOHash, true);
        Instantiate(BodyPrefab, transform.position, Quaternion.identity);
        OnPickPocket(prisoner);
        Destroy(gameObject);
    }
}
