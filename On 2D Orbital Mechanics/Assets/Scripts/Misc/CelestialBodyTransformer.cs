using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CelestialBodyTransformer : MonoBehaviour
{
    private SystemController systemLookup;
    private Transform[] bodies;

    private void Awake()
    {
        systemLookup = GetComponent<SystemController>();
        bodies = GetComponentsInChildren<Transform>();
    }

    public void TransformBodies(int exaggeration)
    {
        foreach (Transform t in bodies)
        {
            if (t.TryGetComponent<CelestialBodyController>(out var cBC)) Scale(t, cBC.Body, exaggeration);
        }
    }

    void Scale(Transform t, CelestialBody b, int e)
    {
        float scale = (float)systemLookup.ScaleDistance(b.Diameter) * 1000 * e;

        Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
        t.localScale = new Vector3(
            scale / parentScale.x,
            scale / parentScale.y,
            0
        );
    }
}
