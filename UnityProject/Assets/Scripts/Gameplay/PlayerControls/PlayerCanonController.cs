using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanonController : MonoBehaviour
{
    [SerializeField]
    private GameObject[] leftCanons;
    [SerializeField]
    private GameObject[] rightCanons;

    private CameraController camController;
    private Rigidbody rb;
    private IProjectileShooter[][] canons;

    private float pitch = 0f;
    private float yaw = 0f;

    private int fireMode;

    private void Start()
    {
        camController = Camera.main.gameObject.GetComponent<CameraController>();
        rb = gameObject.GetComponent<Rigidbody>();

        canons = new IProjectileShooter[2][];
        canons[0] = InitCanons(leftCanons);
        canons[1] = InitCanons(rightCanons);

        fireMode = 2;
    }

    private void Update()
    {
        ControlFireMode();
        if (fireMode != 2)
        {
            ControlAim();

            foreach (IProjectileShooter canon in canons[fireMode])
            {
                canon.DisplayAim();
            }
            ControlFire();
        }
    }

    private void ControlFireMode()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            CycleFireMode();
        }
    }

    private void ControlAim()
    {
        Vector2 delta = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
        pitch = Mathf.Clamp(pitch + delta.y, 0f, 45f);
        yaw = Mathf.Clamp(yaw + delta.x, -30f, 30f);

        foreach (IProjectileShooter canon in canons[fireMode])
        {
            canon.SetOrientation(yaw, pitch);
        }
    }

    private void ControlFire()
    {
        if (Input.GetMouseButton(0))
        {
            foreach (IProjectileShooter canon in canons[fireMode])
            {
                canon.Shoot();
                fireMode = 1;
                CycleFireMode();
            }
        }
    }

    private void CycleFireMode()
    {
        fireMode = (fireMode + 1) % 3;
        if (fireMode == 2)
        {
            camController.EnableMainCamera();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            camController.EnableSpecialCamera(fireMode);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private IProjectileShooter[] InitCanons(GameObject[] canonObjects)
    {
        List<IProjectileShooter> canonsList = new List<IProjectileShooter>();

        foreach (GameObject obj in canonObjects)
        {
            IProjectileShooter canon = obj.GetComponent<IProjectileShooter>();
            canon.SetOrientation(0, 20f);
            canon.SetParentRigidbody(rb);

            canonsList.Add(canon);
        }
        return canonsList.ToArray();
    }
}
