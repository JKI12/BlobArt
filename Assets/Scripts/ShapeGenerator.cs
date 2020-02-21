using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings settings;
    NoiseFilter noiseFilter;

    public ShapeGenerator(ShapeSettings settings)
    {
        this.settings = settings;

        this.noiseFilter = new NoiseFilter(settings.noiseSettings);
    }

    public Vector3 CalculatePointOnSphere(Vector3 pointOnUnitSphere)
    {
        var evaluation = noiseFilter.Evaluate(pointOnUnitSphere);
        return pointOnUnitSphere * settings.sphereRadius * (1 + evaluation);
    }
}
