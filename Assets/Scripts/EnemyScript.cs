using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SimpleRigidbody))]
public class EnemyScript : MonoBehaviour
{
    private SimpleRigidbody rb;
    public float NoiseStrengthThreshold;
    public Vector2 homePosition;
    public float speed;
    public float maxDistToWalls;
    public List<Vector2> PreProgrammedPath;
    public int waitFrames;
    public LayerMask wallLayer;
    public List<string> clothingToAlert;

    private Vector3 currentPathFind;
    private Vector3 currentDir;
    private bool routine;
    private int routineStage;
    private int currentWaitFrame;
    private PolygonCollider2D myLineOfSight;
    private BoxCollider2D Player1Col;
    private BoxCollider2D Player2Col;
    private GameObject Player1;
    private GameObject Player2;
    private SceneTransition transition;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<SimpleRigidbody>();
        currentPathFind = new Vector3(homePosition.x, homePosition.y, 0);
        currentDir = new Vector3(0, 0, 0);
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
        currentWaitFrame = 0;
        myLineOfSight = GetComponentInChildren<PolygonCollider2D>();
        Player1Col = GameObject.FindGameObjectWithTag("Player1").transform.Find("CameraCollider").GetComponent<BoxCollider2D>();
        Player2Col = GameObject.FindGameObjectWithTag("Player2").transform.Find("CameraCollider").GetComponent<BoxCollider2D>();
        Player1 = GameObject.FindGameObjectWithTag("Player1");
        Player2 = GameObject.FindGameObjectWithTag("Player2");
        transition = GameObject.Find("SceneTransitioner").GetComponent<SceneTransition>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((((int)currentPathFind.x != (int)transform.position.x || (int)currentPathFind.y != (int)transform.position.y)) && currentWaitFrame == 0)
        {
            Pathfind();
        }
        else if (routine && currentWaitFrame == 0)
        {
            if (PreProgrammedPath.Count > 1)
            {
                if(routineStage == PreProgrammedPath.Count - 1)
                {
                    routineStage = 0;
                }
                else
                {
                    routineStage++;
                    currentWaitFrame = waitFrames;
                }
                currentPathFind = new Vector3(PreProgrammedPath[routineStage].x, PreProgrammedPath[routineStage].y, 0);
            }
        }
        else if(currentWaitFrame == 0) { currentPathFind = homePosition; routine = true; routineStage = 1; }

        if (currentWaitFrame > 0) { currentWaitFrame--; }

        if (CheckLineOfSight()) { transition.RestartLevel(); }
        myLineOfSight.transform.rotation = Quaternion.AngleAxis(0, currentDir);
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
            if (canGoRight && (myPosX < pathX || ((myPosY < pathY && !canGoUp) && (myPosY > pathY && !canGoDown)))) 
                { currentDir = new Vector3(speed, 0, 0); }
            else if (canGoLeft && (myPosX > pathX || ((myPosY < pathY && !canGoUp) && (myPosY > pathY && !canGoDown)))) 
                { currentDir = new Vector3(speed * -1, 0, 0); }
            else if (canGoUp && (myPosY < pathY || myPosX != pathX)) { currentDir = new Vector3(0, speed, 0); }
            else if (canGoDown && (myPosY > pathY || myPosX != pathX)) { currentDir = new Vector3(0, speed * -1, 0); }
        }
        else if(currentDir == new Vector3(speed, 0, 0) && (!canGoRight || myPosX >= pathX))
        {
            if (!canGoRight) { currentDir = Vector3.zero; }
            else if ((canGoUp && myPosY <= pathY) || (canGoDown && myPosY >= pathY)) { currentDir = Vector3.zero; }
        }
        else if (currentDir == new Vector3(speed * -1, 0, 0) && (!canGoLeft || myPosX <= pathX))
        {
            if (!canGoLeft) { currentDir = Vector3.zero; }
            else if ((canGoUp && myPosY <= pathY) || (canGoDown && myPosY >= pathY)) { currentDir = Vector3.zero; }
        }
        else if (currentDir == new Vector3(0, speed, 0) && (!canGoUp || myPosY >= pathY))
        {
            if (!canGoUp) { currentDir = Vector3.zero; }
            else if ((canGoRight && myPosX <= pathX) || (canGoLeft && myPosX >= pathX)) { currentDir = Vector3.zero; }
        }
        else if (currentDir == new Vector3(0, speed * -1, 0) && (!canGoDown || myPosY <= pathY))
        {
            if (!canGoDown) { currentDir = Vector3.zero; }
            else if ((canGoRight && myPosX <= pathX) || (canGoLeft && myPosX >= pathX)) { currentDir = Vector3.zero; }
        }
        transform.position += currentDir * Time.deltaTime;
    }

    public void OnNoiseHeard(Vector3 noise, GameObject cause)
    {
        float strength;
        bool heard = CalculateIfHeard(noise, out strength);
        Debug.Log(heard + "," + strength);
        if(heard)
        {
            performAction(noise, strength, cause);
        }
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

    private void performAction(Vector3 noise, float strength, GameObject source)
    {
        if(currentPathFind.z <= strength && source.GetComponent<PlayerScript>().wearing() == "spy")
        {
            currentPathFind = new Vector3(noise.x, noise.y, strength);
            routine = false;
        }
    }

    private bool CheckLineOfSight()
    {
        Vector2[] Player1ColliderVertices = GetVerticesOfBoxCollider(Player1Col);

        foreach (Vector2 point in Player1ColliderVertices)
        {

            if (myLineOfSight.OverlapPoint(point) && VisionToPointNotObstructed(point))
            {
                if (clothingToAlert.Contains(Player1.gameObject.GetComponent<PlayerScript>().wearing()))
                {
                    return true;
                }
            }
        }

        Vector2[] Player2ColliderVertices = GetVerticesOfBoxCollider(Player2Col);

        foreach (Vector2 point in Player2ColliderVertices)
        {
            if (myLineOfSight.OverlapPoint(point) && VisionToPointNotObstructed(point))
            {
                if (clothingToAlert.Contains(Player2.gameObject.GetComponent<PlayerScript>().wearing()))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private bool VisionToPointNotObstructed(Vector2 point)
    {
        Vector2 directionToPlayer = point - (Vector2)(transform.position);
        float distanceToPlayer = directionToPlayer.magnitude;
        directionToPlayer.Normalize();

        RaycastHit2D rayHitsWall = Physics2D.Raycast(transform.position, directionToPlayer.normalized, distanceToPlayer, wallLayer);

        return !rayHitsWall;
    }

    private Vector2[] GetVerticesOfBoxCollider(BoxCollider2D col)
    {
        Vector2[] output = new Vector2[4];

        //put them all at the center, then move them to the corners using size.
        for (int i = 0; i < output.Length; i++)
        {
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
}
