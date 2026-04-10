using UnityEngine;

public class Deposit : MonoBehaviour
{
    public enum FurnaceState
    {
        Idle,
        Processing,
        Finished,
    }

    [SerializeField]
    private ScoreHandler scoreHandler;
    private GameObject itemBeingProcessed;

    //TODO implement layer
    [Header("Item Settings")]
    [Tooltip("The layer of the item this Furnace accepts.")]
    public string validItemLayer = "processed";

    void Update() { }

    private void OnTriggerEnter(Collider other)
    {
        GameObject parentObject = other.transform.root.gameObject;
        if (parentObject.TryGetComponent(out FinishedItem finished))
        {
            StartProcessing(parentObject);
        }
    }

    private void StartProcessing(GameObject inputItem)
    {
        Debug.Log($"Deposit box started processing: {inputItem.name}");
        itemBeingProcessed = inputItem;

        CompleteProcessing();
    }

    private void CompleteProcessing()
    {
        Debug.Log("Deposit box finished processing.");
        // update score
        scoreHandler.UpdateScore(itemBeingProcessed);

        if (itemBeingProcessed != null)
        {
            Destroy(itemBeingProcessed);
        }
    }

    //Public interact for IInteract interface
    //Handles the interact functionality
    public void Interact(GameObject player)
    {
        //TODO idk do something here maybe
        Debug.Log(player);
    }
}
