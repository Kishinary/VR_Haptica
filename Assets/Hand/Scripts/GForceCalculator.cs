using UnityEngine;

public class GForceCalculator : MonoBehaviour
{
    private Rigidbody rb;
    public static float CalculatedGravity { get; private set; } // Bien tinh de chia se gia tri

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            float mass = rb.mass;
            CalculatedGravity = mass * Physics.gravity.magnitude;
            Debug.Log($"Mass of the cube: {mass} kg");
            Debug.Log($"Gravitational force: {CalculatedGravity} N");
        }
        else
        {
            Debug.LogError("No Rigidbody component found on the cube!");
        }
    }
}