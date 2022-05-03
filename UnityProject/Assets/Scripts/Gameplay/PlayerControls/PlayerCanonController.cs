using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanonController : MonoBehaviour
{
    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private GameObject[] leftCannons;
    [SerializeField]
    private GameObject[] rightCannons;

    private PlayerInventory inventory;
    private Camera cam;
    private Rigidbody rb;
    private IProjectileShooter[][] cannons;

    private void Start()
    {
        inventory = inventoryObject.GetComponent<PlayerInventory>();
        cam = Camera.main;
        rb = gameObject.GetComponent<Rigidbody>();
        cannons = new IProjectileShooter[2][];
        cannons[0] = InitCanons(leftCannons);
        cannons[1] = InitCanons(rightCannons);
    }

    public void SetInventory(GameObject newInventoryObject)
    {
        inventoryObject = newInventoryObject;
        inventory = inventoryObject.GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        int cannonIndex = ControlAim();
        ControlFire(cannonIndex);
    }

    private Vector3 GetTargetPos()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit);
        return hit.point;
    }

    private int ControlAim()
    {
        Vector3 targetPos = GetTargetPos();

        Vector2 diff_2d = new Vector2(targetPos.x - transform.position.x,
            targetPos.z - transform.position.z);
        Vector2 right = new Vector2(transform.right.x, transform.right.z);
        Vector2 left = -right;
        float aimAngleRight = Vector2.SignedAngle(right, diff_2d);
        float aimAngleLeft = Vector2.SignedAngle(left, diff_2d);

        float aimAngle = Mathf.Abs(aimAngleLeft) < Mathf.Abs(aimAngleRight) ? aimAngleLeft : aimAngleRight;
        int cannonIndex = aimAngle == aimAngleLeft ? 0 : 1;

        Vector3 diff_3d = targetPos - transform.position;
        float range = Vector3.Scale(diff_3d, new Vector3(1, 0, 1)).magnitude;
        float height = transform.position.y - targetPos.y;

        foreach (IProjectileShooter cannon in cannons[cannonIndex])
        {
            cannon.SetOrientation(-aimAngle, 0f);
            cannon.SetPitch(range, height);
        }

        return cannonIndex;
    }

    private void ControlFire(int cannonIndex)
    {
        if (Input.GetMouseButton(0) && PlayerAttackMode.AttackEnabled())
        {
            foreach (IProjectileShooter cannon in cannons[cannonIndex])
            {
                if (inventory.GetCannonballAmount() > 0)
                {
                    bool didFire = cannon.Shoot();
                    if (didFire)
                    {
                        inventory.IncrementCannonball(-1);
                    }
                }
            }
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
