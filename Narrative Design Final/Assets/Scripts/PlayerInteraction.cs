using System.Collections.Generic;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using System.Linq;

public class PlayerInteraction : MonoBehaviour
{
    public float interactDistance = 5f;

    [SerializeField] private List<InteractableItem> inventoryList = new List<InteractableItem>();
    [SerializeField] private GameObject inventoryBg;
    [SerializeField] private TextMeshProUGUI inventoryText;

    private CinemachineCamera mainCamera;
    private Canvas hud;
    private GameObject pickupText;

    private void Start()
    {
        mainCamera = transform.GetComponentInChildren<CinemachineCamera>();
        hud = transform.GetComponentInChildren<Canvas>();
        pickupText = hud.transform.Find("ItemPickupText").gameObject;
        
        inventoryBg.SetActive(false);
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

            if (item.requiredItem == "" || inventoryList.Any(t => t.itemFlag == item.requiredItem))
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
        if (inventoryList.Count == 0) {
            inventoryBg.SetActive(true);
        }
        inventoryList.Add(item);
        DisplayText(item.itemFlag + " picked up");
        UpdateInventoryDisplay();
    }

    public void RemoveItemFromInventory(string itemName)
    {
        if (inventoryList.Any(t => t.itemFlag == itemName))
        {
            InteractableItem itemToRemove = inventoryList.Find(t => t.itemFlag == itemName);
            inventoryList.Remove(itemToRemove);
        }
        else
        {
            Debug.LogWarning("Item " + itemName + " not in player's inventory");
        }

        UpdateInventoryDisplay();
        if (inventoryList.Count == 0) {
            inventoryBg.SetActive(false);
        }
    }

    private void UpdateInventoryDisplay()
    {
        var rect = inventoryBg.GetComponent<RectTransform>().sizeDelta;
        float bgYLength = (10 * 2 + inventoryList.Count * 36 + (inventoryList.Count - 1) * 5);
        rect = new Vector2(rect.x, bgYLength);
        inventoryBg.GetComponent<RectTransform>().sizeDelta = rect;
        
        string inventoryString = null;
        foreach (InteractableItem item in inventoryList) {
            string newItem = "- " + item.itemFlag + "\n";
            inventoryString += newItem;
        }
        inventoryText.text = inventoryString;
    }

    private void DisplayText(string text)
    {
        pickupText.GetComponent<TextMeshProUGUI>().text = text;
        pickupText.GetComponent<Animation>().Play();
    }

    public void ChangeInventoryVisibility(bool visible)
    {
        inventoryBg.SetActive(visible);
    }

    public List<InteractableItem> GetInventory()
    {
        return inventoryList;
    }
}
