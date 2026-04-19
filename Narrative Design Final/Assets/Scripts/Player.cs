using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FPController))]
[RequireComponent(typeof(PlayerInteraction))]
public class Player : MonoBehaviour
{
    FPController FPController;
    PlayerInteraction interaction;

    private bool isInMenu = false;

    [SerializeField] private GameObject characterImage;
    [SerializeField] private GameObject namePlate;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject itemMenu;

    #region Input Functions
    void OnMove(InputValue value)
    {
        FPController.moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        if (isInMenu) {
            return;
        }
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
        characterImage.SetActive(false);
        namePlate.SetActive(false);
        textBox.SetActive(false);
        itemMenu.SetActive(false);
    }

    public void SetIsInMenu(bool menuStatus)
    {
        isInMenu = menuStatus;
        if (!isInMenu) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
