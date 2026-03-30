using UnityEngine;

public class Interactable : MonoBehaviour
{
    private CharacterController controller;

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
        //This is slightly unnecessary to be completely honest, but it's a good to showcase that scripts
        //can add and manage their own components. I think sticking to one paradigm is best.
        interactSphereTransform = transform.Find("InteractSphere");
        if (!interactSphereTransform)
        {
            var interactSphereObj = new GameObject("InteractSphere");
            interactSphereObj.AddComponent<InteractSphere>();
            interactSphereObj.transform.SetParent(transform);
            interactSphereObj.transform.localPosition = new Vector3(0, 0, 1);
            //interactSphereObj.AddComponent<SphereCollider>().isTrigger = true;
            interactSphereTransform = interactSphereObj.transform;
            interactSphereScript = interactSphereObj.GetComponent<InteractSphere>();
            // var meshFilter = interactSphereObj.GetComponent<MeshFilter>();
            // var meshRenderer = interactSphereObj.GetComponent<MeshRenderer>();
            // meshFilter = interactSphereObj.AddComponent<MeshFilter>();
            // meshFilter.mesh = Resources.GetBuiltinResource<Mesh>("Sphere.fbx");
            // meshRenderer = interactSphereObj.AddComponent<MeshRenderer>();
            // meshRenderer.material.color = Color.green;
            //meshRenderer.enabled = showInteractSphere;
        }

    }

}
