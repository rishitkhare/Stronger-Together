using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableContinuous : Interactable
{
    public UnityEvent<GameObject> OnInteractionEnded;
    private bool interacting = false;
    public override void Start()
    {
        if (OnInteractionEnded == null) { OnInteractionEnded = new UnityEvent<GameObject>(); }
        base.Start();
    }

    public override void Update()
    {
        if(CheckIfInteracting(out GameObject other))
        {
            OnInteracted.Invoke(other);
            interacting = true;
        }
        else if(interacting)
        {
            OnInteractionEnded.Invoke(other);
            interacting = false;
        }
    }

    public bool CheckIfInteracting(out GameObject other)
    {
        float dist1 = 0; float dist2 = 0;
        if (!DisablePlayer1) { dist1 = (GameObject.Find(Player1ObjectName).transform.position - transform.position).magnitude; }
        if (!DisablePlayer2) { dist2 = (GameObject.Find(Player2ObjectName).transform.position - transform.position).magnitude; }
        if (!DisablePlayer1 && (Input.GetKey(Player1InteractionKey) || myTriggerMethod == EventTrigger.Collision) && dist1 < MaxDist)
            { other = GameObject.Find(Player1ObjectName); return true; }
        if (!DisablePlayer2 && (Input.GetKey(Player2InteractionKey) || myTriggerMethod == EventTrigger.Collision) && dist2 < MaxDist)
            { other = GameObject.Find(Player2ObjectName); return true; }
        other = null;
        return false;
    }
}
