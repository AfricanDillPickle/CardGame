using UnityEngine;

public class Rotate : MonoBehaviour
{
    [Tooltip("Rotation speed in degrees per second.")]
    public float rotationSpeed = 50f;

    private Vector3 rotationAxis;

    void Start()
    {
        // Generate a random rotation axis
        rotationAxis = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized; // normalize so speed is consistent
    }

    void Update()
    {
        // Rotate the object along the random axis
        transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
    }
}
