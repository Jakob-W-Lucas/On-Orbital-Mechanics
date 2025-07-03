using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera main;
    private void Start()
    {
        main = Camera.main;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            main.orthographicSize = 350;
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            main.orthographicSize = 4000;
        }
    }
}
