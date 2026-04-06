using UnityEngine;

//List of FinishedItems
public enum FinishedType
{
    FinishedSword,
}

public class FinishedItem : MonoBehaviour
{
    public FinishedType type;
    public OreType metalType;
}
