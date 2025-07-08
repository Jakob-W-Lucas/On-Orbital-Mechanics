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

    public double r { get; private set; }
    public double t { get; private set; }
    private double delta, h;
    private double radLOP;

    private void Start()
    {
        systemLookup = GetComponentInParent<SystemController>();
        CelestialBodyController cBC = GetComponent<CelestialBodyController>();

        body = cBC.Body;
        orbitingBody = cBC.Orbiting;
        // Convert the bodies longitude of Perihelion in radians
        radLOP = body.LongitudeOfPerihelion * Math.PI / 180.0;
        // Get the axes
        CalculateAxes();
    }

    /// <summary>
    /// 
    /// Calculates the Major and Minor axis of an orbital ellipse using Newton's version of Kepler's third law. 
    /// 
    /// Only the celestial body that is being orbited's mass is taken into account for now.
    /// 
    /// We also determine the center of the ellipse from the longitude of perihelion of the ellipse.
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
            (float)(centerMagnitude * Math.Cos(radLOP)), 
            (float)(centerMagnitude * Math.Sin(radLOP))
        );

        h = Math.Sqrt(mu * a * (1.0 - e * e));
        delta = a * (1.0 - e * e);
        // Inital random position around the ellipse
        t = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
    }

    /// <summary>
    /// 
    /// Progresses the position of the object around its ellipse, making sure to take into account
    /// Kepler's 2nd law when calculating velocity.
    /// 
    /// </summary>
    void FixedUpdate()
    {
        r = delta / (1.0 + e * Math.Cos(t));
        double omegaReal = systemLookup.ScaleTime(h / (r * r));

        t += omegaReal * Time.fixedDeltaTime * Math.Sign(body.MeanOrbitalVelocity);

        // wrap to [0,2π) occasionally:
        if (t > 2.0 * Math.PI) t -= 2.0 * Math.PI;

        // Progress the position around the ellipse
        double theta = t + radLOP;
        // Compute the Cartesian coordinates
        double x = Center.x + systemLookup.ScaleDistance(r * Math.Cos(theta));
        double y = Center.y + systemLookup.ScaleDistance(r * Math.Sin(theta));

        transform.localPosition = new Vector2((float)x, (float)y);
    }
}   
