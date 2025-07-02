using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixedOrbit : MonoBehaviour
{
    [SerializeField] private float _w;
    [SerializeField] private float _r;
    private float _t;

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        // Get the fixed time update
        _t += Time.fixedDeltaTime;
        
        // Calculate the position around the circular orbit
        float x = _r * Mathf.Cos(_w * _t);
        float y = _r * Mathf.Sin(_w * _t);

        transform.position = new Vector2(x, y);
    }
}   
