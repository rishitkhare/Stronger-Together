using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public enum Role { spy, prisoner }
    public Role character;
    private List<string> Inventory;
    private string myClothing;
    //public GameObject myWinCondition;

    private Animator anim;

    private Canvas UI;
    public TMP_Text cardCount;

    public bool IsDragging { get; set; }
    private readonly int dragHash = Animator.StringToHash("IsDragging");

    public bool IsPunching { get; set; }
    private readonly int punchHash = Animator.StringToHash("IsPunching");

    void Start()
    {
        UI = gameObject.GetComponent<Canvas>();

        Inventory = new List<string>();
        if (character == Role.spy) { myClothing = "spy"; }
        else { myClothing = "prisoner"; }

        anim = gameObject.GetComponent<Animator>();
    }


    void Update() {
        if(cardCount != null) {
            cardCount.text = $"x{Inventory.Count}";
        }

        if (IsPunching) {
            anim.SetBool(punchHash, true);
        }

        if (IsDragging) {
            anim.SetBool(dragHash, true);
        }
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
        if(character == Role.spy)
        {
            for (int i = 0; i < other.gameObject.GetComponent<PlayerScript>().myInventory().Count; i++) {
                addItem(other.gameObject.GetComponent<PlayerScript>().myInventory()[i]);
                other.gameObject.GetComponent<PlayerScript>().myInventory().Remove(other.gameObject.GetComponent<PlayerScript>().myInventory()[i]);
            }
        }
    }
}
