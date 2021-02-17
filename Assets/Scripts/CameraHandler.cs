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
    private float currentAngle;
    public List<GameObject> blackList;
    public UnityEvent<GameObject> onDetected;
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
        foreach (GameObject obj in blackList)
        {
            if (checkIfPlayerCollides(obj)) { onDetected.Invoke(obj); }
        }
    }

    public bool checkIfPlayerCollides(GameObject obj)
    {
        bool detected = false;
        Vector2 PlayerPos = new Vector2(obj.transform.position.x, obj.transform.position.y);
        Vector2 myPos = new Vector2(transform.position.x, transform.position.y);
        float nonRelativeAngle = center + currentAngle;
        Vector2 cameraVec = new Vector2(Mathf.Cos(nonRelativeAngle), Mathf.Sin(nonRelativeAngle));
        Vector2 playerVec = new Vector2(PlayerPos.x - myPos.x, PlayerPos.y - myPos.y);
        float angleBetween = Mathf.Acos(((cameraVec.x * playerVec.x) + (cameraVec.y * playerVec.y)) / (cameraVec.magnitude * playerVec.magnitude));
        RaycastHit2D hit = Physics2D.Raycast(myPos, playerVec, playerVec.magnitude - 1.5f, wallLayer);
        if(angleBetween < FOVAngle/2 && !hit && playerVec.magnitude < innerRadius)
        {
            detected = true;
        }
        return detected;
    }
}
