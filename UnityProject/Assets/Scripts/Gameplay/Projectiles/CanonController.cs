using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonController : MonoBehaviour, IProjectileShooter
{
    private static readonly string COROUTINE_CALL_NAME = "ReloadTimer";

    [SerializeField]
    private string tagMask;
    [SerializeField]
    private GameObject cannonModel;
    [SerializeField]
    private GameObject baseModel;
    [SerializeField]
    private GameObject firePoint;
    [SerializeField]
    private GameObject cannonBallPrefab;

    [SerializeField]
    private float reloadTime = 1f;
    [SerializeField]
    private float fireForce = 10f;
    [SerializeField]
    private float yawLimit = 30f;
    [SerializeField]
    private float pitchLimit = 45f;

    private ProjectilePathRenderer pathRendrer;
    private Rigidbody parentRb;
    private bool fireReady = true;

    private void Start()
    {
        pathRendrer = gameObject.GetComponent<ProjectilePathRenderer>();
        ValidateParams(yawLimit, pitchLimit);
    }

    public void DisplayAim()
    {
        pathRendrer.RenderPathForFrame(firePoint.transform.position, firePoint.transform.forward * fireForce, Physics.gravity.magnitude);
    }

    public void SetParentRigidbody(Rigidbody rb)
    {
        if (rb == null)
        {
            Debug.LogError("Canon: parent rigidbody cannot be null");
        }
        parentRb = rb;
    }

    public void SetRotationLimits(float yaw, float pitch)
    {
        if (ValidateParams(yaw, pitch))
        {
            yawLimit = yaw;
            pitchLimit = pitch;
        }
    }

    public void SetOrientation(float yaw, float pitch)
    {
        float y = Mathf.Clamp(yaw, -yawLimit, yawLimit);
        float x = Mathf.Clamp(pitch, 0, pitchLimit);

        baseModel.transform.localEulerAngles = new Vector3(0, y, 0);
        cannonModel.transform.localEulerAngles = new Vector3(-x, 0, 0);
    }

    public void SetPitch(float range, float height)
    {
        float g = Physics.gravity.magnitude;

        float temp = Mathf.Pow(fireForce, 4) - (2 * fireForce * fireForce * -height * g) - (g * g * range * range);
        temp = temp < 0 ? 0 : temp;
        float pitch = Mathf.Atan(((fireForce * fireForce) - Mathf.Sqrt(temp)) / (g * range));
        pitch *= Mathf.Rad2Deg;

        pitch = Mathf.Clamp(pitch, 0, pitchLimit);
        cannonModel.transform.localEulerAngles = new Vector3(-pitch, 0, 0);
    }

    public void Shoot()
    {
        if (fireReady)
        {
            GameObject cannonBall = Instantiate(cannonBallPrefab, firePoint.transform.position, firePoint.transform.rotation);
            Projectile proj = cannonBall.GetComponent<Projectile>();
            if (proj != null)
            {
                proj.SetTagMask(tagMask);
            }

            Rigidbody rb = cannonBall.GetComponent<Rigidbody>();

            rb.velocity = firePoint.transform.forward * fireForce;
            if (parentRb != null)
            {
                rb.velocity += parentRb.velocity;
            }

            fireReady = false;
            StartCoroutine(COROUTINE_CALL_NAME);
        }
        
    }

    private bool ValidateParams(float xRot, float yRot)
    {
        if (xRot > 180f)
        {
            Debug.LogError("Canon: X rotation limit must be less than 180 degrees");
            return false;
        }
        if (yRot > 90f)
        {
            Debug.LogError("Canon: Y rotation limit must be less than 90 degrees");
            return false;
        }
        return true;
    }

    private IEnumerator ReloadTimer()
    {
        yield return new WaitForSeconds(reloadTime);
        fireReady = true;
    }
}
