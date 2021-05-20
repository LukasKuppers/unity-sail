using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveTextureGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject WaveManager;
    [SerializeField]
    private string displacementReferenceName;
    [SerializeField]
    private float meshWidth;
    [SerializeField]
    private int meshSize;

    private WaveGenerator waveFunc;
    private MeshRenderer meshRenderer;

    private void Start()
    {
        waveFunc = WaveManager.GetComponent<WaveGenerator>();
        meshRenderer = gameObject.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        ApplyTexture();
    }

    private Texture2D GenerateTexture(int size, float width)
    {
        Texture2D waveTex = new Texture2D(size + 1, size + 1);

        waveTex.filterMode = FilterMode.Bilinear;
        waveTex.wrapMode = TextureWrapMode.Clamp;

        float cellWidth = width / size;

        for (int i = 0; i <= size; i++)
        {
            for (int j = 0; j <= size; j++)
            {
                float xPos = (transform.position.x + (width / 2) - (i * cellWidth));
                float yPos = (transform.position.z + (width / 2) - (j * cellWidth));

                float height = waveFunc.NormalizedHeight(xPos, yPos);
                waveTex.SetPixel(i, j, new Color(height, height, height));
            }
        }

        waveTex.Apply();
        return waveTex;
    }

    private void ApplyTexture()
    {
        Texture2D waveTex = GenerateTexture(meshSize, meshWidth);
        meshRenderer.material.SetTexture(displacementReferenceName, waveTex);
    }
}
