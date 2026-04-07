using System.Collections.Generic;
using UnityEngine;

public class Crucible : MonoBehaviour
{
    public enum CrucibleState
    {
        EmptyCold,
        EmptyHot,
        PartiallyFilledCold,
        PartiallyFilledHot,
        FullCold,
        FullHot,
    }

    //FIFO list of size 20 for metal in crucible
    private const int maxCapacity = 20;
    private int currentMetal = 0;
    private int currentSlag = 0;
    List<OreType> currentMetalList = new List<OreType>(capacity: maxCapacity);

    [SerializeField]
    //TODO
    //private CrucibleRecipeDict recipeManager;

    [Header("Crucible Status")]
    public CrucibleState currentState = CrucibleState.EmptyCold;

    private const int maxTemperature = 100;
    private const int minPourTemp = 15;
    public int temperature = 0;

    [Header("Processing Settings")]
    public float processingTime = 3.0f;

    void Start() { }

    void Update()
    {
        temperature -= (int)(5 * Time.deltaTime);
        bool isHot = temperature > minPourTemp;
        int currentFill = currentMetalList.Count;
        if (currentFill == 0)
        {
            currentState = isHot ? CrucibleState.EmptyHot : CrucibleState.EmptyCold;
        }
        else if (currentFill >= maxCapacity)
        {
            currentState = isHot ? CrucibleState.FullHot : CrucibleState.FullCold;
        }
        else
        {
            currentState = isHot
                ? CrucibleState.PartiallyFilledHot
                : CrucibleState.PartiallyFilledCold;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (currentState != CrucibleState.FullCold && currentState != CrucibleState.FullHot)
        {
            GameObject parentObject = other.transform.root.gameObject;

            if (parentObject.TryGetComponent(out OreItem oreItem))
            {
                AddOreToCrucible(oreItem.type);
                Destroy(parentObject);
            }
            else if (parentObject.TryGetComponent(out CoalItem coalItem))
            {
                AddCoalToCrucible();
                Destroy(parentObject);
            }
            else if (parentObject.TryGetComponent(out WorkableItem workableItem))
            {
                AddMetalToCrucible(workableItem.metalType);
                Destroy(parentObject);
            }
            else if (parentObject.TryGetComponent(out FinishedItem finishedItem))
            {
                AddMetalToCrucible(finishedItem.metalType);
                Destroy(parentObject);
            } //Todo add junk items for cat?
        }
    }

    public void AddHeat(int amountToAdd)
    {
        temperature += amountToAdd;
    }

    public OreType PourMetal()
    {
        OreType pouredMetal = OreType.None;
        if (temperature > minPourTemp && currentMetalList.Count > 0)
        {
            pouredMetal = currentMetalList[currentMetalList.Count - 1];
            currentMetalList.RemoveAt(currentMetalList.Count - 1);
        }
        else
        {
            return OreType.None;
        }
        return pouredMetal;
    }

    private void AddCoalToCrucible()
    {
        //temporaryAddIronOreToCrucible();
        Debug.Log("TODO add steel if coal added to iron layer");
    }

    private void AddMetalToCrucible(OreType itemType)
    {
        //TODO finish MetalItem.cs
        //temporaryAddIronOreToCrucible();
    }

    private void AddOreToCrucible(OreType itemType)
    {
        if (currentMetalList.Count < maxCapacity || currentSlag + currentMetal > maxCapacity)
        {
            currentMetalList.Add(itemType);
            currentMetal += 1;
            currentSlag += 1;
        }
        else
        {
            Debug.Log("Crucible is full");
        }
    }
}
