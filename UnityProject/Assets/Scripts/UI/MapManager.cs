using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    private static readonly string INTERACTION_LOCK_KEY = "Map_manager_key";

    [SerializeField]
    private GameObject contentContainer;
    [SerializeField]
    private GameObject playerPosIcon;
    [SerializeField]
    private float worldMapWidth;
    [SerializeField]
    private float worldMapHeight;

    private GameObject playerShip;
    private float containerWidth = 1f;
    private float containerHeight = 1f;

    private RectTransform playerPosRect;

    private void Awake()
    {
        playerPosRect = playerPosIcon.GetComponent<RectTransform>();

        PlayerSceneInteraction.DisableInteraction(INTERACTION_LOCK_KEY);

        StartCoroutine(DelayedAwake());
    }

    private void OnDestroy()
    {
        PlayerSceneInteraction.EnableInteraction(INTERACTION_LOCK_KEY);
    }

    public void SetShip(GameObject ship)
    {
        playerShip = ship;
    }

    private IEnumerator DelayedAwake()
    {
        yield return new WaitForEndOfFrame();

        RectTransform containerTransform = contentContainer.GetComponent<RectTransform>();
        containerWidth = containerTransform.rect.size.x;
        containerHeight = containerTransform.rect.size.y;
    }

    private void Update()
    {
        Vector3 playerMapPos = WorldToMapPos(playerShip.transform.position);
        playerPosRect.anchoredPosition = playerMapPos;
    }

    private Vector3 WorldToMapPos(Vector3 worldPos)
    {
        float relativePosX = worldPos.x / worldMapWidth;
        float relativePosY = worldPos.z / worldMapHeight;

        return new Vector3(relativePosX * containerWidth, relativePosY * containerHeight, 0);
    }
}
