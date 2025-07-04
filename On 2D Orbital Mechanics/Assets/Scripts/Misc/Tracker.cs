using System;
using System.Text;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(CelestialBodyController))]
public class Tracker : MonoBehaviour
{
    private SystemController systemController;
    private CelestialBody body;
    private FixedOrbit fixedOrbit;
    private bool orbitComplete = false;
    private float timer = 0f;
    private bool _half;
    private double _startRadian;
    private double _endRadian;
    private float aphelion;
    private float perihelion = Mathf.Infinity;

    private void Start()
    {
        systemController = GetComponentInParent<SystemController>();
        body = GetComponent<CelestialBodyController>().Body;
        fixedOrbit = GetComponent<FixedOrbit>();

        _startRadian = fixedOrbit.t;
        _endRadian = (fixedOrbit.t + Math.PI) % (2 * Math.PI);
    }

    private void Update()
    {
        if (orbitComplete) return;

        timer += Time.deltaTime;

        if (Math.Abs(Math.PI - fixedOrbit.t) < 0.01f)
        {
            double x = fixedOrbit.Center.x + systemController.ScaleDistance(fixedOrbit.radius * Math.Cos(Math.PI + body.LongitudeOfPerihelion * 180.0 / Math.PI));
            double y = fixedOrbit.Center.y + systemController.ScaleDistance(fixedOrbit.radius * Math.Sin(Math.PI + body.LongitudeOfPerihelion * 180.0 / Math.PI));

            aphelion = (float)Math.Sqrt(x * x + y * y);
        }
        
        if (fixedOrbit.t < 0.01f)
        {
            perihelion = (float)fixedOrbit.radius / 1000;
        }

        if (_half && Math.Abs(_startRadian - fixedOrbit.t) < 0.01f)
        {
            orbitComplete = true;

            PrintLogs();

            return;
        }

        if (Math.Abs(_endRadian - fixedOrbit.t) < 0.01f)
        {
            _half = true;
        }

        float distFromOrbiting = Vector2.Distance(Vector2.zero, transform.position);
        //Debug.Log($"Difference between calculated and given: {Math.Abs(systemController.IScaleDistance(distFromOrbiting / 1000) - systemController.ScaleDistance(fixedOrbit.radius / 1000))}");
        // perihelion = distFromOrbiting < perihelion ? systemController.IScaleDistance(distFromOrbiting) / 1000 : perihelion;
        // aphelion = distFromOrbiting > aphelion ? systemController.IScaleDistance(distFromOrbiting) / 1000 : aphelion;
    }

    void PrintLogs()
    {
        StringBuilder str = new StringBuilder($"Here are the stats for {body.Name} after 1 orbit:");
        str.Append($"   Time to complete: {(float)systemController.ScaleTime(timer) / 86400} days. Error: {Mathf.Round(body.SiderealOrbitalPeriod / ((float)systemController.ScaleTime(timer) / 86400 * 10000)) / 100}%");
        str.Append($"   Perihelion: {perihelion / Mathf.Pow(10, 6)}");
        str.Append($"   Aphelion: {(float)(systemController.IScaleDistance(aphelion / 1000) / Mathf.Pow(10, 6))}");

        Debug.Log(str);
    }
}
