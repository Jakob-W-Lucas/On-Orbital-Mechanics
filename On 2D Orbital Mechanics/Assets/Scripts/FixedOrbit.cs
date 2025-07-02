using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CelestialBody))]
public class FixedOrbit : MonoBehaviour
{
    [SerializeField] private float e;
    [SerializeField] private Vector2 _center;
    private CelestialBody body;

    // Semi-Major and Semi-Minor orbital axes respectfully
    private float a, b;
    // Adjusted angular velocity;
    private float w;
    private float t;

    private void Start()
    {
        body = GetComponent<CelestialBody>();
        w = body.MeanOrbitalVelocity / 10;

        (a, b) = CalculateAxes();
    }

    /// <summary>
    /// Calculates the Major and Minor axis of an orbital ellipse using Newton's version of Kepler's third law. 
    /// 
    /// Each axes is scaled to Galactic scale so the distances are not extreme.
    /// 
    /// Note also that in the calculation the mass of the orbiting planet is taken into account,
    /// this is not strictly necessary as (esspecially for terrestrial planets) m << M_sun. But
    /// I thought I would include it anyways because this calculation is only performed once,
    /// and is now generalized to any fixed orbit of any rotating body around the Sun.
    /// 
    /// </summary>
    (float a, float b) CalculateAxes()
    {
        // Get the orbital period in seconds
        float p = body.SiderealOrbitalPeriod * 24 * 60 * 60;

        double a = Math.Pow(Math.Pow(p, 2) * AstronomicalConstants.G * (AstronomicalConstants.SolarMass + (body.Mass * Math.Pow(10, 24))) / (4 * Math.Pow(Math.PI, 2)), 1f / 3f);
        double c = e * a;
        double b = Math.Sqrt(Math.Pow((float)a, 2) - Math.Pow((float)c, 2));

        // Converts the meter calculation into km and then scales it to Galactic scale
        return ((float)a / (AstronomicalConstants.GalacticScale * 1000), (float)b / (AstronomicalConstants.GalacticScale * 1000));
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        Orbit();
    }

    void Orbit()
    {
        // Get the fixed time update
        t += Time.fixedDeltaTime * w;

        // Calculate the position around the circular orbit
        float x = _center.x + a * Mathf.Cos(t);
        float y = _center.y + b * Mathf.Sin(t);

        transform.position = new Vector2(x, y);
    }
}   
