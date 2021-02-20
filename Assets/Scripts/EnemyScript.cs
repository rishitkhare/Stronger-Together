using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SimpleRigidbody))]
public class EnemyScript : MonoBehaviour
{
    private SimpleRigidbody rb;
    private Animator anim;
    private readonly int dirXHash = Animator.StringToHash("DirectionX");
    private readonly int dirYHash = Animator.StringToHash("DirectionY");
    private readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private SpriteRenderer spren;

    public float NoiseStrengthThreshold;
    public Vector2 homePosition;
    public float speed;
    public float maxDistToWalls;
    public List<Vector2> PreProgrammedPath;
    public int waitFrames;
    public int dirWait;
    public LayerMask wallLayer;
    public LayerMask cameraLayer;
    public List<string> clothingToAlert;

    private Vector2Int gridPosition;
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
        anim = gameObject.GetComponent<Animator>();
        spren = gameObject.GetComponent<SpriteRenderer>();

        transform.position = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        gridPosition = new Vector2Int((int) transform.position.x, (int) transform.position.y);
        currentPathFind = new Vector3(homePosition.x, homePosition.y, 0);
        currentDir = new Vector3(1, 0, 0);
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
        Vector2Int currentGridPathFind = new Vector2Int(Mathf.RoundToInt(currentPathFind.x), Mathf.RoundToInt(currentPathFind.y));
        if ((gridPosition - currentGridPathFind).sqrMagnitude > 0.1f && currentWaitFrame == 0)
        {
            if(IsAtNewGridNode()) {
                Pathfind();
            }


            transform.position += currentDir * speed * Time.deltaTime;
            anim.SetBool(isMovingHash, true);
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
                }

                currentWaitFrame = waitFrames;
                currentPathFind = new Vector3(PreProgrammedPath[routineStage].x, PreProgrammedPath[routineStage].y, 0);
            }

            anim.SetBool(isMovingHash, false);
        }
        else if(currentWaitFrame == 0) { currentPathFind = homePosition; routine = true; routineStage = 1; anim.SetBool(isMovingHash, false); }

        if (currentWaitFrame > 0) { currentWaitFrame--; }

        if (CheckLineOfSight()) { transition.RestartLevel(); }
        myLineOfSight.transform.rotation = Quaternion.LookRotation(Vector3.forward, currentDir * -1);

        anim.SetFloat(dirXHash, currentDir.x);
        anim.SetFloat(dirYHash, currentDir.y);

        if (currentDir.x < 0) {
            spren.flipX = true;
        }

        else {
            spren.flipX = false;
        }
    }

    private bool IsAtNewGridNode() {
        Vector2Int roundedPos = new Vector2Int(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        //if not at current node, and close to another node
        if(roundedPos != gridPosition && (Mathf.Abs(transform.position.x - Mathf.Round(transform.position.x)) < 0.05f)
            && (Mathf.Abs(transform.position.y - Mathf.Round(transform.position.y)) < 0.05f)) {

            transform.position = new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y));
            Debug.Log(string.Format("reached new node at {0}", transform.position));

            return true;
        }

        return false;
    }


    void Pathfind()
    {
        bool canGoRight = rb.RaycastXCollision(maxDistToWalls) == maxDistToWalls;
        bool canGoUp = rb.RaycastYCollision(maxDistToWalls) == maxDistToWalls;
        bool canGoLeft = rb.RaycastXCollision(-1 * maxDistToWalls) == maxDistToWalls * -1;
        bool canGoDown = rb.RaycastYCollision(-1 * maxDistToWalls) == maxDistToWalls * -1;

        Debug.Log(string.Format("CanGoRight: {0}", canGoRight));
        Debug.Log(string.Format("CanGoUp: {0}", canGoUp));
        Debug.Log(string.Format("CanGoLeft: {0}", canGoLeft));
        Debug.Log(string.Format("CanGoDown: {0}", canGoDown));

        bool[] possibleDirectionsAllowed =
        {
            canGoUp,
            canGoDown,
            canGoLeft,
            canGoRight,
        };

        Vector2Int target = new Vector2Int(Mathf.RoundToInt(currentPathFind.x), Mathf.RoundToInt(currentPathFind.y));

        Vector2 oldGridPosition = gridPosition;

        //snaps to grid
        gridPosition.x = Mathf.RoundToInt(transform.position.x);
        gridPosition.y = Mathf.RoundToInt(transform.position.y);

        if (gridPosition == target) {
            transform.position = new Vector3(target.x, target.y, 0);
            Debug.Log("Snapped");
            return;
        }

        Vector2Int[] possiblePositions =
        {
            gridPosition + Vector2Int.up,
            gridPosition + Vector2Int.down,
            gridPosition + Vector2Int.left,
            gridPosition + Vector2Int.right,     
        };
        // unpriritizes x-axis

        // how much distance gained towards target in cardinal direction
        float[] possiblePositionsCosts = new float[4];

        Vector2Int curDir = new Vector2Int((int)currentDir.x, (int)currentDir.y);

        for (int i = 0; i < possiblePositions.Length; i++) {

            //Do not let retread old ground
            if (possiblePositions[i] == oldGridPosition) {
                possibleDirectionsAllowed[i] = false;
            }

            //calculate costs if the direction is allowed
            if(possibleDirectionsAllowed[i]) {
                possiblePositionsCosts[i] = (possiblePositions[i] - target).sqrMagnitude;
            }
        }

        float min = Mathf.Infinity;
        int index = -1;

        // calculates minimum cost of a certain cardinal direction
        for (int i = 0; i < possiblePositionsCosts.Length; i++) {
            if (possiblePositionsCosts[i] < min && possibleDirectionsAllowed[i]) {
                min = possiblePositionsCosts[i];
                index = i;
            }
        }

        //randomization on the path
        //if(UnityEngine.Random.Range(0f, 1f) < 0.5f) {

        //}

        Vector3 newDir = Vector3.zero;

        switch (index) {
            case 0:
                newDir = Vector3.up;
                break;
            case 1:
                newDir = Vector3.down;
                break;
            case 2:
                newDir = Vector3.left;
                break;
            case 3:
                newDir = Vector3.right;
                break;
        }

        if (newDir != currentDir) {
            currentWaitFrame = dirWait;
        }

        currentDir = newDir;

        /*if(currentDir == Vector3.zero)
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
        }*/
    }

    public void OnNoiseHeard(Vector3 noise, GameObject cause)
    {
        float strength;
        bool heard = CalculateIfHeard(noise, out strength);
        if(heard && ! checkIfInJailCell(new Vector2(noise.x, noise.y)))
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

    private bool checkIfInJailCell(Vector2 position) {
        Vector2 directionVector = position - new Vector2(transform.position.x, transform.position.y);
        RaycastHit2D hitwall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), directionVector.normalized, directionVector.magnitude, wallLayer);
        RaycastHit2D camerawall = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y), directionVector.normalized, directionVector.magnitude, cameraLayer);

        return hitwall && ! camerawall;
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
