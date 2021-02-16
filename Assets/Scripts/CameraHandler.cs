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
    void Start()
    {
        transform.rotation = Quaternion.AngleAxis(center, Vector3.forward);
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
                    if (currentAngle + rotationSpeed > maxDeviationFromCenter) { currentAngle = maxDeviationFromCenter; }
                    else { currentAngle += rotationSpeed * Time.deltaTime; }
                }
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (currentAngle > maxDeviationFromCenter * -1)
                {
                    if (currentAngle - rotationSpeed < -1 * maxDeviationFromCenter) { currentAngle = -1 * maxDeviationFromCenter; }
                    else { currentAngle -= rotationSpeed * Time.deltaTime; }
                }
            }
            transform.rotation = Quaternion.AngleAxis(center + currentAngle, Vector3.forward);
        }
    }
}
