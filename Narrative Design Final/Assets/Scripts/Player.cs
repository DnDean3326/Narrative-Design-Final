using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(FPController))]
[RequireComponent(typeof(PlayerInteraction))]
public class Player : MonoBehaviour
{
    FPController FPController;
    PlayerInteraction interaction;

    private bool isInMenu = false;
    private bool isInScene = false;

    [SerializeField] private GameObject characterImage;
    [SerializeField] private GameObject namePlate;
    [SerializeField] private GameObject textBox;
    [SerializeField] private GameObject itemMenu;

    #region Input Functions
    void OnMove(InputValue value)
    {
        if (isInScene) return;
        FPController.moveInput = value.Get<Vector2>();
    }

    void OnLook(InputValue value)
    {
        if (isInScene) return;
        if (isInMenu) {
            return;
        }
        FPController.lookInput = value.Get<Vector2>();
    }

    void OnSprint(InputValue value)
    {
        if (isInScene) return;
        FPController.sprintInput = value.isPressed;
    }

    void OnJump(InputValue value)
    {
        if (isInScene) return;
        if (value.isPressed)
        {
            FPController.TryJump();
        }
    }

    void OnInteract(InputValue value)
    {
        if (isInScene) return;
        if (value.isPressed)
        {
            interaction.TryInteract();
        }
    }

    void OnCrouch(InputValue value)
    {
        if (isInScene) return;
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
            FPController.lookInput = Vector2.zero;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        } else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public void SetIsInScene(bool sceneStatus)
    {
        isInScene = sceneStatus;
        if (isInScene) {
            FPController.moveInput = Vector2.zero;
        }
    }

    public void HideTextbox()
    {
        textBox.SetActive(false);
    }
}
