using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelestialBodyController : MonoBehaviour
{
    [SerializeField] private CelestialBody _celestialBody;
    [SerializeField] private CelestialBody _orbitingBody;
    public CelestialBody Body => _celestialBody;
    public CelestialBody Orbiting => _orbitingBody;
    private void Awake()
    {
        _orbitingBody = transform.parent != null && transform.parent.TryGetComponent<CelestialBodyController>(out var CBC) ?
                        CBC.Body :
                        GetComponentInParent<SystemController>().HostBody;
    }
}
