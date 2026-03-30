using UnityEngine;
using UnityEngine.InputSystem;

//This class is simply for debugging purposes. To control which input devices
//are allowed, use the Input System in the editor. Many sensors on Laptops and 
//other devices get mistaken for input devices when really they are simply
//accelerometers for detecting tilt, etc.
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
