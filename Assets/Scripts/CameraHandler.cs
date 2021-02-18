using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class CameraHandler : MonoBehaviour
{
    public bool playerIsControlling { get; set; }
    public float maxDeviationFromCenter;
    public float rotationSpeed;
    public float center;
    public float FOVAngle;
    public float innerRadius;
    public LayerMask wallLayer;
    public LayerMask playerLayer;
    private float currentAngle;
    public UnityEvent<GameObject> onDetected;

    private Transform childTransform;

    private readonly int currentAngleAnimatorHash = Animator.StringToHash("Angle");
    private readonly int isControllingAnimatorHash = Animator.StringToHash("IsControlling");

    private Animator anim;
    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        childTransform = transform.GetChild(0);
        childTransform.rotation = Quaternion.AngleAxis(center, Vector3.forward);
        currentAngle = 0;
        if (onDetected == null) { onDetected = new UnityEvent<GameObject>(); }
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
        if (checkIfPlayerCollides()) { }
        childTransform.rotation = Quaternion.AngleAxis(center + currentAngle, Vector3.forward);

        anim.SetFloat(currentAngleAnimatorHash, center + currentAngle);
        anim.SetBool(isControllingAnimatorHash, playerIsControlling);
    }

    public bool checkIfPlayerCollides()
    {
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        float nonRelativeAngle = center + currentAngle + 90;
        for(float i = nonRelativeAngle - FOVAngle/2; i <= nonRelativeAngle + FOVAngle/2; i++)
        {
            Vector2 dir = new Vector2(Mathf.Cos(i * (Mathf.PI/180)), Mathf.Sin(i * (Mathf.PI/180)));
            RaycastHit2D hitPlayer = Physics2D.Raycast(myPos, dir, innerRadius, playerLayer);
            RaycastHit2D hitWall = Physics2D.Raycast(myPos, dir, innerRadius, wallLayer);
            DebugRayHits(hitPlayer, myPos, dir, innerRadius);
            if(hitPlayer && !hitWall)
            {
                return true;
            }
        }
        return false;
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
