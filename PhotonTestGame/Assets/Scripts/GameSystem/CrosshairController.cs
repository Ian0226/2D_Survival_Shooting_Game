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
        //UGUI screen point to world point,canvas needs to be world space.
        RectTransformUtility.ScreenPointToWorldPointInRectangle(_crosshairRect, Input.mousePosition, _mainCamera, out crosshairPos);
        _crosshairRect.position = crosshairPos;
    }
}
