using System.Collections.Generic;
using UnityEngine;

// TODO filter for pickupable/interactables probably using tags.
public class InteractSphere : MonoBehaviour
{
    private string INTERACT_TAG = "Interactable";
    private List<GameObject> objectsInRange = new List<GameObject>();
    public GameObject selected = null;
    public GameObject lastSelected = null;
    private bool shouldHighlight = false;

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
                if (shouldHighlight)
                {
                    ApplyHighlight(selected);
                }
            }
        }
    }

    public void startHighlighting()
    {
        if (shouldHighlight != true)
            shouldHighlight = true;
    }

    public void stopHighlighting()
    {
        if (shouldHighlight == true)
            shouldHighlight = false;
    }

    //This function returns the nearest object with a trigger collider to the interact sphere
    public GameObject GetNearestInRange()
    {
        //Remove any destroyed or missing objects for whatever reason.
        CleanUpList();
        if (objectsInRange.Count == 0)
            return null;

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
        CleanUpList();
        if (objectsInRange.Count == 0)
            return null;

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

    //Holding the state on the InteractSphere is not correct and can lead to bugs
    //where the highlight might stay on if the InteractSphere ever get's lost
    //or a race condition occurs. Holding state on the highlighted object and simply
    //updating that state is much more robust, but might require more communication through
    //interfaces or events, which can be less performant. For a small project I have opted for
    //the potentially less robust option for now.
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
        //Debug.Log("Highlighting");
        Renderer rend = obj.transform.root.GetComponentInChildren<Renderer>();
        if (rend != null)
        {
            rend.material.EnableKeyword("_EMISSION");
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
        objectsInRange.RemoveAll(item => !item.activeSelf); // Important for things like Mould that become inactive when places into the mould holder
    }
}
