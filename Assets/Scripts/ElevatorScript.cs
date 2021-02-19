using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorScript : MonoBehaviour {
    private Animator anim;
    private Transform p1;
    private Transform p2;
    private Collider2D levelEndTrigger;

    int OpenDoorHash = Animator.StringToHash("Activate");

    // Start is called before the first frame update
    void Start() {
        p1 = GameObject.FindGameObjectWithTag("Player1").transform;
        p2 = GameObject.FindGameObjectWithTag("Player2").transform;

        anim = gameObject.GetComponent<Animator>();
        levelEndTrigger = gameObject.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (levelEndTrigger.OverlapPoint(p1.position) || levelEndTrigger.OverlapPoint(p2.position)) {
            OpenDoor();
        }
        if(anim.GetCurrentAnimatorStateInfo(0).IsName("Open")) {
            EndLevel();
        }
    }

    private void OpenDoor() {
        Debug.Log("Door Open");
        anim.SetTrigger(OpenDoorHash);
    }

    private void EndLevel() {
        Debug.Log("Level ended!");
    }
}
