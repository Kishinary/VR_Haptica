using UnityEngine;

public class FingerPressureDetector : MonoBehaviour
{
    public string fingerName;
    public float maxPressureDistance = 0.01f;  // Max “squash” distance (1 cm)
    public float pressureValue = 0f;

    private bool touching = false;
    private Collider touchedObject;

    private HapticManager hapticManager;

    private RunTIme_Debuger debugger;

    private void Start()
    {
        hapticManager = FindAnyObjectByType<HapticManager>();
        debugger = FindFirstObjectByType<RunTIme_Debuger>();
        if (!hapticManager)
        {
            Debug.Log("Missing Haptic Manager");
        }

    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Finger")) return; // Ignore other fingers
        // When fingertip touches something
        hapticManager.SendHapticSignal(fingerName, 255); // max intensity
        debugger.Colorchanger(fingerName, 255);

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Finger")) return; // Ignore other fingers
        hapticManager.SendHapticSignal(fingerName, 0); // stop vibration
        debugger.Colorchanger(fingerName, 0);
    }
    void Update()
    {
        if (!touching || touchedObject == null)
        {
            pressureValue = 0;
            return;
        }

        // Measure penetration depth
        Vector3 closest = touchedObject.ClosestPoint(transform.position);
        float distance = Vector3.Distance(transform.position, closest);

        // Convert distance → pressure (inverse)
        pressureValue = Mathf.Clamp01((maxPressureDistance - distance) / maxPressureDistance);

        // Debug
        Debug.Log($"{fingerName} pressure = {pressureValue}");
    }
}