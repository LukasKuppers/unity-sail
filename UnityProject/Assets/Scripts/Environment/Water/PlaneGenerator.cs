using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneGenerator : MonoBehaviour
{
    [SerializeField]
    private int size = 10;
    [SerializeField]
    private float width = 10f;

    private void Start()
    {
        CreatePlane();
    }

    private void CreatePlane()
    {
        Mesh mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "procedural plane";
        float cellWidth = width / size;

        List<Vector3> verticies = new List<Vector3>();
        List<Vector2> uv = new List<Vector2>();
        for (int i = 0; i < size + 1; i++)
        {
            for (int j = 0; j < size + 1; j++)
            {
                float zPos = transform.position.x - (width / 2) + (cellWidth * i);
                float xPos = transform.position.z - (width / 2) + (cellWidth * j);
                Vector3 vertex = new Vector3(xPos, 0, zPos);
                verticies.Add(vertex);
                uv.Add(new Vector2((float)j / size, (float)i / size));
            }
        }
        mesh.vertices = verticies.ToArray();
        mesh.uv = uv.ToArray();

        List<int> triangles = new List<int>();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                int anchor = i + ((size + 1) * j);
                triangles.Add(anchor);
                triangles.Add(anchor + size + 1);
                triangles.Add(anchor + 1);

                triangles.Add(anchor + 1);
                triangles.Add(anchor + size + 1);
                triangles.Add(anchor + size + 2);
            }
        }
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }
}
