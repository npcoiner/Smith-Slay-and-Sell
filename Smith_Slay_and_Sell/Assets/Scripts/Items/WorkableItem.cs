using UnityEngine;

//List of WorkableItems
//These are items that will be refined
//at the anvil to make a finished item
public enum WorkableType
{
    None,
    WorkableSword,
}

public class WorkableItem : MonoBehaviour
{
    public WorkableType type;
    public OreType metalType;
}
