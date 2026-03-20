using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private CharacterController controller;
    [Header("Debug Options")]
    public bool showInteractSphere = false;

    private Transform interactSphereTransform;
    private InteractSphere interactSphereScript;

    private GameObject heldObject;

    private void Awake()
    {
        //This code attempts to get a component on the attatched game object and otherwise creates it.
        //This ensures that the required component is available without having to add it manually.
        if (!TryGetComponent<CharacterController>(out controller))
            controller = gameObject.AddComponent<CharacterController>();

        //This code searches for the InteractSphere, and if it doesn't exist, it creates a new one.
        //This is slightly unnecessary to be completely honest.
        interactSphereTransform = transform.Find("InteractSphere");
        if (!interactSphereTransform)
        {
            var interactSphereObj = new GameObject("InteractSphere");
            interactSphereObj.AddComponent<InteractSphere>();
            interactSphereObj.transform.SetParent(transform);
            interactSphereObj.transform.localPosition = new Vector3(0, 0, 1);
            interactSphereObj.AddComponent<SphereCollider>().isTrigger = true;
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

    //Callback function definitions for each of the interactions
    private void OnInteractStarted(InputAction.CallbackContext ctx)
    {

        Debug.Log("Interact started");
    }

    private void OnInteractPerformed(InputAction.CallbackContext ctx)
    {
        var objectInRange = interactSphereScript.GetNearestInRange();
        if (objectInRange)
        {
            heldObject = objectInRange;
            heldObject.transform.SetParent(transform);
            heldObject.transform.localPosition = new Vector3(0, 0, 1.5f);
        }
        Debug.Log(objectInRange);
        //Debug.Log("Interact held");
    }
    private void OnInteractCanceled(InputAction.CallbackContext ctx)
    {

        Debug.Log("Interact canceled");
        if (heldObject)
        {
            heldObject.transform.SetParent(null);
            heldObject = null;
        }
    }
    void Update()
    {
        if (heldObject != null)
        {
            heldObject.transform.position = transform.position + transform.forward * 1.5f;
        }
    }
}
