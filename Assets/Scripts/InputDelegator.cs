using GameLogic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputDelegator : MonoBehaviour
{
    public UnityEvent<Vector2> onCameraMovement;
    public UnityEvent<float> onCameraZoom;
    public UnityEvent<UnitBehaviour> onUnitSelected;
    public UnityEvent onTerrainClick, onCloseDetails;
    private Controls playerControls;
    [SerializeField] private EventSystem eventSystem;

    private Camera mainCamera;

    private void Start()
    {
        GetComponent<PlayerInput>().onActionTriggered += PerformAction;
        playerControls = new Controls();
        mainCamera = Camera.main;
    }

    private void PerformAction(InputAction.CallbackContext context)
    {
        if (context.action.name == playerControls.Default.CameraMovement.name) OnCameraMovement(context);
        if (context.action.name == playerControls.Default.CameraZoom.name) OnCameraZoom(context);
        if (context.action.name == playerControls.Default.Select.name) OnSelectUnit(context);
        if (context.action.name == playerControls.Default.CloseDetails.name) OnCloseDetails(context);
    }

    private void OnCameraMovement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onCameraMovement?.Invoke(context.ReadValue<Vector2>());
        }
        else if (context.canceled)
        {
            onCameraMovement?.Invoke(Vector2.zero);
        }
    }

    private void OnCameraZoom(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            onCameraZoom?.Invoke(context.ReadValue<float>());
        }
        else if (context.canceled)
        {
            onCameraZoom?.Invoke(0f);
        }
    }

    private void OnSelectUnit(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());
        if (!Physics.Raycast(ray, out var hit) || hit.collider == null) return;
        var obj = hit.collider.gameObject;
        Debug.Log($"Clicked on {obj.name}");
        var unit = obj.GetComponent<UnitBehaviour>();
        if (unit != null)
            onUnitSelected?.Invoke(unit);

        if (obj.CompareTag("TGS"))
            onTerrainClick?.Invoke();
    }

    private void OnCloseDetails(InputAction.CallbackContext context)
    {
        if (context.performed)
            onCloseDetails?.Invoke();
    }
}