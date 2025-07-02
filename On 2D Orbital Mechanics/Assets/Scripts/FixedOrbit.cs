using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedOrbit : MonoBehaviour
{
    [SerializeField] private GalacticScale GS;
    [SerializeField] private float e;
    [SerializeField] private Vector2 _center;
    [SerializeField] private CelestialBody body;
    [SerializeField] private CelestialBody orbitingBody;

    // Semi-Major and Semi-Minor orbital axes respectfully
    private float a, b;
    // Adjusted angular velocity;
    private float w;
    private float t;

    private void Start()
    {
        // Retrieve either the Solar mass or the orbiting body mass
        orbitingBody = orbitingBody ? orbitingBody : GS.HostBody;

        if (!GS)
        {
            Debug.LogWarning($"{this.name} has not been assigned a Galactic Scale");
            return;
        }

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

        double mu = AstronomicalConstants.G * ((orbitingBody.Mass * Math.Pow(10, 24)) + (body.Mass * Math.Pow(10, 24)));
        double a = Math.Pow(Math.Pow(p, 2) * mu
            / (4 * Math.Pow(Math.PI, 2)), 1f / 3f);

        double c = e * a;
        double b = Math.Sqrt(Math.Pow((float)a, 2) - Math.Pow((float)c, 2));

        w = (float)Math.Sqrt(mu / Math.Pow(a, 3));

        // Converts the meter calculation into km and then scales it to Galactic scale
        return (GS.ScaleDistance((float)a) / 1000, GS.ScaleDistance((float)b) / 1000);
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (!GS) return;

        Orbit();
    }

    void Orbit()
    {
        // Get the fixed time update
        t += GS.ScaleTime(w * Time.fixedDeltaTime);

        // Calculate the position around the circular orbit
        float x = _center.x + a * Mathf.Cos(t);
        float y = _center.y + b * Mathf.Sin(t);

        transform.localPosition= new Vector2(x, y);
    }
}   
