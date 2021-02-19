using UnityEngine;

public class ElevatorScript : MonoBehaviour {
    public bool detectsP1;
    public bool detectsP2;

    private Animator anim;
    private Transform p1;
    private Transform p2;
    private Collider2D levelEndTrigger;

    [HideInInspector]
    public bool IsOpen { get; set; }

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
        if((detectsP1 && levelEndTrigger.OverlapPoint(p1.position)) ||
            (detectsP2 && levelEndTrigger.OverlapPoint(p2.position))) {
            OpenDoor();
        }
        else {
            CloseDoor();
        }

        IsOpen = anim.GetCurrentAnimatorStateInfo(0).IsName("Open");
    }

    private void OpenDoor() {
        anim.SetBool(OpenDoorHash, true);
    }

    private void CloseDoor() {
        anim.SetBool(OpenDoorHash, false);
    }
}
