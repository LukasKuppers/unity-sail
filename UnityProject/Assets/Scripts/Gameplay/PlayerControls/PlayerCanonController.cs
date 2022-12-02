using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanonController : MonoBehaviour
{
    public static readonly int AIM_RAY_LAYER_MASK = 3;

    [SerializeField]
    private GameObject inventoryObject;
    [SerializeField]
    private GameObject[] leftCannons;
    [SerializeField]
    private GameObject[] rightCannons;
    [SerializeField]
    private float verticalCannonOffset;

    private PlayerInventory inventory;
    private Camera cam;
    private Rigidbody rb;
    private IProjectileShooter[][] cannons;

    private int activeCannonIndex = 0;
    private int layerMask;

    private bool yawInRange = false;
    private bool pitchInRange = false;

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
        inputManager.AddInputListener(InputEvent.HOLD_MOUSE_LEFT, ChargeFire);
    }

    public void SetInventory(GameObject newInventoryObject)
    {
        inventoryObject = newInventoryObject;
        inventory = inventoryObject.GetComponent<PlayerInventory>();
    }

    public bool TargetInRange()
    {
        return yawInRange && pitchInRange;
    }

    private void Update()
    {
        activeCannonIndex = ControlAim();
    }

    private Vector3 GetTargetPos()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 3000f, layerMask))
        {
            Vector3 target = hit.point;
            target -= rb.velocity;

            // lead moving target
            Rigidbody targetRb = hit.transform.gameObject.GetComponent<Rigidbody>();
            if (targetRb != null)
                target += targetRb.velocity;

            return target;
        }
        return Vector3.zero;
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
        float height = (transform.position.y + verticalCannonOffset) - targetPos.y;

        yawInRange = false;
        pitchInRange = false;
        foreach (IProjectileShooter cannon in cannons[cannonIndex])
        {
            yawInRange = cannon.SetOrientation(-aimAngle, 0f) || yawInRange;
            pitchInRange = cannon.SetPitch(range, height) || pitchInRange;
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

    private void ChargeFire()
    {
        if (PlayerAttackMode.AttackEnabled())
            StartCoroutine(ChargeFireActual());
    }

    private IEnumerator FireActual()
    {
        int[] shootOrder = GetRandCannonOrder(cannons[activeCannonIndex].Length);

        for (int i = 0; i < shootOrder.Length; i++)
        {
            IProjectileShooter cannon = cannons[activeCannonIndex][shootOrder[i]];
            FireCannon(cannon);
            yield return null;
        }
    }

    private IEnumerator ChargeFireActual()
    {
        int[] shootOrder = GetRandCannonOrder(cannons[0].Length);
        
        for (int i = 0; i < shootOrder.Length; i++)
        {
            IProjectileShooter cannonOne = cannons[0][shootOrder[i]];
            IProjectileShooter cannonTwo = cannons[1][shootOrder[i]];

            cannonOne.SetOrientation(0f, 0f);
            cannonTwo.SetOrientation(0f, 0f);

            FireCannon(cannonOne);
            FireCannon(cannonTwo);
            yield return null;
        }
    }

    private void FireCannon(IProjectileShooter cannon)
    {
        if (inventory.GetCannonballAmount() > 0)
        {
            bool didFire = cannon.Shoot();
            if (didFire)
                inventory.IncrementCannonball(-1);
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
