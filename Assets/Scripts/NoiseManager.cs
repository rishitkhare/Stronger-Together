using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float NoiseToRadiusRatio;
    private List<Vector3> noises;
    void Start()
    {
        noises = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        noises.Clear();
    }

    public void AddNoise(Vector2 position, float strength)
    {
        noises.Add(new Vector3(position.x, position.y, strength));
    }

    public List<Vector3> CheckNoisesNearMe(Vector2 myPos)
    {
        List<Vector3> noisesHeard = new List<Vector3>();
        foreach (Vector3 noise in noises)
        {
            float radius = noise.z * NoiseToRadiusRatio;
            float distanceToNoise = (myPos - new Vector2(noise.x, noise.y)).magnitude;
            if (distanceToNoise < radius)
            {
                noisesHeard.Add(new Vector3(noise.x, noise.y, (distanceToNoise / radius) * noise.z));
            }
        }

        return noisesHeard;
    }
}