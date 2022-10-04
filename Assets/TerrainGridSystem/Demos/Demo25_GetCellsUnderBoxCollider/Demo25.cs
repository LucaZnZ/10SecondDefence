using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TGS;

/// <summary>
/// Example of using CellGetInArea using a box collider
/// </summary>
public class Demo25 : MonoBehaviour {
    // Instance of TGS reference
    TerrainGridSystem tgs;
    // Object to track
    public GameObject objectToCheck;
    BoxCollider currentBoxCollider;

    // Speed of movement and rotation
    float speed = 20;
    float rotationSpeed = 2;

    // Parameters to pass through to our new method
    [SerializeField] int resolution = 10;
    [SerializeField] float padding = 0.1f;
    [SerializeField] float offset = 0.0f;

    // Start is called before the first frame update
    void Start() {
        // Get reference to instance of TGS
        tgs = TerrainGridSystem.instance;
        // Get the collider from the assigned object
        currentBoxCollider = objectToCheck.GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update() {
        // If we have a collider
        if (currentBoxCollider == null) return;

        // Move it around
        MoveRotateObject();
        // Clear grid
        tgs.ClearAll();
        // Initialize Cell list
        List<int> cellsUnderBoxCollider = new List<int>();
        // Use CellGetInArea with box collider to get cells under it
        tgs.CellGetInArea(currentBoxCollider, cellsUnderBoxCollider, resolution, padding, offset);

        // If we have more than one cell
        if (cellsUnderBoxCollider != null) {
            // Color the cells under the box colllider
            for (int k = 0; k < cellsUnderBoxCollider.Count; k++) {
                tgs.CellSetColor(cellsUnderBoxCollider[k], Color.red);
            }
        }
    }

    // Code to move the object around so you can demo the projection
    void MoveRotateObject() {
#if ENABLE_LEGACY_INPUT_MANAGER
        // Move in alignment with world
        Vector3 move = Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal");
        objectToCheck.transform.position += move * speed * Time.deltaTime;

        // Rotate on all axes
        if (Input.GetKey(KeyCode.Space)) {
            float angle = 90 * rotationSpeed * Time.deltaTime;
            objectToCheck.transform.Rotate(angle, angle, angle);
        }
#endif
    }
}
