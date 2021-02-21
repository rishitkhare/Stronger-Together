using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Corpse : MonoBehaviour
{
    bool isBeingPulled;
    GameObject pulling;
    Vector2 dir;
    Vector2 velocity;
    string clothing;

    private readonly int startDragHash = Animator.StringToHash("IsStartDragging");
    private readonly int dragHash = Animator.StringToHash("IsDragging");
    private readonly int endDragHash = Animator.StringToHash("IsEndDragging");


    void Start()
    {
        pulling = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(isBeingPulled && (pulling.gameObject.GetComponent<SimpleRigidbody>().GetVelocity().x != 0 || pulling.gameObject.GetComponent<SimpleRigidbody>().GetVelocity().y != 0))
        {
            float myVelx = pulling.gameObject.GetComponent<SimpleRigidbody>().GetVelocity().x * Math.Abs(dir.x) * Time.deltaTime;
            float myVely = pulling.gameObject.GetComponent<SimpleRigidbody>().GetVelocity().y * Math.Abs(dir.y) * Time.deltaTime;
            transform.position += new Vector3(myVelx, myVely, 0);
        }
    }

    public void OnPullingStart(GameObject other)
    {
        Animator anim = other.gameObject.GetComponent<Animator>();

        anim.SetBool(startDragHash, true);
        other.gameObject.GetComponent<PlayerScript>().IsDragging = true; 

        Debug.Log("pulling start");
        isBeingPulled = true;
        if(other.transform.position.x > transform.position.x)
        {
            if(Math.Abs(other.transform.position.y - transform.position.y) > other.transform.position.x - transform.position.x)
            {
                if (other.transform.position.y - transform.position.y < 0) { dir = new Vector2(0, -1); }
                else { dir = new Vector2(0, 1); }
            }
            else
            {
                dir = new Vector2(1, 0);
            }
        }
        else if(other.transform.position.x <= transform.position.x)
        {
            if (Math.Abs(other.transform.position.y - transform.position.y) > Math.Abs(other.transform.position.x - transform.position.x))
            {
                if (other.transform.position.y - transform.position.y < 0) { dir = new Vector2(0, -1); }
                else { dir = new Vector2(0, 1); }
            }
            else
            {
                dir = new Vector2(1, 0);
            }
        }
        pulling = other;
    }

    public void OnPullingEnd(GameObject other)
    {
        Animator anim = other.gameObject.GetComponent<Animator>();

        other.gameObject.GetComponent<PlayerScript>().IsDragging = false;
        anim.SetBool(endDragHash, true);
        isBeingPulled = false;
        pulling = null;
    }

    public void setClothing(string clothes) { clothing = clothes; }
}
