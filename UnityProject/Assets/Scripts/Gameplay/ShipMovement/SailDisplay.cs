using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SailDisplay : MonoBehaviour
{
    private static readonly string COLOR_REF = "_SailColor";
    private static readonly string HEIGHT_REF = "_SailHeight";
    private static readonly string WIND_REF = "_WindStrength";

    [SerializeField]
    private GameObject[] sails;
    [SerializeField]
    private Color sailColor;

    private Material[] sailMaterials;
    private IShipController controller;

    private void Start()
    {
        controller = transform.parent.gameObject.GetComponent<IShipController>();

        List<Material> mats = new List<Material>();
        foreach (GameObject sail in sails)
        {
            Material mat = sail.GetComponent<MeshRenderer>().material;
            mat.SetColor(COLOR_REF, sailColor);
            mats.Add(mat);
        }
        sailMaterials = mats.ToArray();
    }

    private void Update()
    {
        SetSailAngle();
        SetSailParamters();
    }

    private void SetSailAngle()
    {
        float angle = controller.GetSailAngle();
        transform.localEulerAngles = new Vector3(0, angle, 0);
    }

    private void SetSailParamters()
    {
        float height = Mathf.Clamp(controller.GetSailHeight(), 0.1f, 1f);
        float strength = controller.GetSailMultiplier();

        foreach (Material mat in sailMaterials)
        {
            mat.SetFloat(HEIGHT_REF, height);
            mat.SetFloat(WIND_REF, strength);
        }
    }
}
