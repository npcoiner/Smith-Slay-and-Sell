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

    //This function returns the nearest object with a trigger collider to the interact sphere
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
    public GameObject GetNearestFiltered(string filterTag)
    {
        objectsInRange.RemoveAll(item => item == null);
        if (objectsInRange.Count == 0) return null;

        GameObject nearestObjFiltered = null;
        float shortestDistance = float.MaxValue;
        foreach (var obj in objectsInRange)
        {

            if (obj.CompareTag(filterTag))
            {

                float distanceToObj = (obj.transform.position - transform.position).sqrMagnitude;
                if (distanceToObj < shortestDistance)
                {
                    shortestDistance = distanceToObj;
                    Debug.Log(obj.tag);
                    nearestObjFiltered = obj;
                }
            }
        }
        return nearestObjFiltered;
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
