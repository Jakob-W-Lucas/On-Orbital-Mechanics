using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private int _distancePerUnit;
    [SerializeField] private float _timePerUnit;
    [SerializeField] private SystemSwitch[] systemSwitches;
    private CelestialBodyTransformer celestialBodyTransformer;

    public CelestialBody HostBody => _host;
    public double ScaleDistance(double d) => d / _distancePerUnit / 1000.0;
    public double IScaleDistance(double d) => d * _distancePerUnit * 1000.0;
    public double ScaleTime(double t) => t * _timePerUnit;

    private void Start()
    {
        celestialBodyTransformer = GetComponent<CelestialBodyTransformer>();
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
}
