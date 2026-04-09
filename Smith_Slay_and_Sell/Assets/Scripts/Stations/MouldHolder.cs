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

    [SerializeField]
    private MouldSprite mouldSpriteManager;

    [SerializeField]
    private SpriteRenderer mouldSprite;

    public SFXManager sfxManager;

    [Header("MouldHolder Status")]
    public MouldHolderState currentState = MouldHolderState.Empty;
    private GameObject heldMouldObj = null;
    private bool hasMould = false;
    private MouldType heldMould = MouldType.None;
    private MetalType heldMetal = MetalType.None;

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
                heldMouldObj = parentObject;
                UpdateMouldSprite();
                currentState = MouldHolderState.Idle;
                parentObject.SetActive(false);
                sfxManager.PopSound(transform.position);
            }
        }
    }

    public void StartProcessing(MetalType metal)
    {
        heldMetal = metal;
        Debug.Log($"MouldHolder started processing: {(heldMetal, heldMould)}");
        if (heldMetal == MetalType.None || currentState != MouldHolderState.Idle)
        {
            return;
        }
        else
        {
            currentState = MouldHolderState.Processing;
            currentTimer = 0f;
        }
    }

    private void UpdateMouldSprite()
    {
        mouldSprite.sprite = mouldSpriteManager.GetSpriteForMouldType(heldMould);
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
        sfxManager.PopSound(transform.position);

        currentState = MouldHolderState.Idle;
        heldMetal = MetalType.None;
    }

    public void Interact(GameObject player)
    {
        //TODO give mould to player and turn off hitboxes for items while being held?
        Debug.Log("TODO give mould to player and turn off hitboxes for items while being held?");
        if (currentState == MouldHolderState.Idle)
        {
            if (heldMouldObj == null)
            {
                Debug.LogError("Tried to remove non-existant mould!");
            }
            //Disable collisions when spawning so it doesn't immediately get absorbed by the MouldHolder
            if (heldMouldObj.TryGetComponent<Rigidbody>(out Rigidbody tempRb))
            {
                if (tempRb != null)
                {
                    tempRb.detectCollisions = false;
                }
            }
            //Set player to hold the re-enabled Mould
            PlayerInteract playerInteractScript = player.GetComponent<PlayerInteract>();
            if (playerInteractScript != null && playerInteractScript.heldRb == null)
            {
                playerInteractScript.heldRb = tempRb;
                playerInteractScript.heldObject = heldMouldObj.transform;
            }
            else
            {
                //TODO make this make more sense
                Debug.LogError("Unreachable code reached! (VERY BAD)");
            }

            heldMouldObj.SetActive(true);
            currentState = MouldHolderState.Empty;
            hasMould = false;
            heldMould = MouldType.None;
            heldMouldObj = null;
            UpdateMouldSprite();
        }
    }
}
