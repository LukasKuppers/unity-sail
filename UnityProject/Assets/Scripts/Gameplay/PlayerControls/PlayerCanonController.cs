using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanonController : MonoBehaviour
{
    private static readonly int AIM_RAY_LAYER_MASK = 3;

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

    private int activeCannonIndex = 0;
    private int layerMask;

    private void Start()
    {
        inventory = inventoryObject.GetComponent<PlayerInventory>();
        cam = Camera.main;
        rb = gameObject.GetComponent<Rigidbody>();
        cannons = new IProjectileShooter[2][];
        cannons[0] = InitCanons(leftCannons);
        cannons[1] = InitCanons(rightCannons);

        layerMask = 1 << AIM_RAY_LAYER_MASK;
        layerMask = ~layerMask;
    }

    private void Awake()
    {
        PlayerInputManager inputManager = InputReference.GetInputManager();
        inputManager.AddInputListener(InputEvent.MOUSE_LEFT, FireCannons);
    }

    public void SetInventory(GameObject newInventoryObject)
    {
        inventoryObject = newInventoryObject;
        inventory = inventoryObject.GetComponent<PlayerInventory>();
    }

    private void Update()
    {
        activeCannonIndex = ControlAim();
    }

    private Vector3 GetTargetPos()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, 3000f, layerMask);
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

    private void FireCannons()
    {
        if (PlayerAttackMode.AttackEnabled())
        {
            StartCoroutine(FireActual());
        }
    }

    private IEnumerator FireActual()
    {
        int[] shootOrder = GetRandCannonOrder(cannons[activeCannonIndex].Length);

        for (int i = 0; i < shootOrder.Length; i++)
        {
            if (inventory.GetCannonballAmount() > 0)
            {
                IProjectileShooter cannon = cannons[activeCannonIndex][shootOrder[i]];
                bool didFire = cannon.Shoot();
                if (didFire)
                {
                    inventory.IncrementCannonball(-1);
                }
            }
            yield return null;
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

    private int[] GetRandCannonOrder(int numCannons)
    {
        int[] randOrder = new int[numCannons];
        for (int i = 0; i < numCannons; i++)
        {
            int rand = Random.Range(0, i + 1);
            if (i != rand)
                randOrder[i] = randOrder[rand];

            randOrder[rand] = i;
        }
        return randOrder;
    }
}
