using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePathRenderer : MonoBehaviour
{
    [SerializeField]
    private float duration = 1f;
    [SerializeField]
    private int numPoints = 10;

    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;
        lineRenderer.receiveShadows = false;
        lineRenderer.enabled = false;
    }

    public void RenderPathForFrame(Vector3 initialPosition, Vector3 initialVelocity, float gravity)
    {
        Vector3[] positions = new Vector3[numPoints];

        Vector3 horizontalVelocity = Vector3.Scale(initialVelocity, new Vector3(1, 0, 1));
        Vector3 verticalVelocity = new Vector3(0, initialVelocity.y, 0);

        for (int i = 0; i < numPoints; i++)
        {
            float t = ((float)i / (float)numPoints) * duration;

            Vector3 xDisplacement = horizontalVelocity * t;
            Vector3 yDisplacement = (verticalVelocity * t) - (Vector3.up * 0.5f * gravity * t * t);
            positions[i] = initialPosition + xDisplacement + yDisplacement;
        }

        StartCoroutine(SingleFrameRender(positions));
    }

    private IEnumerator SingleFrameRender(Vector3[] positions)
    {
        lineRenderer.enabled = true;
        lineRenderer.SetPositions(positions);

        yield return new WaitForEndOfFrame();

        lineRenderer.enabled = false;
    }
}
