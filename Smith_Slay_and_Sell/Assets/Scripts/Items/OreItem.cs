using UnityEngine;

//List of OreItems
public enum OreType
{
    None,
    IronOre,
    CopperOre,
    MagicOre,
}

public class OreItem : MonoBehaviour
{
    public OreType type;
}
