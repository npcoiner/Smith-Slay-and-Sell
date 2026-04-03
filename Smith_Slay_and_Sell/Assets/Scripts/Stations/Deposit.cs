using UnityEngine;

public class Deposit : MonoBehaviour, IInteract
{
    public enum FurnaceState
    {
        Idle,
        Processing,
        Finished
    }

    [SerializeField] private GameObject fireSprite;
    [SerializeField] private ScoreHandler scoreHandler;

    [Header("Furnace Status")]
    public FurnaceState currentState = FurnaceState.Idle;

    private float currentTimer = 0f;
    private GameObject itemBeingProcessed;


    //Will need to switch from a tag system eventually since only
    //one tag can be set at a time in Unity, but we might have multiple processing
    //types that should only work on some entities
    
    //TODO use layer instead of tags
    [Header("Item Settings")]
    [Tooltip("The layer of the item this Furnace accepts.")]
    public string validItemLayer = "processed";


    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO move off the item tag system
        StartProcessing(other.transform.root.gameObject);
    }

    private void StartProcessing(GameObject inputItem)
    {
        Debug.Log($"Deposit box started processing: {inputItem.name}");
        currentState = FurnaceState.Processing;

        itemBeingProcessed = inputItem;
        itemBeingProcessed.SetActive(false);

	CompleteProcessing();
    }

    private void CompleteProcessing()
    {
        Debug.Log("Deposit box finished processing.");
        // currentState = FurnaceState.Finished;
	
	// update score
	scoreHandler.UpdateScore(itemBeingProcessed);	

        if (itemBeingProcessed != null)
        {
            Destroy(itemBeingProcessed);
        }

	
        
        currentState = FurnaceState.Idle;
    }

    //Public interact for IInteract interface
    //Handles the interact functionality
    public void Interact(GameObject player)
    {
        //TODO idk do something here maybe
        Debug.Log(player);
    }
}
