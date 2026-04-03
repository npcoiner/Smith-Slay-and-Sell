using UnityEngine;
using System.Collections.Generic;


// TODO filter for pickupable/interactables probably using tags.
public class InteractSphere : MonoBehaviour
{
    private string INTERACT_TAG = "Interactable";
    private List<GameObject> objectsInRange = new List<GameObject>();
    private GameObject selected = null;
    private GameObject lastSelected = null;

    void Update()
    {
        selected = GetNearestFiltered(INTERACT_TAG);
        if (selected == null)
        {
            if (lastSelected != null)
            {
                RemoveHighlight(lastSelected);

            }
        }
        else
        {
            if (selected != lastSelected)
            {
                if (lastSelected)
                {
                    RemoveHighlight(lastSelected);
                }

                ApplyHighlight(selected);
            }
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

            if (obj.CompareTag(filterTag) && obj.activeInHierarchy)
            {

                float distanceToObj = (obj.transform.position - transform.position).sqrMagnitude;
                if (distanceToObj < shortestDistance)
                {
                    shortestDistance = distanceToObj;
                    //Debug.Log(obj.tag);
                    nearestObjFiltered = obj;
                }
            }
        }
        return nearestObjFiltered;
    }
    void OnTriggerEnter(Collider other)
    {
        //   Debug.Log($"Enter: {other.name}");
        objectsInRange.Add(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        //    Debug.Log($"Exit: {other.name}");

        objectsInRange.Remove(other.gameObject);
    }
    void RemoveHighlight(GameObject obj)
    {
        Renderer rend = obj.transform.root.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            rend.material.SetColor("_EmissionColor", Color.black);

            rend.material.DisableKeyword("_EMISSION");
        }
        else
        {
            Debug.LogWarning("Tried to remove highlight something with no Renderer!");
        }

        lastSelected = null;
    }

    void ApplyHighlight(GameObject obj)
    {
        Debug.Log("Highlighting");
        Renderer rend = obj.transform.root.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            // 1. You have to tell URP it's allowed to glow first
            rend.material.EnableKeyword("_EMISSION");

            // 2. Set the actual built-in emission color property to bright yellow
            rend.material.SetColor("_EmissionColor", Color.white * 0.1f);
        }
        else
        {
            Debug.LogWarning("Tried to highlight something with no Renderer!");
        }

        lastSelected = obj;
    }
    public void CleanUpList()
    {
        objectsInRange.RemoveAll(item => item == null);
    }
}
