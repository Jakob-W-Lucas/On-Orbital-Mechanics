using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct BodyTransform
{
    public Transform Obj;
    public CelestialBody Body;

    // Allows for objects to be exagerated beyond what they would normally be represented at, at this scale
    public int Exaggeration;
}
public class CelestialBodyTransformer : MonoBehaviour
{
    [SerializeField] private BodyTransform[] _bodies;
    private SystemLookup systemLookup;

    private void Start()
    {
        systemLookup = SystemController.SystemLookup;

        foreach (BodyTransform b in _bodies) Scale(b);
    }

    void Scale(BodyTransform b)
    {
        float scale = systemLookup.ScaleDistance(b.Body.Diameter) * b.Exaggeration;

        Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
        b.Obj.localScale = new Vector3(
            scale / parentScale.x,
            scale / parentScale.y,
            0
        );
    }
}
