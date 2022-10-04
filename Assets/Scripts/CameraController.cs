using UnityEngine;

public class CameraController : MonoBehaviour
{
    private new Camera camera;
    [SerializeField] private float movementSpeed = 1f, zoomSpeed = 1f;

    private Vector3 move;
    private float zoom;

    public void SetMovement(Vector2 movement)
    {
        move = movement.magnitude > 0
            ? new Vector3(movement.x, 0, movement.y).normalized * movementSpeed
            : Vector3.zero;
    }

    public void SetZoom(float zoom)
    {
        this.zoom = zoom * zoomSpeed;
    }

    private void MoveCamera(Vector3 movement)
    {
        camera.transform.position += movement;
    }

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        var curMove = move + Vector3.up * zoom;

        if (curMove.magnitude > 0) MoveCamera(curMove * Time.deltaTime);
    }
}