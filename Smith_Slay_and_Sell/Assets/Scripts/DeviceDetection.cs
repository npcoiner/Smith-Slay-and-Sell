using UnityEngine;
using UnityEngine.InputSystem;

public class DeviceDetection : MonoBehaviour
{
    //This is called by the PlayerInputManager when a player joins.
    private void OnPlayerJoined(PlayerInput playerInput)
    {
        //Log the devices detected for that player.
        string devices = "";
        foreach (var device in playerInput.devices)
        {
            devices += device.displayName;
        }
        Debug.Log($"[Send Message] Player joined with: {devices}");
    }
}
