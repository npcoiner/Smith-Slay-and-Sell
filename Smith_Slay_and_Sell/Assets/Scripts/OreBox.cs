using System.Collections.Generic;
using System.Collections;
using UnityEngine;

//FIXME code duplication between Furnace and Anvil. Mostly everything is the same other than
//interact and tag filtering

public class OreBox : MonoBehaviour, IInteract{

    public AudioClip[] oreBoxSounds;
    public GameObject oreObj;
    public
        void Update()
    { }
    public void Interact(GameObject player)
    {
        TriggerSound();
        SpawnObjectTryHold(player);
    }
    void SpawnObjectTryHold(GameObject player)
    {


        GameObject spawnedObject = Instantiate(oreObj, transform.position, Quaternion.identity);
        Rigidbody rb = spawnedObject.GetComponent<Rigidbody>();

        //Launch object. Should be cancelled out by the holding, but supports hands being
        //full while interawct in future
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

        PlayerInteract playerInteractScript = player.GetComponent<PlayerInteract>();
        if (playerInteractScript != null && playerInteractScript.heldRb == null)
        {
            playerInteractScript.heldRb = spawnedObject.GetComponent<Rigidbody>();
            playerInteractScript.heldObject = spawnedObject.transform;//TODO don't name transform as object; jebaited
        }
        else
        {
            //TODO make this make more sense
            Debug.Log("Unreachable code reached");
        }
    }
    void TriggerSound()
    {
        Debug.Log("Sound");

        if (oreBoxSounds != null && oreBoxSounds.Length > 0)
        {
            SFX.Play(oreBoxSounds[Random.Range(0, oreBoxSounds.Length)], transform.position, Random.Range(1f, 1.5f));
        }
    }


}
