using UnityEngine;

public class SlagRemover : MonoBehaviour, IInteract
{
    [SerializeField]
    private Crucible crucible;

    public void Interact(GameObject player)
    {
        crucible.RemoveSlag();
    }
}
