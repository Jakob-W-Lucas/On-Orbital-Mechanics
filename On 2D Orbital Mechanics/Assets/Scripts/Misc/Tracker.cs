using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[RequireComponent(typeof(CelestialBodyController))]
public class Tracker : MonoBehaviour
{
    private SystemController systemController;
    private CelestialBody body;
    private bool orbitComplete = false;
    private float timer = 0f;
    private Vector2 _start;
    private float aphelion;
    private float perihelion = Mathf.Infinity;

    private void Start()
    {
        systemController = GetComponentInParent<SystemController>();
        body = GetComponent<CelestialBodyController>().Body;
    }

    private void Update()
    {
        if (orbitComplete) return;

        timer += Time.deltaTime;

        // Ensures that the orbit is on track before declaring a start point
        if (timer < 1f)
        {
            if (timer < 0.25f) _start = transform.position;
            return;
        }

        if (Mathf.Abs(transform.position.x - _start.x) < 10 &&
                Mathf.Abs(transform.position.y - _start.y) < 10)
            {
                orbitComplete = true;

                ConvertValues();
                PrintLogs();
            }

        float distFromOrbiting = Vector2.Distance(Vector2.zero, transform.position);
        perihelion = distFromOrbiting < perihelion ? distFromOrbiting : perihelion;
        aphelion = distFromOrbiting > aphelion ? distFromOrbiting : aphelion;
    }

    void ConvertValues()
    {
        perihelion = systemController.IScaleDistance(perihelion);
        aphelion = systemController.IScaleDistance(aphelion);
        timer = systemController.ScaleTime(timer);
    }

    void PrintLogs()
    {
        StringBuilder str = new StringBuilder($"Here are the stats for {body.Name} after 1 orbit:");
        str.Append($"   Time to complete: {timer / 86400} days. Error: {Mathf.Round(body.SiderealOrbitalPeriod / (timer / 86400 * 10000)) / 100}%");
        str.Append($"   Perihelion: {perihelion / Mathf.Pow(10, 6)}");
        str.Append($"   Aphelion: {aphelion / Mathf.Pow(10, 6)}");

        Debug.Log(str);
    }
}
