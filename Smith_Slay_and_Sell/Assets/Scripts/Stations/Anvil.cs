using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//FIXME code duplication between Furnace and Anvil. Mostly everything is the same other than
//interact and tag filtering

public class Anvil : MonoBehaviour, IInteract
{
    public enum AnvilState
    {
        Idle,
        Processing,
        Finished,
    }

    [SerializeField]
    private AnvilRecipeDict recipeManager;

    [Header("Anvil Status")]
    public AnvilState currentState = AnvilState.Idle;

    [Header("Processing Settings")]
    public int processingHits = 10;
    private int currentHits = 0;

    public AudioClip[] anvilSounds;

    private bool hasWorkable = false;
    private WorkableType heldWorkable = WorkableType.None;

    [SerializeField]
    private GameObject barSprite;

    [SerializeField]
    private GameObject hitAnimationSprite;

    [Tooltip("Where the finished item should apper.")]
    public Transform spawnPoint;

    void Update()
    {
        if (currentState == AnvilState.Idle && hasWorkable && heldWorkable != WorkableType.None)
        {
            StartProcessing();
        }
        //Only show bar when active.
        //TODO: make this system dynamic so it works with any prefab
        if (currentState == AnvilState.Idle && barSprite != null && barSprite.activeInHierarchy)
        {
            barSprite.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentState == AnvilState.Idle)
        {
            //Process on the parent object
            GameObject parentObject = other.transform.root.gameObject;
            if (!hasWorkable && parentObject.TryGetComponent(out WorkableItem workable))
            {
                hasWorkable = true;
                heldWorkable = workable.type;
                Destroy(parentObject);
            }
        }
    }

    private void StartProcessing()
    {
        Debug.Log($"Anvil started processing: {heldWorkable}");
        currentState = AnvilState.Processing;

        if (barSprite != null)
        {
            barSprite.SetActive(currentState == AnvilState.Processing);
        }
        currentHits = 0;
    }

    private void CompleteProcessing()
    {
        Debug.Log("Anvil finished processing.");
        currentState = AnvilState.Finished;

        //Spawn item
        Vector3 spawnPos =
            spawnPoint != null ? spawnPoint.position : transform.position + Vector3.up;
        GameObject instance = Instantiate(
            recipeManager.GetRecipeForWorkableType(heldWorkable),
            spawnPos,
            Quaternion.identity
        );

        //Reset back to default state
        hasWorkable = false;
        heldWorkable = WorkableType.None;
        currentHits = 0;
        currentState = AnvilState.Idle;
    }

    public void Interact(GameObject player)
    {
        Debug.Log(currentHits);
        if (currentState == AnvilState.Processing)
        {
            //bonk
            if (hitAnimationSprite.TryGetComponent(out SpriteAnimator component))
            {
                component.PlayOnce();
                TriggerSound();
            }
            currentHits += 1;
            Debug.Log("test");
        }
        if (currentHits >= processingHits)
        {
            CompleteProcessing();
        }
    }

    void TriggerSound()
    {
        SFX.Play(
            anvilSounds[Random.Range(0, anvilSounds.Length)],
            transform.position,
            Random.Range(1f, 1.5f)
        );
    }
}
