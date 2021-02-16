using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SimpleRigidbody))]
public class EnemyScript : MonoBehaviour
{
    private SimpleRigidbody rb;
    private bool hearing;
    public float NoiseStrengthThreshold;
    public Vector2 homePosition;
    public float speed;
    public float maxDistToWalls;
    public List<Vector2> PreProgrammedPath;
    private Vector3 currentPathFind;
    private Vector3 currentDir;
    private Vector3 prevDir;
    private bool routine;
    private int routineStage;
    private int routineDir;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<SimpleRigidbody>();
        routineDir = 1;
        currentPathFind = new Vector3(homePosition.x, homePosition.y, 0);
        currentDir = new Vector3(0, 0, 0);
        prevDir = new Vector3(0, 0, 0);
        if(PreProgrammedPath == null)
        {
            PreProgrammedPath = new List<Vector2>();
        }
        else
        {
            routineStage = 1;
            currentPathFind = new Vector3(PreProgrammedPath[1].x, PreProgrammedPath[1].y, 0);
        }
        routine = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (((int)currentPathFind.x != (int)transform.position.x || (int)currentPathFind.y != (int)transform.position.y))
        {
            Pathfind();
        }
        else if (routine)
        {
            if (PreProgrammedPath.Count > 1)
            {
                if (routineStage == PreProgrammedPath.Count - 1) { routineDir = -1; }
                else if (routineStage == 0) { routineDir = 1; }
                routineStage += routineDir;
                currentPathFind = new Vector3(PreProgrammedPath[routineStage].x, PreProgrammedPath[routineStage].y, 0);
            }
        }
        else { currentPathFind = homePosition; routine = true; routineStage = 1; }
    }

    void Pathfind()
    {
        bool canGoRight = rb.RaycastXCollision(maxDistToWalls) == maxDistToWalls;
        bool canGoUp = rb.RaycastYCollision(maxDistToWalls) == maxDistToWalls;
        bool canGoLeft = rb.RaycastXCollision(-1 * maxDistToWalls) == maxDistToWalls * -1;
        bool canGoDown = rb.RaycastYCollision(-1 * maxDistToWalls) == maxDistToWalls * -1;
        int myPosX = (int)transform.position.x;
        int myPosY = (int)transform.position.y;
        int pathX = (int)currentPathFind.x;
        int pathY = (int)currentPathFind.y;
        if(currentDir == Vector3.zero)
        {
            if (canGoRight && myPosX < pathX) { currentDir = new Vector3(speed, 0, 0); }
            else if (canGoLeft && myPosX > pathX) { currentDir = new Vector3(speed * -1, 0, 0); }
            else if (canGoUp && (myPosY < pathY || myPosX != pathX)) { currentDir = new Vector3(0, speed, 0); }
            else if (canGoDown && (myPosY > pathY || myPosX != pathX)) { currentDir = new Vector3(0, speed * -1, 0); }
        }
        else if(currentDir == new Vector3(speed, 0, 0) && (!canGoRight || myPosX >= pathX))
        {
            if (!canGoRight) { currentDir = Vector3.zero; }
            else if ((canGoUp && myPosY < pathY) || (canGoDown && myPosY > pathY)) { currentDir = Vector3.zero; }
        }
        else if (currentDir == new Vector3(speed * -1, 0, 0) && (!canGoLeft || myPosX <= pathX))
        {
            if (!canGoLeft) { currentDir = Vector3.zero; }
            else if ((canGoUp && myPosY < pathY) || (canGoDown && myPosY > pathY)) { currentDir = Vector3.zero; }
        }
        else if (currentDir == new Vector3(0, speed, 0) && (!canGoUp || myPosY >= pathY))
        {
            if (!canGoUp) { currentDir = Vector3.zero; }
            else if ((canGoRight && myPosX < pathX) || (canGoLeft && myPosX > pathX)) { currentDir = Vector3.zero; }
        }
        else if (currentDir == new Vector3(0, speed * -1, 0) && (!canGoDown || myPosY <= pathY))
        {
            if (!canGoDown) { currentDir = Vector3.zero; }
            else if ((canGoRight && myPosX < pathX) || (canGoLeft && myPosX > pathX)) { currentDir = Vector3.zero; }
        }

        //each one: if current direction, and (can't move or shouldn't move) then if(can'tmove) and then if(can move but shouldn't and can in other dir)
        transform.position += currentDir * Time.deltaTime;
    }

    public void OnNoiseHeard(Vector3 noise)
    {
        float strength;
        bool heard = CalculateIfHeard(noise, out strength);
    }
    
    private bool CalculateIfHeard(Vector3 noise, out float strength)
    {
        float radius = noise.z;
        float dist = (new Vector2(noise.x, noise.y) - new Vector2(transform.position.x, transform.position.y)).magnitude;
        if (dist < radius)
        {
            strength = dist;
            return true;
        }
        strength = 0;
        return false;
    }

    private void performAction(Vector3 noise, float strength)
    {
        if(currentPathFind.z < strength)
        {
            currentPathFind = new Vector3(noise.x, noise.y, strength);
            routine = false;
        }
    }
}
