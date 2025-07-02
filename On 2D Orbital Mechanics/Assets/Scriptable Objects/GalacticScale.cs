using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Galactic Scale", menuName = "Galactic Scale")]
public class GalacticScale : ScriptableObject
{
    // The 'Main' body of the system
    [SerializeField] private CelestialBody _host;
    [SerializeField] private int _distancePerUnit;
    [SerializeField] private float _timePerUnit;

    public CelestialBody HostBody => _host;
    public float ScaleDistance(float d) => d / _distancePerUnit;
    public float ScaleTime(float t) => t / _timePerUnit;
}
