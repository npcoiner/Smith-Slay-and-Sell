using System.Collections.Generic;
using UnityEngine;

public class MouldHolder : MonoBehaviour, IInteract
{
    public enum MouldHolderState
    {
        Empty,
        Idle,
        Processing,
        Finished,
    }

    [SerializeField]
    private MouldRecipeDict recipeManager;

    [Header("MouldHolder Status")]
    public MouldHolderState currentState = MouldHolderState.Empty;

    private bool hasMould = false;
    private MouldType heldMould = MouldType.None;
    private OreType heldMetal = OreType.None;

    [Header("Processing Settings")]
    public float processingTime = 3.0f;
    private float currentTimer = 0.0f;

    [Tooltip("Where the finished item should apper.")]
    public Transform spawnPoint;

    void Start() { }

    void Update()
    {
        if (currentState == MouldHolderState.Processing)
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
        if (currentState == MouldHolderState.Empty)
        {
            GameObject parentObject = other.transform.root.gameObject;
            if (!hasMould && parentObject.TryGetComponent(out MouldItem mould))
            {
                heldMould = mould.type;
                hasMould = true;
                currentState = MouldHolderState.Idle;
                Destroy(parentObject);
            }
        }
    }

    public void StartProcessing(OreType metal)
    {
        heldMetal = metal;
        Debug.Log($"MouldHolder started processing: {(heldMetal, heldMould)}");
        if (heldMetal == OreType.None || currentState != MouldHolderState.Idle)
        {
            return;
        }
        else
        {
            currentState = MouldHolderState.Processing;
            currentTimer = 0f;
        }
    }

    private void CompleteProcessing()
    {
        Debug.Log("Furnace finished processing.");
        currentState = MouldHolderState.Finished;
        Vector3 spawnPos =
            spawnPoint != null ? spawnPoint.position : transform.position + Vector3.up;

        GameObject instance = Instantiate(
            recipeManager.GetRecipeForMetalMouldType(heldMetal, heldMould),
            spawnPos,
            Quaternion.identity
        );

        currentState = MouldHolderState.Idle;
        heldMetal = OreType.None;
    }

    public void Interact(GameObject player)
    {
        //TODO give mould to player and turn off hitboxes for items while being held?
        Debug.Log("TODO give mould to player and turn off hitboxes for items while being held?");
    }
}
