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
    [SerializeField] private int DefaultExaggeration;
    [SerializeField] private int HostExaggeration;
    [SerializeField] private BodyTransform[] _bodies;
    private SystemLookup systemLookup;

    private void Start()
    {
        systemLookup = SystemController.SystemLookup;

        if (_bodies.Length == 0)
        {
            Transform[] bodies = GetComponentsInChildren<Transform>();

            foreach (Transform t in bodies)
            {
                if (t.TryGetComponent<CelestialBodyController>(out var cBC))
                {
                    int exaggeration = HostExaggeration > 0 && cBC.Body == systemLookup.HostBody ? HostExaggeration : DefaultExaggeration;
                    Scale(t, cBC.Body, exaggeration);
                }
            }
        }

        foreach (BodyTransform b in _bodies) Scale(b.Obj, b.Body, b.Exaggeration);
    }

    void Scale(Transform t, CelestialBody b, int e)
    {
        float scale = systemLookup.ScaleDistance(b.Diameter) * e;

        Vector3 parentScale = transform.parent != null ? transform.parent.lossyScale : Vector3.one;
        t.localScale = new Vector3(
            scale / parentScale.x,
            scale / parentScale.y,
            0
        );
    }
}
