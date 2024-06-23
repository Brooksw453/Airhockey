using UnityEngine;
using System.Collections.Generic;

public class ObjectResetter : MonoBehaviour
{
    [Tooltip("The y-coordinate of the reset plane. Objects below this value will be reset.")]
    public float resetPlaneY = -10f;
    
    [Tooltip("List of objects to be monitored and reset if they fall below the reset plane.")]
    public List<GameObject> objectsToReset = new List<GameObject>();

    private Dictionary<GameObject, Vector3> originalPositions = new Dictionary<GameObject, Vector3>();
    private Dictionary<GameObject, Quaternion> originalRotations = new Dictionary<GameObject, Quaternion>();

    void Start()
    {
        // Store the original positions and rotations of the objects
        foreach (GameObject obj in objectsToReset)
        {
            if (obj != null)
            {
                originalPositions[obj] = obj.transform.position;
                originalRotations[obj] = obj.transform.rotation;
            }
        }
    }

    void Update()
    {
        // Check each object's position
        foreach (GameObject obj in objectsToReset)
        {
            if (obj != null && obj.transform.position.y < resetPlaneY)
            {
                ResetObjectPositionAndRotation(obj);
            }
        }
    }

    void ResetObjectPositionAndRotation(GameObject obj)
    {
        if (originalPositions.ContainsKey(obj) && originalRotations.ContainsKey(obj))
        {
            obj.transform.position = originalPositions[obj];
            obj.transform.rotation = originalRotations[obj];
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            // Optional: Log the reset action
            Debug.Log($"{obj.name} has been reset to its original position and rotation.");
        }
    }

    // Optional: To dynamically add objects to be monitored
    public void AddObjectToReset(GameObject obj)
    {
        if (obj != null && !objectsToReset.Contains(obj))
        {
            objectsToReset.Add(obj);
            originalPositions[obj] = obj.transform.position;
            originalRotations[obj] = obj.transform.rotation;
        }
    }
}
