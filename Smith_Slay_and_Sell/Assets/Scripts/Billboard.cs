using UnityEngine;
// This class is intended to be used with sprites to have them always face the camera.

public class Billboard : MonoBehaviour
{
    [SerializeField] private bool preserveZRotation = true;
//test
    void LateUpdate()
    {
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0; // Keep it flat on the XZ plane

        // Safety for if/when camera pointing straight up/down
        if (camForward.sqrMagnitude > 0.001f)
        {
            Vector3 targetForward = camForward.normalized;

            if (preserveZRotation)
            {
                float currentRoll = transform.eulerAngles.z;
                Quaternion targetLook = Quaternion.LookRotation(targetForward);
                transform.rotation = targetLook * Quaternion.Euler(0f, 0f, currentRoll);
            }
            else
            {
                transform.forward = targetForward;
            }
        }
    }
}
