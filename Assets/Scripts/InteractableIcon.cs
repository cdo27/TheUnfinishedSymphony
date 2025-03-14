using System.Collections;
using UnityEngine;

public class InteractableIcon : MonoBehaviour
{
    public bool isActive = true;
    private Vector3 startPos;
    public float floatSpeed = 10f; // Speed of floating
    public float floatAmount = 0.02f; // How much it moves up and down

    void Start()
    {
        startPos = transform.position; // Save original position
        floatSpeed = 6f;
        floatAmount = 0.03f;
    }

    void Update()
    {
        if (isActive)
        {
            // Smooth floating effect
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmount;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }
    }

    // Function to set the icon active or inactive
    public void SetActive(bool state)
    {
        isActive = state;
        gameObject.SetActive(state); // Enable or disable the GameObject
    }
}