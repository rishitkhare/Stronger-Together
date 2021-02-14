using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    bool IsInteracting;
    public string Player1ObjectName;
    public string Player2ObjectName;
    public KeyCode Player1InteractionKey;
    public KeyCode Player2InteractionKey;
    public float MaxDist;
    public UnityEvent OnInteracted;
    public bool DisablePlayer1;
    public bool DisablePlayer2;
    void Start()
    {
        if(OnInteracted == null)
        {
            OnInteracted = new UnityEvent();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckInteractions();
    }

    void CheckInteractions()
    {
        float dist1 = 0; float dist2 = 0;
        if (!DisablePlayer1) { dist1 = (GameObject.Find(Player1ObjectName).transform.position - transform.position).magnitude; }
        if (!DisablePlayer2) { dist2 = (GameObject.Find(Player2ObjectName).transform.position - transform.position).magnitude; }
        if (!DisablePlayer1 && Input.GetKeyDown(Player1InteractionKey) && dist1 < MaxDist) { OnInteracted.Invoke(); }
        if (!DisablePlayer2 && Input.GetKeyDown(Player2InteractionKey) && dist2 < MaxDist) { OnInteracted.Invoke(); }
    }
}
