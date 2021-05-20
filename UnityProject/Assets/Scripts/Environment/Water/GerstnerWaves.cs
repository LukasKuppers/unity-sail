using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GerstnerWaves
{
    private readonly float amplitude;
    private readonly float frequency;

    public GerstnerWaves(float amplitude, float frequency)
    {
        this.amplitude = amplitude;
        this.frequency = frequency;
    }

    public float GetHeight(float x)
    {
        return GetNormalHeight(x) * amplitude;
    }

    public float GetNormalHeight(float x)
    {
        float scaledX = frequency * x;
        float originalHeight = Mathf.Sin(scaledX - 0.5f * Mathf.Cos(scaledX)) + 1f;
        return originalHeight / 2f;
    }
}
