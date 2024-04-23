using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{

    [SerializeField] private MeshRenderer _meshRenderer;
    private MaterialPropertyBlock _matBlock;

    private Camera _mainCamera;


    private void Start()
    {
        _matBlock = new MaterialPropertyBlock();
        _mainCamera = Camera.main;
    }

    private void Update()
    {
        AlignCamera();
    }


    public void SetValue(float value)
    {
        _meshRenderer.GetPropertyBlock(_matBlock);
        _matBlock.SetFloat("_Fill", value);
        _meshRenderer.SetPropertyBlock(_matBlock);
    }

    private void AlignCamera()
    {
        if (_mainCamera != null)
        {
            var camXform = _mainCamera.transform;
            var forward = transform.position - camXform.position;
            forward.Normalize();
            var up = Vector3.Cross(forward, camXform.right);
            transform.rotation = Quaternion.LookRotation(forward, up);
        }
    }
}
