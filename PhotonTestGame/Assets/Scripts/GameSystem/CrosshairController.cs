using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairController : MonoBehaviour
{
    private Camera _mainCamera;
    private RectTransform _crosshairRect;
    private void Start()
    {
        _mainCamera = Camera.main;
        _crosshairRect = this.GetComponent<RectTransform>();
    }
    private void Update()
    {
        CrosshairHandler();
    }
    private void CrosshairHandler()
    {
        Vector3 crosshairPos;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_crosshairRect, Input.mousePosition, _mainCamera, out crosshairPos);
        _crosshairRect.position = crosshairPos;
    }
}
