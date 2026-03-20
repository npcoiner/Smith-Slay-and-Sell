using UnityEngine;
// This class is intended to be used with sprites to have them always face the camera.

public class Billboard : MonoBehaviour
{
    void LateUpdate()
    {
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0;
        //Safety for if/when camera pointing straight up/down
        if (camForward.sqrMagnitude > 0.001f)
        {
            transform.forward = -camForward.normalized;
        }

    }
}
