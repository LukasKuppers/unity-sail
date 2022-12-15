using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseInputManager;
    [SerializeField]
    private GameObject[] specialCameraPositions;

    [SerializeField]
    private float elevationAngle = 10f;
    [SerializeField]
    private float angleSwitchTime = 0.5f;
    [SerializeField]
    private float panSpeed = 0.5f;
    [SerializeField]
    private float pitchSpeed = 0.5f;
    [SerializeField]
    private float zoomSpeed = 0.5f;
    [SerializeField]
    private float minZoom = 10f;
    [SerializeField]
    private float maxZoom = 50f;

    [SerializeField]
    private bool smoothCamera = true;
    [SerializeField]
    private float smoothSensitivityAmount = 10f;
    [SerializeField]
    private float smoothGravityAmount = 5f;

    private MouseInputManager mouseIn;
    private GameObject parent;

    private Vector2 mouseDelta;
    private Vector3 mainPosition;
    private Quaternion mainRotation;

    private float pitchAngle = 0f;
    private float panAngle = 0f;
    private float zoom;
    private bool mainCameraEnabled;

    private void Start()
    {
        mouseDelta = Vector2.zero;

        parent = gameObject.transform.parent.gameObject;
        mouseIn = mouseInputManager.GetComponent<MouseInputManager>();

        // set init position
        zoom = (minZoom + maxZoom) / 2f;
        pitchAngle = 45f;

        mainCameraEnabled = true;
    }

    public void SetCameraSmooth(bool isSmooth)
    {
        smoothCamera = isSmooth;
    }

    public void EnableMainCamera()
    {
        if (!mainCameraEnabled)
        {
            StartCoroutine(SwitchPosition(mainPosition, mainRotation));
            mainCameraEnabled = true;
        }
    }

    public void EnableSpecialCamera(int cameraIndex)
    {
        if (mainCameraEnabled)
        {
            mainPosition = transform.localPosition;
            mainRotation = transform.localRotation;
        }
        mainCameraEnabled = false;

        cameraIndex %= specialCameraPositions.Length;
        GameObject newCamPos = specialCameraPositions[cameraIndex];

        StartCoroutine(SwitchPosition(newCamPos.transform.localPosition, newCamPos.transform.localRotation));
    }

    private void Update()
    {
        if (mainCameraEnabled && PlayerSceneInteraction.InteractionEnabled())
        {
            CalculateRotationParameters();
            SetTransform();
        }
    }

    private void CalculateRotationParameters()
    {
        Vector2 rawDelta = mouseIn.GetMouseDelta();

        if (smoothCamera)
        {
            if (Input.GetMouseButton(1) && rawDelta.magnitude > mouseDelta.magnitude)
                mouseDelta += rawDelta / smoothSensitivityAmount;
            else
                mouseDelta -= mouseDelta / smoothGravityAmount;

            mouseDelta.x = Mathf.Clamp(mouseDelta.x, -panSpeed * 100f, panSpeed * 100f);
            mouseDelta.y = Mathf.Clamp(mouseDelta.y, -pitchSpeed * 100f, pitchSpeed * 100f);
        }
        else
        {
            mouseDelta = Vector3.zero;
            if (Input.GetMouseButton(1))
                mouseDelta = rawDelta;
        }

        pitchAngle = Mathf.Clamp(pitchAngle - (mouseDelta.y * pitchSpeed), 10f, 90f);
        panAngle += mouseDelta.x * panSpeed;
        
        zoom = Mathf.Clamp(zoom - (Input.mouseScrollDelta.y * zoomSpeed), minZoom, maxZoom);
    }

    private void SetTransform()
    {
        transform.position = parent.transform.position;
        Vector3 forward = new Vector3(parent.transform.forward.x, 0, parent.transform.forward.z);
        transform.rotation = Quaternion.LookRotation(forward, Vector3.up);

        transform.RotateAround(transform.position, transform.right, pitchAngle);
        transform.RotateAround(transform.position, Vector3.up, panAngle);
        transform.position -= (transform.forward * zoom);

        transform.Rotate(new Vector3(-elevationAngle, 0, 0), Space.Self);
    }

    private IEnumerator SwitchPosition(Vector3 newPos, Quaternion newRot)
    {
        Vector3 originalPos = transform.localPosition;
        Quaternion originalRot = transform.localRotation;

        for (float i = 0; i <= angleSwitchTime; i += Time.deltaTime)
        {
            float t = Mathf.SmoothStep(0, 1, i / angleSwitchTime);
            Vector3 tempPos = Vector3.Lerp(originalPos, newPos, t);
            Quaternion tempRot = Quaternion.Lerp(originalRot, newRot, t);
            transform.localPosition = tempPos;
            transform.localRotation = tempRot;

            yield return null;
        }

        transform.localPosition = newPos;
        transform.localRotation = newRot;
    }
}
