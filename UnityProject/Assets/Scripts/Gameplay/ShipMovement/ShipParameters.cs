using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipParameters
{
    private float keelInfluence;

    public ShipParameters(float keelInfluence)
    {
        this.keelInfluence = keelInfluence;
    }

    public float GetSailMultiplier(Vector3 shipForward, float sailAngle, float windDirection)
    {
        Vector2 shipVector = new Vector2(shipForward.x, shipForward.z);
        Vector2 windVector = Vector2Util.DegreeToVector2(windDirection);
        float localWindAngle = Vector2.SignedAngle(shipVector, windVector) * -1f;

        float sailOffset = Mathf.Abs(sailAngle - localWindAngle);
        
        float sailMultiplier = sailOffset < 90f ? 1 - (sailOffset / 90f) : 0f;
        return sailMultiplier;
    }

    public float GetKeelMultiplier(Vector3 shipForward, float windDirection)
    {
        Vector2 shipVector = new Vector2(shipForward.x, shipForward.z);
        Vector2 windVector = Vector2Util.DegreeToVector2(windDirection);
        float keelOffset = Vector2.Angle(shipVector, windVector);
        keelOffset = keelOffset > 90f ? 180 - keelOffset : keelOffset;

        float keelMultiplier = ((keelOffset / 90f) * keelInfluence) + (1f - keelInfluence);
        return keelMultiplier;
    }
}
