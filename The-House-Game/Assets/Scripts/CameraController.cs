using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool canMoveCamera;

    // speed of camera
    [SerializeField] private float movementSpeed;

    // borders on the edge of the screen on which cursor moves camera (in pixels)
    [SerializeField] private float bordersWidth;

    // switchers
    [SerializeField] public bool ButtonsCameraMoveEnabled;
    [SerializeField] public bool CursorCameraMoveEnabled;

    [SerializeField] private GameObject cameraObject;
    private Transform cameraTransform;

    // extreme points that camera can see
    [SerializeField] private GameObject upperBorderObject;
    [SerializeField] private GameObject lowerBorderObject;
    [SerializeField] private GameObject leftBorderObject;
    [SerializeField] private GameObject rightBorderObject;
    private float upperBorder;
    private float lowerBorder;
    private float leftBorder;
    private float rightBorder;

    private Vector3 newPosition;

    public void SetViewBorders() {
        upperBorder = upperBorderObject.transform.position.y;
        lowerBorder = lowerBorderObject.transform.position.y;
        leftBorder = leftBorderObject.transform.position.x;
        rightBorder = rightBorderObject.transform.position.x;
    }

    void Awake()
    {
        SetViewBorders();
        cameraTransform = cameraObject.transform;
        canMoveCamera = true;
    }

    void Update()
    {
        if (canMoveCamera)
        {
            if (ButtonsCameraMoveEnabled) ButtonsCameraMove();
            if (CursorCameraMoveEnabled) CursorCameraMove();
        }
    }

    void ButtonsCameraMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0.0f || vertical != 0.0f)
        {
            Vector3 direction = new Vector3(horizontal, vertical, 0);
            MoveCamera(direction);
        }
    }

    void CursorCameraMove()
    {
        if (Input.mousePosition.x >= Screen.width  - bordersWidth && Input.mousePosition.x <= Screen.width + bordersWidth
            || Input.mousePosition.x <= 0 + bordersWidth && Input.mousePosition.x >= 0 - bordersWidth
            || Input.mousePosition.y >= Screen.height - bordersWidth && Input.mousePosition.y <= Screen.height + bordersWidth
            || Input.mousePosition.y <= 0 + bordersWidth && Input.mousePosition.y >= 0 - bordersWidth)
        {
            Vector3 direction = Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            MoveCamera(direction);
        }
    }

    void MoveCamera(Vector3 direction)
    {
        direction = Vector3.Normalize(direction);
        direction *= movementSpeed * Time.deltaTime;

        newPosition = cameraTransform.position + direction;

        newPosition.y = Mathf.Min(newPosition.y, upperBorder);
        newPosition.y = Mathf.Max(newPosition.y, lowerBorder);
        newPosition.x = Mathf.Max(newPosition.x, leftBorder);
        newPosition.x = Mathf.Min(newPosition.x, rightBorder);

        cameraTransform.position = newPosition;
    }
}
