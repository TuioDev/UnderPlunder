using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float CameraHeight = -10f;
    [SerializeField] private float CameraSpeed;
    [SerializeField] private float CameraZoomOut;
    [SerializeField] private float CameraZoomIn;
    [SerializeField] private float ZoomDuration;

    private Transform PlayerReference;
    private Camera MainCameraReference;
    private Vector3 TargetPosition;
    private float CurrentTime = 0f;
    private bool CanZoom = false;

    private void Awake()
    {
        //ZoomDuration = 1/ZoomDuration;
        MainCameraReference = Camera.main;
    }
    void Start()
    {
        PlayerReference = Player.Instance.transform;
    }
    void Update()
    {
        TargetPosition = new Vector3(PlayerReference.position.x, PlayerReference.position.y, CameraHeight);
        if (CanZoom) //Executes ZoomIn, now find a way to execute ZoomOut
        {
            if (CurrentTime < ZoomDuration)
            {
                CurrentTime += Time.deltaTime;
                MainCameraReference.orthographicSize = Mathf.Lerp(CameraZoomOut, CameraZoomIn, CurrentTime / ZoomDuration);
            }
            else
            {
                CanZoom = false;
                CurrentTime = 0f;
            }
        }
    }
    private void LateUpdate()
    {
        //Make the camera smoothly transition to the player
        this.transform.position = Vector3.Lerp(this.transform.position, TargetPosition, CameraSpeed * Time.deltaTime);
    }
    public void ExecuteZoomIn()
    {
        CanZoom = true;        
    }
    void ExecuteZoomOut()
    {
        CanZoom = true;
        Camera.main.orthographicSize = CameraZoomOut;
    }
}
