using UnityEngine;

//List of types of mould
public enum MouldType
{
    None,
    SwordMould,
    BlankMould,
    DaggerMould,
}

public class MouldItem : MonoBehaviour
{
    public MouldType type;
}
