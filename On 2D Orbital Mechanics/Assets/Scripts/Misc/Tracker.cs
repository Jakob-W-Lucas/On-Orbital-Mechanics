using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    [SerializeField] private CelestialBody CB;
    private bool isTiming = false;
    private float timer = 0f;
    private bool hasEnteredNegative = false;

    private void Update()
    {
        if (!hasEnteredNegative)
        {
            // Start timing when object is not yet in negative x and y
            if (!isTiming && (transform.position.x > 0 || transform.position.y > 0))
            {
                isTiming = true;
                timer = 0f;
            }

            // If timing, increment timer
            if (isTiming)
            {
                timer += Time.deltaTime;
            }

            // Check if object has entered negative x and y
            if (transform.position.x <= 0 && transform.position.y <= 0)
            {
                hasEnteredNegative = true;
                isTiming = false;
                Debug.Log($"Time to reach negative x and y for {CB.Name} is: {timer} seconds");
            }
        }
    }
}
