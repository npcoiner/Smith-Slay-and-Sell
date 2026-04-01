using UnityEngine;

//FIXME code duplication between Furnace and Anvil. Mostly everything is the same other than
//interact and tag filtering

public class Anvil : MonoBehaviour, IInteract
{
    public enum AnvilState
    {
        Idle,
        Processing,
        Finished
    }
    [Header("Anvil Status")]
    public AnvilState currentState = AnvilState.Idle;

    [Header("Processing Settings")]
    public int processingHits = 10;
    private int currentHits = 0;


    //Will need to switch from a tag system eventually since only
    //one tag can be set at a time in Unity, but we might have multiple processing
    //types that should only work on some entities
    [Header("Item Settings")]
    [Tooltip("The tag of the item this Anvil accepts.")]
    public string validItemTag = "Forgeable";
    [Tooltip("The prefab to spawn after processing completes")]
    public GameObject outputPrefab;
    [Tooltip("Where the finished item should apper.")]
    public Transform spawnPoint;


    private GameObject itemBeingProcessed;

    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentState == AnvilState.Idle && other.CompareTag(validItemTag))
        {
            //Process on the root object
            StartProcessing(other.transform.root.gameObject);
        }
    }
    private void StartProcessing(GameObject inputItem)
    {
        Debug.Log($"Anvil started processing: {inputItem.name}");
        currentState = AnvilState.Processing;

        itemBeingProcessed = inputItem;
        itemBeingProcessed.SetActive(false);

    }

    private void CompleteProcessing()
    {
        Debug.Log("Anvil finished processing.");
        currentHits = 0;
        currentState = AnvilState.Finished;

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
            Debug.LogWarning("No output prefab assigned to Anvil!");
        }
        currentState = AnvilState.Idle;
    }
    public void Interact(GameObject player)
    {
        Debug.Log(currentHits);
        if (currentState == AnvilState.Processing)
        {
            //bonk
            currentHits += 1;
            Debug.Log("test");

        }
        if (currentHits >= processingHits)
        {
            CompleteProcessing();
        }

    }
}
