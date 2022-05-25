using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyIslandVisitManager : MonoBehaviour
{
    [SerializeField]
    private string islandColliderTag = "IslandProximityCol";

    UnityEvent<Islands> visitEvent;

    public void AddVisitListener(UnityAction<Islands> call)
    {
        if (visitEvent == null)
            visitEvent = new UnityEvent<Islands>();

        visitEvent.AddListener(call);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject colObj = collision.gameObject;
        if (colObj.CompareTag(islandColliderTag))
        {
            if (visitEvent != null)
            {
                IslandVisitCollider islandCollider = colObj.GetComponent<IslandVisitCollider>();
                Islands island = islandCollider.GetIsland();
                visitEvent.Invoke(island);
            }
        }
    }
}
