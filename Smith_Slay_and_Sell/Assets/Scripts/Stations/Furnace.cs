using UnityEngine;

public class Furnace : MonoBehaviour, IInteract
{
    public enum FurnaceState
    {
        Idle,
        Processing,
        Finished
    }

    [SerializeField] private GameObject fireSprite;
    [Header("Furnace Status")]
    public FurnaceState currentState = FurnaceState.Idle;

    [Header("Processing Settings")]
    public float processingTime = 3.0f;

    private float currentTimer = 0f;


    //Will need to switch from a tag system eventually since only
    //one tag can be set at a time in Unity, but we might have multiple processing
    //types that should only work on some entities
    [Header("Item Settings")]
    [Tooltip("The tag of the item this Furnace accepts.")]
    public string validItemTag = "Processable";
    [Tooltip("The prefab to spawn after processing completes")]
    public GameObject outputPrefab;
    [Tooltip("Where the finished item should apper.")]
    public Transform spawnPoint;


    private GameObject itemBeingProcessed;

    void Update()
    {
        if (fireSprite != null)
        {
            fireSprite.SetActive(currentState == FurnaceState.Processing);
        }
        if (currentState == FurnaceState.Processing)
        {
            currentTimer += Time.deltaTime;
            if (currentTimer >= processingTime)
            {
                CompleteProcessing();
                currentTimer = 0f;
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //TODO move off the item tag system
        if (currentState == FurnaceState.Idle && other.CompareTag(validItemTag))
        {

            StartProcessing(other.transform.root.gameObject);
        }
    }

    private void StartProcessing(GameObject inputItem)
    {
        Debug.Log($"Furnace started processing: {inputItem.name}");
        currentState = FurnaceState.Processing;
        currentTimer = 0f;

        itemBeingProcessed = inputItem;
        itemBeingProcessed.SetActive(false);
    }

    private void CompleteProcessing()
    {
        Debug.Log("Furnace finished processing.");
        currentState = FurnaceState.Finished;

        if (itemBeingProcessed != null)
        {
            Destroy(itemBeingProcessed);
        }
        if (outputPrefab != null)
        {
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position + Vector3.up;
            GameObject spawnedObject = Instantiate(outputPrefab, spawnPos, Quaternion.identity);
            Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                float upForce = 5f;
                float sideRange = 2f;

                Vector3 force = new Vector3(
                    Random.Range(-sideRange, sideRange),
                    upForce,
                    Random.Range(-sideRange, sideRange)
                );

                rb.AddForce(force, ForceMode.Impulse);
            }
        }
        else
        {
            Debug.LogWarning("No output prefab assigned to Furnace!");
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
