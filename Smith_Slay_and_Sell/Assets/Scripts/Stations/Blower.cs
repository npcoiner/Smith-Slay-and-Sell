using System.Collections.Generic;
using UnityEngine;

public class Blower : MonoBehaviour, IInteract
{
    [SerializeField]
    private Crucible connectedCrucible;

    public int maxCoalCapacity = 10;
    public int heatPerPump = 20;

    [Header("Current Status (Read Only)")]
    [SerializeField]
    private int currentCoalCount = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (currentCoalCount >= maxCoalCapacity)
        {
            return; // Blower is full of coal
        }

        GameObject parentObject = other.transform.root.gameObject;
        if (parentObject.TryGetComponent(out CoalItem coalItem))
        {
            currentCoalCount++;
            Destroy(parentObject);
        }
    }

    public void Interact(GameObject player)
    {
        if (connectedCrucible == null)
        {
            Debug.LogError("Blower is missing a reference to the Crucible!");
            return;
        }

        if (currentCoalCount <= 0)
        {
            Debug.Log("Blower interaction failed: Out of coal! Throw some in.");
            return;
        }

        currentCoalCount--;
        connectedCrucible.AddHeat(heatPerPump);
        Debug.Log($"Blower pumped! Added {heatPerPump} heat. Coal remaining: {currentCoalCount}");
    }
}
