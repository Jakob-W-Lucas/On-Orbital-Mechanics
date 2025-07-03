using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemController : MonoBehaviour
{
    [SerializeField] private SystemLookup _systemLookup;
    public static SystemLookup SystemLookup { get; private set; }

    private void Awake()
    {
        SystemLookup = _systemLookup;
    }
}
