using System;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public struct CelestialBodyStore
{
    public Transform T;
    public CelestialBody Body;

    public CelestialBodyStore(Transform t, CelestialBody body)
    {
        T = t;
        Body = body;
    }
}

[System.Serializable]
public struct SystemSwitch
{
    public KeyCode Key;
    public int DistancePerUnit;
    public int Exaggeration;
}

public class SystemController : MonoBehaviour
{
    // The 'Main' body of the system
    [SerializeField] private CelestialBody _host;
    private int _distancePerUnit;
    private double _accScale;
    [SerializeField] private float _timePerUnit;
    [SerializeField] private SystemSwitch[] systemSwitches;
    private CelestialBodyTransformer celestialBodyTransformer;
    public List<CelestialBodyStore> SystemBodies { get; private set; } = new List<CelestialBodyStore>();
    public CelestialBody HostBody => _host;
    public double ScaleDistance(double d) => d / _distancePerUnit / 1000.0;
    public double IScaleDistance(double d) => d * _distancePerUnit * 1000.0;
    public double ScaleTime(double t) => t * _timePerUnit;
    public double2 ScaleAcceleration(double2 acc) => new double2(acc.x * _accScale, acc.y * _accScale);
    public Vector2 RandPoint;
    public GameObject obj;
    public Rigidbody2D rb;
    private double2 v;

    public GameObject orbiting;
    public CelestialBody orbitl;

    private void Start()
    {
        rb = obj.GetComponent<Rigidbody2D>();
        CelestialBodyController[] CB = GetComponentsInChildren<CelestialBodyController>();
        foreach (CelestialBodyController cb in CB)
        {
            SystemBodies.Add(new CelestialBodyStore(cb.GetComponent<Transform>(), cb.Body));
        }

        celestialBodyTransformer = GetComponent<CelestialBodyTransformer>();

        // Default to the largest position
        Switch(systemSwitches[systemSwitches.Length - 1]);

        _accScale = _timePerUnit * _timePerUnit / (_distancePerUnit * 1000);

        obj.transform.position = new Vector2(orbiting.transform.position.x + (float)((400.0 + orbitl.Diameter / 2.0) / _distancePerUnit), orbiting.transform.position.y);
        // Calculate velocity with maximum precision using double for all intermediate calculations
        Debug.Log($"sadfd : {obj.transform.position.x - orbiting.transform.position.x}");
        double G = AstronomicalConstants.G;
        double mass = orbitl.Mass * 1e24;
        double r = IScaleDistance((400.0 + orbitl.Diameter / 2.0) / _distancePerUnit);
        double velocity = Math.Sqrt(G * mass / r) * (_timePerUnit / (_distancePerUnit * 1000));
        v = new double2(0.0, velocity);
        Debug.Log($"Velocity: {v}m/s");
    }

    public double2 GravityAccelerationAtPoint(Vector2 p)
    {
        double u = AstronomicalConstants.G;
        double MRx = 0;
        double MRy = 0;

        foreach (CelestialBodyStore cBS in SystemBodies)
        {
            double r = IScaleDistance(Vector2.Distance(cBS.T.position, p));
            double MRSquare = cBS.Body.Mass * 1e24 / (r * r);
            Vector2 dir = (Vector2)cBS.T.position - p;
            double theta = Math.Atan2(dir.y, dir.x);

            MRx += MRSquare * Math.Cos(theta);
            MRy += MRSquare * Math.Sin(theta);
        }

        return new double2(u * MRx, u * MRy);
    } 

    public void Switch(SystemSwitch systemSwitch)
    {
        _distancePerUnit = systemSwitch.DistancePerUnit;
        celestialBodyTransformer.TransformBodies(systemSwitch.Exaggeration);
    }

    private void Update()
    {
        if (systemSwitches.Length == 0) return;

        foreach (SystemSwitch s in systemSwitches)
        {
            if (Input.GetKeyDown(s.Key)) Switch(s);
        }
    }

    private void FixedUpdate()
    {
        double2 FG = GravityAccelerationAtPoint(obj.transform.position);
        double2 acceleration = ScaleAcceleration(FG);
        v += acceleration * Time.fixedDeltaTime;
        obj.transform.position += new Vector3((float)v.x, (float)v.y, 0f) * Time.deltaTime;
    }
}
