using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FPController))]
[RequireComponent(typeof(PlayerInteraction))]
public class Player : MonoBehaviour
{
    FPController FPController;
    PlayerInteraction interaction;

    #region Input Functions
    void OnMove(InputValue value)
    {
        FPController.moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        FPController.lookInput = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        FPController.sprintInput = value.isPressed;
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            FPController.TryJump();
        }
    }

    void OnInteract(InputValue value)
    {
        if (value.isPressed)
        {
            interaction.TryInteract();
        }
    }

    void OnCrouch(InputValue value)
    {
        FPController.crouchInput = value.isPressed;
    }
    #endregion

    private void OnValidate()
    {
        if (FPController == null)
        {
            FPController = GetComponent<FPController>();
        }

        if(interaction == null)
        {
            interaction = GetComponent<PlayerInteraction>();
        }
    }

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
