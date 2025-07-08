using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CameraControl
{
    public KeyCode Key;
    public GameObject Focus;
    public int Size;
}

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraControl[] _cameraControls;
    private GameObject _focusObj;
    private Camera main;
    private void Start()
    {
        main = Camera.main;

        CameraControl sCamera = _cameraControls[0];
        _focusObj = sCamera.Focus;
        main.orthographicSize = sCamera.Size;
    }
    private void Update()
    {
        main.transform.position = _focusObj ? new Vector3(_focusObj.transform.position.x, _focusObj.transform.position.y, main.transform.position.z) : Vector2.zero;

        if (_cameraControls.Length == 0) return;

        foreach (CameraControl cc in _cameraControls)
        {
            if (Input.GetKeyDown(cc.Key))
            {
                _focusObj = cc.Focus;
                main.orthographicSize = cc.Size;
            }
        }
    }
}
