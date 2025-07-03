using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SystemLookup", menuName = "SystemLookup")]
public class SystemLookup : ScriptableObject
{
    // The 'Main' body of the system
    [SerializeField] private CelestialBody _host;
    [SerializeField] private int _distancePerUnit;
    [SerializeField] private float _timePerUnit;

    public CelestialBody HostBody => _host;
    
    public float ScaleDistance(float d) => d / _distancePerUnit;
    public (float a, float b) ScaleDistance(float a, float b) => (a / _distancePerUnit, b / _distancePerUnit);

    public float ScaleTime(float t) => t / _timePerUnit;
}
