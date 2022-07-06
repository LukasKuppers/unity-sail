using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindFlag : MonoBehaviour
{
    [SerializeField]
    private GameObject windGenerator;

    private WindGenerator wind;

    private void Start()
    {
        wind = windGenerator.GetComponent<WindGenerator>();
    }

    private void Update()
    {
        SetRotation();    
    }

    public void SetWindGenerator(GameObject newWindGenerator)
    {
        windGenerator = newWindGenerator;
        wind = windGenerator.GetComponent<WindGenerator>();
    }

    private void SetRotation()
    {
        Vector3 shipForward = transform.parent.transform.forward;
        Vector2 shipVec = new Vector2(shipForward.x, shipForward.z);
        Vector2 windVec = Vector2Util.DegreeToVector2(wind.GetWindDirection());

        float localWindAngle = Vector2.SignedAngle(shipVec, windVec);

        transform.localEulerAngles = new Vector3(0, -localWindAngle, 0);
    }
}
