using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public struct Request
{
    public float timeLeft;

    // This is the requested object type
    public FinishedItemSO finishedItemSO;
    public FinishedType finishedType;
    public MetalType metalType;
}

public class RequestService : MonoBehaviour
{
    private readonly int MAX_REQUEST = 6; //6 fit on screen at a time without covering score
    private readonly float DEFAULT_TIME = 120f; //Each request/order is 2 minutes long by default
    private List<Request> requests;

    [SerializeField]
    FinishedSOManager finishedSOManager;
    private FinishedItemSO[] finishedItemSOs;

    void Awake()
    {
        requests = new List<Request>();
        if (!finishedSOManager)
        {
            Debug.LogError("FinishedSOManager not set in this scene!");
        }
        finishedItemSOs = finishedSOManager.finishedItemSOs;
    }

    Request CreateAndGetRequest(float time, FinishedItemSO finishedItemSO)
    {
        Request newRequest = new()
        {
            timeLeft = time,
            finishedItemSO = finishedItemSO,
            finishedType = finishedItemSO.finishedObject.GetComponent<FinishedItem>().type,
            metalType = finishedItemSO.finishedObject.GetComponent<FinishedItem>().metalType,
        };
        return newRequest;
    }

    void AddRequest(Request newRequest)
    {
        if (requests.Count < MAX_REQUEST)
        {
            requests.Add(newRequest);
        }
    }

    //Are we correctly managing memory here or is this depending on C# GC?
    public List<Request> GetRequests()
    {
        //HELLO??
        return requests;
    }

    //TODO pull enums from a manager in the scene so we can ensure only items available to being made can be requested. (complicated recipes should be saved for later levels)
    public void CreateAndAddRandomRequest()
    {
        if (requests.Count < MAX_REQUEST)
        {
            //Old requests that didn't use SOs.
            // //Cool hack for getting array of type enum
            // FinishedType[] finishedTypes = (FinishedType[])Enum.GetValues(typeof(FinishedType));
            // MetalType[] metalTypes = (MetalType[])Enum.GetValues(typeof(MetalType));
            // //Filter out None and Slag using Linq
            // var validMetals = metalTypes
            //     .Where(m => m != MetalType.None && m != MetalType.Slag)
            //     .ToArray();
            // System.Random rand = new();
            // Request newRequest = new()
            // {
            //     finishedType = finishedTypes[rand.Next(finishedTypes.Length)],
            //     metalType = validMetals[rand.Next(validMetals.Length)],
            //     timeLeft = DEFAULT_TIME,
            // };
            System.Random rand = new();
            Request newRequest = CreateAndGetRequest(
                DEFAULT_TIME,
                finishedItemSOs[rand.Next(finishedItemSOs.Length)]
            );
            AddRequest(newRequest);
        }
        else
        {
            Debug.LogWarning("Requests are full");
        }
    }

    //Return type to determine if item should be destroyed/turned in
    public bool SubmitFinishedItem(FinishedItem finishedItem)
    {
        for (int i = 0; i < requests.Count; i++)
        {
            Request request = requests[i];
            if (
                request.finishedType == finishedItem.type
                && request.metalType == finishedItem.metalType
            )
            {
                //TODO add reward processing here?
                requests.RemoveAt(i);
                return true;
            }
        }
        return false;
    }

    public void UpdateTimeLeft()
    {
        for (int i = 0; i < requests.Count; i++)
        {
            Request temp = requests[i];
            if (temp.timeLeft <= 0)
            {
                requests.RemoveAt(i);
            }
            else
            {
                temp.timeLeft -= Time.deltaTime;
                requests[i] = temp;
            }
        }
    }
}
