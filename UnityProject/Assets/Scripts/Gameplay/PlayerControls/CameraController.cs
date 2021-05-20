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

    private MouseInputManager mouseIn;
    private GameObject parent;

    private Vector3 mainPosition;
    private Quaternion mainRotation;

    private float pitchAngle = 0f;
    private float panAngle = 0f;
    private float zoom;
    private bool mainCameraEnabled;

    private void Start()
    {
        parent = gameObject.transform.parent.gameObject;
        mouseIn = mouseInputManager.GetComponent<MouseInputManager>();
        zoom = minZoom;

        mainCameraEnabled = true;
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
        if (mainCameraEnabled)
        {
            CalculateRotationParameters();
            SetTransform();
        }
    }

    private void CalculateRotationParameters()
    {
        if (Input.GetMouseButton(1))
        {
            Vector2 delta = mouseIn.GetMouseDelta();

            pitchAngle = Mathf.Clamp(pitchAngle - (delta.y * pitchSpeed), 10f, 90f);
            panAngle += delta.x * panSpeed;
        }
        
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
