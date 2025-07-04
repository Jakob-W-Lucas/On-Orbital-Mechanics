using System;
using UnityEngine;

[RequireComponent(typeof(CelestialBodyController))]
public class FixedOrbit : MonoBehaviour
{
    [SerializeField] private float e;
    public Vector2 Center { get; private set; }
    private SystemController systemLookup;
    private CelestialBody body;
    private CelestialBody orbitingBody;

    public double radius;
    public double t { get; private set; }
    private double delta, h;

    private void Start()
    {
        systemLookup = GetComponentInParent<SystemController>();
        CelestialBodyController cBC = GetComponent<CelestialBodyController>();

        body = cBC.Body;
        orbitingBody = cBC.Orbiting;

        CalculateAxes();
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
    void CalculateAxes()
    {
        // Get the orbital period in seconds
        float p = body.SiderealOrbitalPeriod * 86400;
        // Assumes that m << M
        double mu = AstronomicalConstants.G * (orbitingBody.Mass * Math.Pow(10, 24));
        // Semi-Major axis
        double a = Math.Pow(p * p * mu / (4.0 * Math.PI * Math.PI), 1.0 / 3.0);
        // Distance from centre to focus
        double c = e * a;
        // Semi-Minor axis
        double b = Math.Sqrt(a * a - c * c);

        double centerMagnitude = Math.Abs(systemLookup.ScaleDistance(c / 1000.0));
        Center = new Vector2(
            (float)(centerMagnitude * Math.Cos(body.LongitudeOfPerihelion * Math.PI / 180.0)), 
            (float)(centerMagnitude * Math.Sin(body.LongitudeOfPerihelion * Math.PI / 180.0))
        );

        h = Math.Sqrt(mu * a * (1.0 - e * e));

        delta = a * (1.0 - e * e);

        t = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
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
        double r = delta / (1.0 + e * Math.Cos(t));
        radius = r;
        double omegaReal = systemLookup.ScaleTime(h / (r * r));

        t += omegaReal * Time.fixedDeltaTime * Math.Sign(body.MeanOrbitalVelocity);

        // wrap to [0,2π) occasionally:
        if (t > 2.0 * Math.PI) t -= 2.0 * Math.PI;

        // 6) Compute new position in Cartesian
        double x = Center.x + systemLookup.ScaleDistance(r * Math.Cos(t + body.LongitudeOfPerihelion * 180.0 / Math.PI));
        double y = Center.y + systemLookup.ScaleDistance(r * Math.Sin(t + body.LongitudeOfPerihelion * 180.0 / Math.PI));

        transform.localPosition = new Vector2((float)x, (float)y);
    }
}   
