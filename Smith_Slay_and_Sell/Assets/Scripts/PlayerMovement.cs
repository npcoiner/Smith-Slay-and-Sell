using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 5.0f;
    [SerializeField] private float deadzone = 0.5f;
    [SerializeField] private float turnSmoothSpeed = 60f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private PlayerInput playerInput;

    private bool isWalking;

    private void Start()
    {
        //This if statement attemps to get a component if it already exists,
        //otherwise it adds it. This saves editor memory and allows easier modularity.
        if (!TryGetComponent<CharacterController>(out controller))
        {
            controller = gameObject.AddComponent<CharacterController>();
        }
        controller.minMoveDistance = 0f;
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
}
