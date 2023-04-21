using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private bool canMoveCamera;

    // speed of camera
    [SerializeField] private float movementSpeed;
    [SerializeField] private float zoomSpeed;

    // borders on the edge of the screen on which cursor moves camera (in pixels)
    [SerializeField] private float bordersPercentageWidth;

    // switchers
    [SerializeField] public bool ButtonsCameraMoveEnabled;
    [SerializeField] public bool CursorCameraMoveEnabled;
    [SerializeField] public bool MouseWheelZoomEnabled;
    [SerializeField] public bool ZoomToCursorEnabled;

    [SerializeField] private GameObject cameraObject;
    private Transform cameraTransform;

    // extreme points that camera can see
    private float upperBorder;
    private float lowerBorder;
    private float leftBorder;
    private float rightBorder;
    [SerializeField] private float backBorder;
    [SerializeField] private float frontBorder;
    [SerializeField] private float borderWidth;

    private Map gameMap;
    private bool bordersSeted = false;

    private Vector3 newPosition;

    Vector3 CorrectCoordinates(Vector3 coordinates) {
        coordinates[0] = Mathf.Max(coordinates[0], leftBorder);
        coordinates[0] = Mathf.Min(coordinates[0], rightBorder);
        coordinates[1] = Mathf.Max(coordinates[1], lowerBorder);
        coordinates[1] = Mathf.Min(coordinates[1], upperBorder);
        return coordinates;
    }


    void Awake()
    {
        Cursor.lockState = CursorLockMode.Confined; 
        cameraTransform = cameraObject.transform;
        canMoveCamera = false;
        gameMap = GameObject.Find("Map").GetComponent<Map>();
    }


    void Update()
    {
        if (!bordersSeted) {
            if (gameMap.Ready) {;
                upperBorder = gameMap.upperBorder + borderWidth;
                lowerBorder = gameMap.lowerBorder - borderWidth;
                leftBorder  = gameMap.leftBorder  - borderWidth;
                rightBorder = gameMap.rightBorder + borderWidth; 
                bordersSeted = true;
                canMoveCamera = true;
            }
        }
        else if (canMoveCamera) {
            if (ButtonsCameraMoveEnabled) ButtonsCameraMove();
            if (CursorCameraMoveEnabled) CursorCameraMove();
            if (MouseWheelZoomEnabled) MouseWheelZoom();
        }
    }


    void ButtonsCameraMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (horizontal != 0.0f || vertical != 0.0f) {
            Vector3 direction = new Vector3(horizontal, vertical, 0);
            MoveCamera(direction);
        }
    }


    void CursorCameraMove()
    {
        if ((Input.mousePosition.x >= Screen.width *  (1 - bordersPercentageWidth / 100)) ||
            (Input.mousePosition.x <= Screen.width *  (0 + bordersPercentageWidth / 100)) ||
            (Input.mousePosition.y >= Screen.height * (1 - bordersPercentageWidth / 100)) ||
            (Input.mousePosition.y <= Screen.height * (0 + bordersPercentageWidth / 100))) {
            Vector3 direction = Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            MoveCamera(direction);
        }
    }


    void MouseWheelZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0.0f) {
            Vector3 direction;
            if (ZoomToCursorEnabled)
                direction = Camera.main.ScreenPointToRay(Input.mousePosition).direction * scroll * zoomSpeed * Time.deltaTime;
            else
                direction = cameraTransform.forward * scroll * zoomSpeed * Time.deltaTime;

            if ((direction[2] > 0 && cameraTransform.position.z < frontBorder) ||
                (direction[2] < 0 && cameraTransform.position.z > backBorder)) {
                cameraTransform.position = CorrectCoordinates(cameraTransform.position + direction);
            }
        } 
    }


    void MoveCamera(Vector3 direction)
    {
        direction = Vector3.Normalize(direction);
        direction *= movementSpeed * Time.deltaTime;

        SetCamera(cameraTransform.position + direction);
    }


    public void SetCamera(Vector3 position)
    {
        cameraTransform.position = CorrectCoordinates(position);
    }


    public Transform getCameraTransform()
    {
        return cameraTransform;
    }
}
