using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class InteractionEvent : UnityEvent<GameObject> { }
public class Interactable : MonoBehaviour
{
    public enum EventTrigger { Key, Collision}
    public EventTrigger myTriggerMethod;
    public string Player1ObjectName;
    public string Player2ObjectName;
    public KeyCode Player1InteractionKey;
    public KeyCode Player2InteractionKey;
    public float MaxDist;
    public bool DisablePlayer1;
    public bool DisablePlayer2;
    public UnityEvent<GameObject> OnInteracted;
    [HideInInspector]
    public bool Interacting;
    public virtual void Start()
    {
        if(OnInteracted == null)
        {
            OnInteracted = new InteractionEvent();
        }
        Interacting = false;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (!Interacting) { CheckInteractions(); }
    }

    void CheckInteractions()
    {
        float dist1 = 0; float dist2 = 0;
        if (!DisablePlayer1) { dist1 = (GameObject.Find(Player1ObjectName).transform.position - transform.position).magnitude; }
        if (!DisablePlayer2) { dist2 = (GameObject.Find(Player2ObjectName).transform.position - transform.position).magnitude; }
        if (!DisablePlayer1 && (Input.GetKeyDown(Player1InteractionKey) || myTriggerMethod == EventTrigger.Collision) && dist1 < MaxDist) 
            { OnInteracted.Invoke(GameObject.Find(Player1ObjectName)); }
        if (!DisablePlayer2 && (Input.GetKeyDown(Player2InteractionKey) || myTriggerMethod == EventTrigger.Collision) && dist2 < MaxDist) 
            { OnInteracted.Invoke(GameObject.Find(Player2ObjectName)); }
    }
}
