using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private string PICKUP_TAG = "Pickupable";
    private CharacterController controller;
    [Header("Debug Options")]
    public bool showInteractSphere = false;

    private Transform interactSphereTransform;
    private InteractSphere interactSphereScript;

    private Transform heldObject;
    private Rigidbody heldRb;

    [Header("Magnet Settings")]
    public float suctionStrength = 250f;
    public float dampening = 15f;
    public float holdDistance = 1.5f;
    public float holdHeight = 0.5f;

    private void Awake()
    {
        //This code attempts to get a component on the attatched game object and otherwise creates it.
        //This ensures that the required component is available without having to add it manually.
        if (!TryGetComponent<CharacterController>(out controller))
            controller = gameObject.AddComponent<CharacterController>();

        //This code searches for the InteractSphere, and if it doesn't exist, it creates a new one.
        //This is slightly unnecessary to be completely honest, but it's a good way to showcase how
        //to use scripting to create components and attatch them all together.
        interactSphereTransform = transform.Find("InteractSphere");
        if (!interactSphereTransform)
        {
            var interactSphereObj = new GameObject("InteractSphere");
            interactSphereObj.AddComponent<InteractSphere>();
            interactSphereObj.transform.SetParent(transform);
            interactSphereObj.transform.localPosition = new Vector3(0, 0, 1);
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
        if (!interactSphereTransform) return;
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
        if (heldRb != null)
        {
            DropObject();
            return;
        }

        var objectInRange = interactSphereScript.GetNearestFiltered(PICKUP_TAG);
        if (objectInRange)
        {
            heldObject = objectInRange.transform.root;
            heldRb = heldObject.GetComponent<Rigidbody>();

            if (heldRb != null)
            {
                heldRb.useGravity = false;
                heldRb.angularDamping = 10f;
                Physics.IgnoreCollision(GetComponent<Collider>(), objectInRange.GetComponent<Collider>(), true);
            }
        }
    }

    private void OnInteractCanceled(InputAction.CallbackContext ctx)
    {
    }

    private void DropObject()
    {
        if (heldRb != null)
        {
            var col = heldRb.GetComponentInChildren<Collider>();
            if (col != null) Physics.IgnoreCollision(GetComponent<Collider>(), col, false);

            heldRb.useGravity = true;
            heldRb.angularDamping = 0.05f;
            heldRb = null;
        }
        heldObject = null;
    }

    void FixedUpdate()
    {
        if (heldRb != null)
        {
            Vector3 targetPos = transform.position + (transform.forward * holdDistance) + (Vector3.up * holdHeight);
            Vector3 direction = targetPos - heldRb.position;
            float distance = direction.magnitude;

            float forceMultiplier = Mathf.Clamp(distance * distance, 0.1f, 10f);
            Vector3 suctionForce = direction.normalized * (suctionStrength * forceMultiplier);
            Vector3 dragForce = heldRb.linearVelocity * dampening;

            if (distance < 0.05f)
            {
                heldRb.linearVelocity = Vector3.Lerp(heldRb.linearVelocity, Vector3.zero, Time.fixedDeltaTime);
            }

            heldRb.AddForce(suctionForce - dragForce, ForceMode.Acceleration);
        }
    }
}
