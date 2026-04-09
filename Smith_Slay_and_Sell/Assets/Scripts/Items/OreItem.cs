using UnityEngine;

//List of OreItems
public enum OreType
{
    None,
    Slag,
    IronOre,
    CopperOre,
    MagicOre,
}

public class OreItem : MonoBehaviour
{
    public OreType type;
    public MetalType processedMetalType;
}
