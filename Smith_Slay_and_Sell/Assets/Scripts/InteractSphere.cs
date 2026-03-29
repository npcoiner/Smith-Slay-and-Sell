using UnityEngine;
using System.Collections.Generic;


// TODO filter for pickupable/interactables probably using tags.
public class InteractSphere : MonoBehaviour
{
    private List<GameObject> objectsInRange = new List<GameObject>();
    void Update()
    {
        foreach (var obj in objectsInRange)
        {
            //Debug.Log($"In range: {obj.name}");
        }
    }

    public GameObject GetNearestInRange()
    {
        //Remove any destroyed or missing objects for whatever reason.
        objectsInRange.RemoveAll(item => item == null);
        if (objectsInRange.Count == 0) return null;

        GameObject nearestObj = null;
        float shortestDistance = float.MaxValue;

        foreach (var obj in objectsInRange)
        {
            // sqrMagnitude is faster
            float distanceToObj = (obj.transform.position - transform.position).sqrMagnitude;
            if (distanceToObj < shortestDistance)
            {
                shortestDistance = distanceToObj;
                nearestObj = obj;
            }
        }
        return nearestObj;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Enter: {other.name}");
        objectsInRange.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log($"Exit: {other.name}");

        objectsInRange.Remove(other.gameObject);
    }

    public void CleanUpList()
    {
        objectsInRange.RemoveAll(item => item == null);
    }
}
