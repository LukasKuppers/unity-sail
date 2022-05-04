using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOutline : MonoBehaviour
{
    [SerializeField]
    private GameObject model;
    [SerializeField]
    private Material outlineMaterial;
    [SerializeField]
    private float outlineScale = 1.1f;
    [SerializeField]
    private Color outlineColor = Color.white;

    private Renderer outlineRenderer;

    private void Start()
    {
        outlineRenderer = CreateOutline(outlineMaterial, outlineScale, outlineColor);
    }

    private Renderer CreateOutline(Material outlineMat, float scaleFactor, Color outlineCol)
    {
        GameObject outlineObj;
        if (model != null)
        {
            outlineObj = Instantiate(model, model.transform.position, model.transform.rotation, model.transform);
        }
        else
        {
            outlineObj = Instantiate(this.gameObject, transform.position, transform.rotation, transform);
        }
        Renderer rend = outlineObj.GetComponent<Renderer>();

        rend.material = outlineMat;
        rend.material.SetColor("_OutlineColor", outlineCol);
        rend.material.SetFloat("_ScaleFactor", scaleFactor);

        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        ObjectOutline childOutline = outlineObj.GetComponent<ObjectOutline>();
        Collider childCol = outlineObj.GetComponent<Collider>();
        if (childOutline != null)
        {
            childOutline.enabled = false;
        }
        if (childCol != null)
        {
            childCol.enabled = false;
        }

        outlineObj.transform.localScale = new Vector3(1, 1, 1);
        rend.enabled = false;

        return rend;
    }

    private void OnMouseEnter()
    {
        outlineRenderer.enabled = true;
    }

    private void OnMouseExit()
    {
        outlineRenderer.enabled = false;
    }
}
