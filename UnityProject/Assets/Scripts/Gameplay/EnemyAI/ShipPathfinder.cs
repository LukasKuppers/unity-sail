using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipPathfinder : MonoBehaviour
{
    [SerializeField]
    private int maxNumSearchRays = 3;
    [SerializeField]
    private float pathfindingRayLength = 50f;
    [SerializeField]
    [Range(0f, 1f)]
    private float pathfindingRouteSensitivity = 0.5f;
    [SerializeField]
    [Range(0f, 1f)]
    private float pathfindingForwardBias = 0.5f;
    [SerializeField]
    private int pathfindingRayObstacleLayer;

    private IAutomaticShip ship;
    private int searchLayerMask;

    private void Start()
    {
        ship = gameObject.GetComponent<IAutomaticShip>();

        searchLayerMask = 1 << pathfindingRayObstacleLayer;
    }

    public void TravelToPoint(Vector3 target)
    {
        Vector3 atTarget = Vector3.Normalize(new Vector3(
            target.x - transform.position.x, 0, target.z - transform.position.z));
        Vector3 forward = Vector3.Normalize(new Vector3(transform.forward.x, 0, transform.forward.z));
        atTarget = Vector3.Normalize(atTarget + (forward * pathfindingForwardBias));

        int numRays = 1;
        float totalCumDist = (1f - pathfindingRouteSensitivity) * pathfindingRayLength;
        float rayDist = CastSearchRay(atTarget);
        float angleOffset = 0f;
        bool preferRight = true;

        if (RayIsValidPath(rayDist, totalCumDist / numRays))
        {
            ship.SetTarget(transform.position + (atTarget * pathfindingRayLength));
            return;
        }

        // cast cascading search rays to left and right
        for (int i = 0; i < maxNumSearchRays; i++)
        {
            angleOffset += 180f / (maxNumSearchRays + 1);

            rayDist = CastSearchRayRight(angleOffset, atTarget);
            if (RayIsValidPath(rayDist, totalCumDist / numRays))
            {
                preferRight = true;
                break;
            }
            numRays++;
            totalCumDist += rayDist;
            rayDist = CastSearchRayLeft(angleOffset, atTarget);
            if (RayIsValidPath(rayDist, totalCumDist / numRays))
            {
                preferRight = false;
                break;
            }
            numRays++;
            totalCumDist += rayDist;
        }

        Vector3 targetDir = GetDirection(angleOffset, atTarget, preferRight);
        ship.SetTarget(transform.position + (targetDir * pathfindingRayLength));
    }

    private bool RayIsValidPath(float rayLength, float averageLength)
    {
        return (rayLength == Mathf.Infinity ||
                rayLength * pathfindingRouteSensitivity > averageLength);
    }

    private float CastSearchRayRight(float angleOffset, Vector3 forward)
    {
        Vector3 localDir = GetDirection(angleOffset, forward, true);
        return CastSearchRay(localDir);
    }

    private float CastSearchRayLeft(float angleOffset, Vector3 forward)
    {
        Vector3 localDir = GetDirection(angleOffset, forward, false);
        return CastSearchRay(localDir);
    }

    private float CastSearchRay(Vector3 direction)
    {
        Vector3 origin = new Vector3(transform.position.x, 0, transform.position.z);
        if (Physics.Raycast(origin, direction,
            out RaycastHit hit, pathfindingRayLength, searchLayerMask))
        {
            return hit.distance;
        }
        else
            return Mathf.Infinity;
    }

    private Vector3 GetDirection(float angleOffset, Vector3 forward, bool toRight)
    {
        float angleRad = Mathf.Deg2Rad * angleOffset;
        Vector3 right = Vector3.Cross(forward, Vector3.up);
        if (toRight)
            return Vector3.RotateTowards(forward, right, angleRad, Mathf.Infinity);

        return Vector3.RotateTowards(forward, -right, angleRad, Mathf.Infinity);
    }
}
