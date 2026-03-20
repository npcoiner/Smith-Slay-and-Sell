using UnityEngine;

public class Station : MonoBehaviour
{
    public enum StationState
    {
        Idle,
        Processing,
        Finished
    }

    [Header("Station Status")]
    public StationState currentState = StationState.Idle;

    [Header("Processing Settings")]
    public float processingTime = 3.0f;
    private float currentTimer = 0f;

    [Header("Item Settings")]
    [Tooltip("The tag of the item this station accepts.")]
    public string validItemTag = "Processable";
    [Tooltip("The prefab to spawn after processing completes")]
    public GameObject outputPrefab;
    [Tooltip("Where the finished item should apper.")]
    public Transform spawnPoint;


    private GameObject itemBeingProcessed;

    void Update()
    {
        if (currentState == StationState.Processing)
        {
            currentTimer += Time.deltaTime;
        }
        if (currentTimer >= processingTime)
        {
            CompleteProcessing();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentState == StationState.Idle && other.CompareTag(validItemTag))
        {
            StartProcessing(other.gameObject);
        }
    }
    private void StartProcessing(GameObject inputItem)
    {
        Debug.Log($"Station started processing: {inputItem.name}");
        currentState = StationState.Processing;
        currentTimer = 0f;

        itemBeingProcessed = inputItem;
        itemBeingProcessed.SetActive(false);
    }

    private void CompleteProcessing()
    {
        Debug.Log("Station finished processing.");
        currentState = StationState.Finished;

        if (itemBeingProcessed != null)
        {
            Destroy(itemBeingProcessed);
        }
        if (outputPrefab != null)
        {
            Vector3 spawnPos = spawnPoint != null ? spawnPoint.position : transform.position + Vector3.up;
            Instantiate(outputPrefab, spawnPos, Quaternion.identity);
        }
        else
        {
            Debug.LogWarning("No output prefab assigned to station! Did you forget to add an output prefab?");
        }
        currentState = StationState.Idle;
    }
}
