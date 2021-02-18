using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.Linq;

public class CameraHandler : MonoBehaviour
{
    public bool playerIsControlling { get; set; }
    public List<string> clothingToAlert;
    public float maxDeviationFromCenter;
    public float rotationSpeed;
    public float center;
    public string Player1;
    public string Player2;

    //public float FOVAngle;
    //public float innerRadius;

    public LayerMask wallLayer;
    public LayerMask playerLayer;

    private BoxCollider2D Player1CameraCollider;
    private BoxCollider2D Player2CameraCollider;

    private readonly int currentAngleAnimatorHash = Animator.StringToHash("Angle");
    private readonly int isControllingAnimatorHash = Animator.StringToHash("IsControlling");

    private float currentAngle;

    private Collider2D SpotlightTrigger;
    public GameObject SpotlightTriggerGameObject;
    public GameObject LightGameObject;

    public Animator SpriteAnimator;

    public UnityEvent OnDetected;

    private GameObject player1;
    private GameObject player2;


    void Start()
    {
        currentAngle = 0;
        if (OnDetected == null) { OnDetected = new UnityEvent(); }

        Player1CameraCollider = GameObject.FindGameObjectWithTag("Player1")
            .transform.Find("CameraCollider").GetComponent<BoxCollider2D>();

        Player2CameraCollider = GameObject.FindGameObjectWithTag("Player2")
            .transform.Find("CameraCollider").GetComponent<BoxCollider2D>();

        SpotlightTrigger = SpotlightTriggerGameObject.GetComponent<Collider2D>();
        player1 = GameObject.Find(Player1);
        player2 = GameObject.Find(Player2);
    }

    // Update is called once per frame
    void Update()
    {
        if(playerIsControlling)
        {
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                if(currentAngle < maxDeviationFromCenter)
                {
                    if (currentAngle + (rotationSpeed * Time.deltaTime) > maxDeviationFromCenter) { currentAngle = maxDeviationFromCenter; }
                    else { currentAngle += rotationSpeed * Time.deltaTime; }
                }
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (currentAngle > maxDeviationFromCenter * -1)
                {
                    if (currentAngle - (rotationSpeed * Time.deltaTime) < -1 * maxDeviationFromCenter) { currentAngle = -1 * maxDeviationFromCenter; }
                    else { currentAngle -= rotationSpeed * Time.deltaTime; }
                }
            }
        }


        SpotlightTriggerGameObject.transform.rotation = Quaternion.AngleAxis(center + currentAngle, Vector3.forward);
        LightGameObject.transform.rotation = Quaternion.AngleAxis(center + currentAngle, Vector3.forward);

        SpriteAnimator.SetFloat(currentAngleAnimatorHash, center + currentAngle);
        SpriteAnimator.SetBool(isControllingAnimatorHash, playerIsControlling);
    }

    void FixedUpdate()
    {
        SpotlightTriggerGameObject.transform.rotation = Quaternion.AngleAxis(center + currentAngle, Vector3.forward);
        if(CheckIfPlayerCollides())
        {
            Debug.Log("Player Detected");
        }
    }

    public bool CheckIfPlayerCollides()
    {
        Vector2[] Player1ColliderVertices = GetVerticesOfBoxCollider(Player1CameraCollider);

        foreach(Vector2 point in Player1ColliderVertices) {
            if(SpotlightTrigger.OverlapPoint(point) && VisionToPointNotObstructed(point)) {
                if(clothingToAlert.Contains(player1.gameObject.GetComponent<PlayerScript>().wearing()))
                {
                    return true;
                }
            }
        }

        Vector2[] Player2ColliderVertices = GetVerticesOfBoxCollider(Player2CameraCollider);

        foreach(Vector2 point in Player2ColliderVertices) {
            if(SpotlightTrigger.OverlapPoint(point) && VisionToPointNotObstructed(point)) {
                if (clothingToAlert.Contains(player2.gameObject.GetComponent<PlayerScript>().wearing()))
                {
                    return true;
                }
            }
        }

        return false;

        //// OLD IMPLEMENTATION
        //Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        //float nonRelativeAngle = center + currentAngle + 90;
        //for(float i = nonRelativeAngle - FOVAngle/2; i <= nonRelativeAngle + FOVAngle/2; i++)
        //{
        //    Vector2 dir = new Vector2(Mathf.Cos(i * (Mathf.PI/180)), Mathf.Sin(i * (Mathf.PI/180)));
        //    RaycastHit2D hitPlayer = Physics2D.Raycast(myPos, dir, innerRadius, playerLayer);
        //    RaycastHit2D hitWall = Physics2D.Raycast(myPos, dir, innerRadius, wallLayer);
        //    DebugRayHits(hitPlayer, myPos, dir, innerRadius);
        //    if(hitPlayer && !hitWall)
        //    {
        //        return true;
        //    }
        //}
        //return false;
    }

    private bool VisionToPointNotObstructed(Vector2 point) {
        Vector2 directionToPlayer = point - (Vector2)(transform.position);
        float distanceToPlayer = directionToPlayer.magnitude;
        directionToPlayer.Normalize();

        RaycastHit2D rayHitsWall = Physics2D.Raycast(transform.position, directionToPlayer.normalized, distanceToPlayer, wallLayer);
        DebugRayHits(rayHitsWall, transform.position, directionToPlayer.normalized, distanceToPlayer);

        return !rayHitsWall;
    }

    private Vector2[] GetVerticesOfBoxCollider(BoxCollider2D col)
    {
        Vector2[] output = new Vector2[4];

        //put them all at the center, then move them to the corners using size.
        for (int i = 0; i < output.Length; i ++) {
            output[i] = col.transform.position;
            output[i].x += col.offset.x;
            output[i].y += col.offset.y;
        }

        //top right
        output[0].x += col.size.x / 2f;
        output[0].y += col.size.y / 2f;

        //top left
        output[1].x -= col.size.x / 2f;
        output[1].y += col.size.y / 2f;

        //bottom right
        output[2].x += col.size.x / 2f;
        output[2].y -= col.size.y / 2f;

        //bottom left
        output[3].x -= col.size.x / 2f;
        output[3].y -= col.size.y / 2f;


        return output;
    }

    private void DebugRayHits(RaycastHit2D hit, Vector2 origin, Vector2 direction, float magnitude)
    {
        if (hit)
        {
            Debug.DrawRay(origin, Mathf.Sign(magnitude) * hit.distance * direction, Color.green);
        }
        else
        {
            Debug.DrawRay(origin, magnitude * direction, Color.red);
        }
    }
}
