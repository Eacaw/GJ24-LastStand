using UnityEngine;

public class SelectionArrow : MonoBehaviour
{
    public float rotationSpeed = 10f; // Speed of rotation in degrees per second
    public float bobSpeed = 1f; // Speed of bobbing in units per second
    public float bobHeight = 0.1f; // Height of bobbing motion

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        // Rotate the model clockwise
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // Bob the model up and down
        float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
