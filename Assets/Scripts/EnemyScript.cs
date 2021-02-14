using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    private NoiseManager noiseManager;
    private bool hearing;
    public float NoiseStrengthThreshold;
    public Vector2 homePosition;
    public float speed;
    private Vector3 currentPathFind;
    // Start is called before the first frame update
    void Start()
    {
        noiseManager = GameObject.Find("NoiseManager").gameObject.GetComponent<NoiseManager>();
        currentPathFind = new Vector3(homePosition.x, homePosition.y, 0);
    }

    // Update is called once per frame
    void Update()
    {
        List<Vector3> noises = noiseManager.CheckNoisesNearMe(new Vector2(transform.position.x, transform.position.y));
        if(noises.Count > 0)
        {
            Vector3 loudestNoise = noises[0];
            foreach(Vector3 noise in noises)
            {
                if (noise.z > loudestNoise.z) { loudestNoise = noise; }
            }
            if(loudestNoise.z > currentPathFind.z)
            {
                currentPathFind = loudestNoise;
            }
        }
        Pathfind();
    }

    void Pathfind()
    {
        if (transform.position.x != currentPathFind.x)
        {
            if (transform.position.x > currentPathFind.x) { transform.position -= new Vector3(speed, 0, 0); }
            if (transform.position.x < currentPathFind.x) { transform.position += new Vector3(speed, 0, 0); }
        }
        else if (transform.position.y != currentPathFind.y)
        {
            if (transform.position.y > currentPathFind.y) { transform.position -= new Vector3(0, speed, 0); }
            if (transform.position.y < currentPathFind.y) { transform.position += new Vector3(0, speed, 0); }
        }
        else { currentPathFind = new Vector3(homePosition.x, homePosition.y, 0);  }
    }
}
