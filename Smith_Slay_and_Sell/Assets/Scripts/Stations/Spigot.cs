using UnityEngine;

public class Spigot : MonoBehaviour, IInteract
{
    [SerializeField]
    private Crucible connectedCrucible;

    [SerializeField]
    private MouldHolder connectedMouldHolder;

    public void Interact(GameObject player)
    {
        Debug.Log("test");
        if (connectedCrucible == null || connectedMouldHolder == null)
        {
            Debug.LogError("Spigot is missing a reference to the Crucible or MouldHolder!");
            return;
        }

        if (connectedMouldHolder.currentState != MouldHolder.MouldHolderState.Idle)
        {
            Debug.Log(
                "Spigot interaction failed: MouldHolder is not Idle (Missing mould or already processing)."
            );
            return;
        }
        OreType pouredMetal = connectedCrucible.PourMetal();
        if (pouredMetal == OreType.None)
        {
            Debug.Log("Spigot interaction failed: Crucible is empty or too cold to pour.");
            return;
        }
        Debug.Log($"Spigot successfully poured {pouredMetal} into the MouldHolder.");
        connectedMouldHolder.StartProcessing(pouredMetal);
    }
}
