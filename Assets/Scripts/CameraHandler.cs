using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    public bool playerIsControlling { get; set; }
    public float maxDeviationFromCenter;
    public float rotationSpeed;
    public float center;
    private float currentAngle;

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

        childTransform.rotation = Quaternion.AngleAxis(center + currentAngle, Vector3.forward);

        anim.SetFloat(currentAngleAnimatorHash, -(currentAngle - maxDeviationFromCenter) / (maxDeviationFromCenter / 5f));
        anim.SetBool(isControllingAnimatorHash, playerIsControlling);
    }
}
