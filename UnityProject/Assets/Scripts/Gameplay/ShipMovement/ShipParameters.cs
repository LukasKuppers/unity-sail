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

    public float GetSailMultiplier(float shipRotation, float sailAngle, float windDirection)
    {
        float sailRotation = (shipRotation + sailAngle) % 360f;

        float sailOffset = AngleDifference(sailRotation, windDirection);
        float sailMultiplier = sailOffset < 90f ? 1 - (sailOffset / 90f) : 0f;
        return sailMultiplier;
    }

    public float GetKeelMultiplier(float shipRotation, float windDirection)
    {
        float keelOffset = AngleDifference(shipRotation, windDirection);
        keelOffset = keelOffset > 90f ? 180 - keelOffset : keelOffset;

        return ((keelOffset / 90f) * keelInfluence) + (1f - keelInfluence);
    }

    private float AngleDifference(float a, float b)
    {
        float min = Mathf.Min(a, b);
        float max = a + b - min;

        return Mathf.Min(max - min, min + (360 - max));
    }
}
