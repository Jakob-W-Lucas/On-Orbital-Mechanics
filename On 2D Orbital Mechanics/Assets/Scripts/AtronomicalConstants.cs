using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
public static class AstronomicalConstants
{ 
    // Gravitational Constant (m^3/kg^-1/s^-2)
    public const float G = 0.000000000066743f;

    // Solar mass (kg)
    public const float SolarMass = 1989000000000000000000000000000f;

    // Galactic scale (all values are divided by this value so they are not ridiculous)
    // Unity units per km
    public const int GalacticScale = 56000;
}
