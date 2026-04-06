using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour, IInteract
{
    public enum FurnaceState
    {
        Idle,
        Processing,
        Finished,
    }

    [SerializeField]
    private FurnaceRecipeDict recipeManager;

    [SerializeField]
    private GameObject fireSprite;

    [Header("Furnace Status")]
    public FurnaceState currentState = FurnaceState.Idle;

    [Header("Processing Settings")]
    public float processingTime = 3.0f;

    private float currentTimer = 0f;
    private bool hasFuel = false;
    private bool hasSmeltable = false;

    private OreType heldSmeltable = OreType.None;

    [Tooltip("Where the finished item should apper.")]
    public Transform spawnPoint;

    private GameObject itemBeingProcessed;

    void Start() { }

    void Update()
    {
        if (
            currentState == FurnaceState.Idle
            && hasFuel
            && hasSmeltable
            && heldSmeltable != OreType.None
        )
        {
            StartProcessing();
        }
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
        if (currentState == FurnaceState.Idle)
        {
            GameObject parentObject = other.transform.root.gameObject;
            if (!hasSmeltable && parentObject.TryGetComponent(out OreItem smeltable))
            {
                heldSmeltable = smeltable.type;
                hasSmeltable = true;
                Destroy(parentObject);
            }
            else if (!hasFuel && parentObject.TryGetComponent(out CoalItem coalItem))
            {
                hasFuel = true;
                Destroy(parentObject);
            }
        }
    }

    private void StartProcessing()
    {
        Debug.Log($"Furnace started processing: {heldSmeltable}");
        currentState = FurnaceState.Processing;
        currentTimer = 0f;
    }

    private void CompleteProcessing()
    {
        Debug.Log("Furnace finished processing.");
        currentState = FurnaceState.Finished;
        Vector3 spawnPos =
            spawnPoint != null ? spawnPoint.position : transform.position + Vector3.up;

        //Using FurnaceRecipeDict to find the corresponding prefab give the smeltable and spawn it
        GameObject instance = Instantiate(
            recipeManager.GetRecipeForOreType(heldSmeltable),
            spawnPos,
            Quaternion.identity
        );

        //Set back to default state
        hasFuel = false;
        hasSmeltable = false;
        heldSmeltable = OreType.None;
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
