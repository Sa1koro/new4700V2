using UnityEngine;

public class Lever2Event1 : MonoBehaviour
{
    [SerializeField] private GameObject lever;
    [SerializeField] private GameObject waterGate1;
    [SerializeField] private GameObject waterGate2;

    [SerializeField] private bool playerInRange = false;
    [SerializeField] private bool isGate1Active = true; // Tracks which gate is currently active

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            ToggleWaterGates();
        }
    }

    private void ToggleWaterGates()
    {
        isGate1Active = !isGate1Active; // Flip state
        waterGate1.SetActive(isGate1Active);
        waterGate2.SetActive(!isGate1Active);

        Debug.Log($"Lever activated! WaterGate1: {waterGate1.activeSelf}, WaterGate2: {waterGate2.activeSelf}");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered lever zone.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            Debug.Log("Player left lever zone.");
        }
    }
}
