using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CelestialBodyController))]
public class AxesRotation : MonoBehaviour
{
    private CelestialBody body;

    private void Start()
    {
        body = GetComponent<CelestialBodyController>().Body;
    }
}
