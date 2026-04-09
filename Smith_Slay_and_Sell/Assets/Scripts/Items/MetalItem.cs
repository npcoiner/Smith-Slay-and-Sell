using UnityEngine;

//List of OreItems
public enum MetalType
{
    None,
    Slag,
    Iron,
    Copper,
    Magic,
}

public class MetalItem : MonoBehaviour
{
    public MetalType metalType;
    public OreType oreType; //TODO, crucible should work with MetalType not OreType to handle things like Steel
}
