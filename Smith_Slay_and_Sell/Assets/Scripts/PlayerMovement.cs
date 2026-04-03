using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float deadzone = 1f;
    [SerializeField] private float turnSmoothSpeed = 1f;
    [SerializeField] private float pushPower = 2.0f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private PlayerInput playerInput;

    private bool isWalking;

    private float playerCenter = 0.5f;
    private void Start()
    {
        //This if statement attemps to get a component if it already exists,
        //otherwise it adds it. This saves editor memory and allows easier modularity.
        if (!TryGetComponent<CharacterController>(out controller))
        {
            controller = gameObject.AddComponent<CharacterController>();
        }

        controller.center = new Vector3(0f, playerCenter, 0f);
        controller.radius = 0.25f;
        controller.minMoveDistance = 0f;
        controller.skinWidth = 0.1f;
        playerInput = GetComponent<PlayerInput>();

        //Debug.Log(controller);
    }

    void Update()
    {
        //Read the move input from the new input system
        Vector2 input = playerInput.actions["Move"].ReadValue<Vector2>();

        //Ensure the value has a magnitude greater than the deadzone
        if (input.magnitude < deadzone)
        {

            isWalking = false;
            return;
        }

        //Create a Vector3 from the Vector2 input
        Vector3 move = new Vector3(input.x, 0, input.y);

        move = move.normalized;

        //Should never happen unless deadzone is set to 0 for keyboard users
        if (move != Vector3.zero)
        {
            isWalking = true;
            //Rotate player forward to slerp into moving direction
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSmoothSpeed * Time.deltaTime);
        }

        //Apply movement speed modifier
        Vector3 finalMove = (move * playerSpeed);

        controller.Move(finalMove * Time.deltaTime);
    }
    public bool IsWalking()
    {
        return isWalking;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        if (body == null || body.isKinematic)
        {
            return;
        }

        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0.1f, hit.moveDirection.z);
        body.linearVelocity = pushDir * pushPower;
        //        body.AddForce(pushDir * pushPower, ForceMode.Impulse);
    }
    void LateUpdate()
    {
        Vector3 lockedPosition = transform.position;
        //
        lockedPosition.y = playerCenter;

        transform.position = lockedPosition;
    }
}
