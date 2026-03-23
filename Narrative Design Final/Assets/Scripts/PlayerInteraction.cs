using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 5f;

    [SerializeField] private List<string> inventoryList = new List<string>();

    private CinemachineCamera mainCamera;
    private Canvas HUD;
    private GameObject pickupText;

    private void Start()
    {
        mainCamera = transform.GetComponentInChildren<CinemachineCamera>();
        HUD = transform.GetComponentInChildren<Canvas>();
        pickupText = HUD.transform.Find("ItemPickupText").gameObject;
    }

    public void TryInteract()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactDistance) && hit.collider.gameObject.GetComponent<InteractableItem>())
        {
            Vector3 hitPoint = hit.point;
            Debug.DrawLine(ray.origin, hitPoint, Color.red);

            InteractableItem item = hit.collider.gameObject.GetComponent<InteractableItem>();

            if (item.requiredItem == "" || inventoryList.Contains(item.requiredItem))
            {
                item.onInteractEvent?.Invoke();
                if(item.requiredItem != "")
                {
                    DisplayText(item.requiredItem.Replace('_', ' ') + " was used");
                }
            }
            else
            {
                DisplayText("I don't have the " + item.requiredItem.Replace('_', ' '));
            }
        }
        else
        {
            Debug.Log("Nothing to interact with");
        }
    }

    public void AddItemToInventory(InteractableItem item)
    {
        inventoryList.Add(item.itemFlag);
        DisplayText(item.gameObject.name + " picked up");
    }

    public void RemoveItemFromInventory(string item)
    {
        if (inventoryList.Contains(item))
        {
            inventoryList.Remove(item);
        }
        else
        {
            Debug.LogWarning("Item " +  item + " not in player's inventory");
        }
    }

    private void DisplayText(string text)
    {
        pickupText.GetComponent<TextMeshProUGUI>().text = text;
        pickupText.GetComponent<Animation>().Play();
    }
}
