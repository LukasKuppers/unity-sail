using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] islandLocations;
    [SerializeField]
    private float[] islandRadii;

    [SerializeField]
    private float strength;
    [SerializeField]
    private float speed;

    private float t = 0;


    public float Height(float x, float y)
    {
        return NormalizedHeight(x, y) * strength;
    }

    public float NormalizedHeight(float x, float y)
    {
        float baseHeight = Mathf.PerlinNoise((x / 100) + t, y / 200);

        int closestIsland = GetClosestIsland(x, y);
        Vector3 island = islandLocations[closestIsland].transform.position;

        float islandMultiplier = Vector2.Distance(new Vector2(x, y), new Vector2(island.x, island.z)) / islandRadii[closestIsland];
        islandMultiplier = Mathf.Clamp01(islandMultiplier);
        return baseHeight * islandMultiplier;
    }

    // approximation of wave functions normal at given coordinates
    public Vector3 Normal(float x, float y)
    {
        Vector3 p1 = new Vector3(x, Height(x, y), y);
        Vector3 p2 = new Vector3(x + 1, Height(x + 1, y), y);
        Vector3 p3 = new Vector3(x, Height(x, y + 1), y + 1);

        Plane tangent = new Plane(p1, p2, p3);
        return tangent.flipped.normal;
    }

    private void Update()
    {
        t += Time.deltaTime * speed;
    }

    private int GetClosestIsland(float x, float y)
    {
        Vector2 pos = new Vector2(x, y);
        int closest = 0;
        float leastDistance = Vector2.Distance(pos, 
            new Vector2(islandLocations[0].transform.position.x, islandLocations[0].transform.position.z));

        for (int i = 1; i < islandLocations.Length; i++)
        {
            Vector3 location = islandLocations[i].transform.position;
            float dist = Vector2.Distance(pos, new Vector2(location.x, location.z));
            if (dist < leastDistance)
            {
                leastDistance = dist;
                closest = i;
            }
        }

        return closest;
    }
}
