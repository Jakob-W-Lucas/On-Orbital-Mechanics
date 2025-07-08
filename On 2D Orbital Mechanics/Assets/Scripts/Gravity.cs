using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private SystemController systemController;

    private void Start()
    {
        systemController = transform.parent.GetComponent<SystemController>();
    }

    private void Update()
    {
        
    }
}
