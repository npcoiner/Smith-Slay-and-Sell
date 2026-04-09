using UnityEngine;

//List of FinishedItems
public enum FinishedType
{
    FinishedSword,
    FinishedHammer,
}

public class FinishedItem : MonoBehaviour
{
    public FinishedType type;
    public MetalType metalType;
}
