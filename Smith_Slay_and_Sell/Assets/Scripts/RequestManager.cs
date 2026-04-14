using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RequestManager : MonoBehaviour
{
    public event EventHandler OnRequestSpawned;
    public event EventHandler OnRequestCompleted;

    [SerializeField]
    private RequestService requestService;

    [SerializeField]
    private float spawnInterval = 30f;

    [SerializeField]
    private bool debuglog = false;

    private float spawnTimer = 0f;

    void Update()
    {
        requestService.UpdateTimeLeft();
        spawnTimer += Time.deltaTime;
        if (
            ShouldSpawn()
            && GameStateManager.Instance.CurrentState == GameStateManager.GameState.Active
        )
        {
            SpawnRequest();
            spawnTimer = 0f;
            OnRequestSpawned?.Invoke(this, EventArgs.Empty);
            if (debuglog)
            {
                Debug.Log("Attempting to spawn a new request.");

                Debug.Log("Current List of Requests: ");
                foreach (var x in requestService.GetRequests())
                {
                    Debug.Log(x.finishedType);
                    Debug.Log(x.metalType);
                    Debug.Log(x.timeLeft);
                }
            }
        }
    }

    public bool SubmitFinishedItem(FinishedItem finishedItem)
    {
        if (debuglog)
        {
            Debug.Log("Attempting to submit finished item.");
        }
        if (requestService.SubmitFinishedItem(finishedItem))
        {
            OnRequestCompleted?.Invoke(this, EventArgs.Empty);
            return true;
        }
        else
        {
            return false;
        }
    }

    public List<Request> GetRequests()
    {
        return requestService.GetRequests();
    }

    private void SpawnRequest()
    {
        //Have RandomRequest from a scene manager instead of enum
        requestService.CreateAndAddRandomRequest();
    }

    private bool ShouldSpawn()
    {
        return spawnTimer >= spawnInterval;
    }
}
