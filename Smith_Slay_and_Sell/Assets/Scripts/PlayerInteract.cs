using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    private string INTERACT_TAG = "Interactable";
    private CharacterController controller;

    [Header("Debug Options")]
    public bool showInteractSphere = false;

    private Transform interactSphereTransform;
    private InteractSphere interactSphereScript;

    //Public for interact interface
    public Transform heldObject;
    public Rigidbody heldRb;

    [Header("Magnet Settings")]
    public float suctionStrength = 250f;
    public float dampening = 15f;
    public float holdDistance = 1.5f;
    public float holdHeight = 1.0f;

    private void Awake()
    {
        //This code attempts to get a component on the attatched game object and otherwise creates it.
        //This ensures that the required component is available without having to add it manually.
        if (!TryGetComponent<CharacterController>(out controller))
            controller = gameObject.AddComponent<CharacterController>();

        //This code searches for the InteractSphere, and if it doesn't exist, it creates a new one.
        //This is very unnecessary to be completely honest, but it's a good way to showcase how
        //to use scripting to create components and attatch them all together.
        interactSphereTransform = transform.Find("InteractSphere");
        if (!interactSphereTransform)
        {
            var interactSphereObj = new GameObject("InteractSphere");
            interactSphereObj.AddComponent<InteractSphere>();
            interactSphereObj.transform.SetParent(transform);
            interactSphereObj.transform.localPosition = new Vector3(0, 0.5f, 1);
            SphereCollider col = interactSphereObj.AddComponent<SphereCollider>();
            col.isTrigger = true;
            col.radius = 1f;
            interactSphereTransform = interactSphereObj.transform;
            interactSphereScript = interactSphereObj.GetComponent<InteractSphere>();
            var meshFilter = interactSphereObj.GetComponent<MeshFilter>();
            var meshRenderer = interactSphereObj.GetComponent<MeshRenderer>();
            meshFilter = interactSphereObj.AddComponent<MeshFilter>();
            meshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
            meshRenderer = interactSphereObj.AddComponent<MeshRenderer>();
            meshRenderer.material.color = Color.green;
            meshRenderer.enabled = showInteractSphere;
        }
        //Set callback functions
        var interactAction = GetComponent<PlayerInput>().actions["Interact"];
        interactAction.started += OnInteractStarted;
        interactAction.performed += OnInteractPerformed;
        interactAction.canceled += OnInteractCanceled;
    }

    //OnValidate runs on both awake and on editor change to show the interactSphere for debugging purposes.
    private void OnValidate()
    {
        if (!interactSphereTransform)
            return;
        var meshRenderer = interactSphereTransform.GetComponent<MeshRenderer>();
        if (meshRenderer)
        {
            meshRenderer.enabled = showInteractSphere;
        }
    }

    private void OnInteractStarted(InputAction.CallbackContext ctx)
    {
        interactSphereScript.CleanUpList();
    }

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        //Some things don't destroy immediately in order to determine logic.
        //Better to cover all cases than follow one strict paradigm. All objects
        //that are set inactive should get deleted eventually though or we risk memory leaks.
        if (heldRb != null && heldRb.gameObject.activeInHierarchy)
        {
            DropObject();
            return;
        }

        //Sanity check
        heldRb = null;
        heldObject = null;

        //The following block of code gets the nearest interactable that collided with the interact sphere
        //it then gets all of the MonoBehaviours in order to extract a possible interface, and then
        //uses the provided interface to interact with the object. We pass the player so the object can
        //attach itself or perform any needed behaviors. On my machine, the namespace doens't recognize
        //the interface
        var objectInRange = interactSphereScript.GetNearestFiltered(INTERACT_TAG);
        //Debug.Log(objectInRange.name);

        if (objectInRange != null)
        {
            var tempMonoArray = objectInRange.GetComponents<MonoBehaviour>();
            foreach (var monoBehavior in tempMonoArray)
            {
                var temp = monoBehavior as IInteract;
                if (temp != null)
                {
                    temp.Interact(this.gameObject);
                }
            }
        }
    }

    private void OnInteractCanceled(InputAction.CallbackContext ctx) { }

    private void DropObject()
    {
        if (heldRb != null)
        {
            //Reenable collisions
            heldRb.detectCollisions = true;
            heldRb.useGravity = true;
            heldRb.angularDamping = 0.05f; //default unity value
            //clamp and give a little push in the forward direction
            heldRb.linearVelocity =
                Vector3.ClampMagnitude(heldRb.linearVelocity, 10f) + transform.forward * 3.0f;
            heldRb.angularVelocity = Vector3.zero;
            heldRb = null;
        }
        heldObject = null;
    }

    void FixedUpdate()
    {
        if (heldRb != null)
        {
            //Disable collisions while holding object
            heldRb.detectCollisions = false;
            //Magnet hands code
            Vector3 targetPos =
                transform.position + (transform.forward * holdDistance) + (Vector3.up * holdHeight);
            Vector3 direction = targetPos - heldRb.position;
            float distance = direction.magnitude;

            float forceMultiplier = Mathf.Clamp(distance * distance, 0.1f, 10f);
            Vector3 suctionForce = direction.normalized * (suctionStrength * forceMultiplier);
            Vector3 dragForce = heldRb.linearVelocity * dampening;

            if (distance < 0.05f)
            {
                heldRb.linearVelocity = Vector3.Lerp(
                    heldRb.linearVelocity,
                    Vector3.zero,
                    Time.fixedDeltaTime
                );
            }

            heldRb.AddForce(suctionForce - dragForce, ForceMode.Acceleration);
        }
    }
}
