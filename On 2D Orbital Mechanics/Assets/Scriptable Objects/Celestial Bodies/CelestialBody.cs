using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

[CreateAssetMenu(fileName = "Celestial Body", menuName = "Celestial Body")]
public class CelestialBody : ScriptableObject
{
    [SerializeField] protected string _name;

    [Header("Mass (10^24 kg)")]
    [SerializeField] protected float _mass;

    [Header("Diameter (km)")]
    [SerializeField] protected float _diameter;

    [Header("Density (kg/m^3)")]
    [SerializeField] protected float _density;

    [Header("Sidereal rotation period (hrs)")]
    [SerializeField] protected float _siderealRotationPeriod;

    [Header("Distance to orbiting body (10^6 km)")]
    [SerializeField] protected float _orbitalDistance;

    [Header("Sidereal orbit period (days)")]
    [SerializeField] protected float _siderealOrbitalPeriod;

    [Header("Mean orbital velocity (km/s)")]
    [SerializeField] protected float _meanOrbitalVelocity;

    [Header("Mean Temperature (C)")]
    [SerializeField] protected float _temperature;

    public string Name => _name;
    public float Mass => _mass;
    public float Diameter => _diameter;
    public float Density => _density;
    public float LengthOfDay => _siderealRotationPeriod;
    public float OrbitalDistance => _orbitalDistance;
    public float SiderealOrbitalPeriod => _siderealOrbitalPeriod;
    public float MeanOrbitalVelocity => _meanOrbitalVelocity;
    public float Temperature => _temperature;
}
