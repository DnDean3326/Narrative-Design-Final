using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfferingText : UItext
{
    [SerializeField] private string characterName;
    [SerializeField] private InteractableItem requiredOffering;
    [SerializeField] private InteractableItem heldOffering = null;
    [SerializeField] private bool correctOffering = false;
    
    private GameObject itemMenu;
    [SerializeField] private GameObject itemButtonPrefab;
    [SerializeField] private GameObject takeOfferingButtonPrefab;
    
    private bool exitedTrigger = true;
    
    protected override void Awake()
    {
        GameObject hud = GameObject.FindWithTag("PlayerHUD");
        messageBox = hud.transform.Find("Textbox").gameObject;
        message = messageBox.GetComponentInChildren<TextMeshProUGUI>();
        
        itemMenu = hud.transform.Find("Item Menu").gameObject;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!exitedTrigger) return;
        exitedTrigger = false;
            
        message.text = text;
        messageBox.SetActive(true);
        
        FindFirstObjectByType<Player>().SetIsInMenu(true);
        FindFirstObjectByType<PlayerInteraction>().ChangeInventoryVisibility(false);
        itemMenu.SetActive(true);
        if (heldOffering == null) {
            GenerateItemButtons();
        } else {
            GenerateTakeOfferingButton();
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        exitedTrigger = true;
        ExitMenu();
    }

    private void GenerateItemButtons()
    {
        foreach (Transform child in itemMenu.transform) {
            Destroy(child.gameObject);
        }
        
        List<InteractableItem> inventory = FindFirstObjectByType<PlayerInteraction>().GetInventory();
        foreach (var item in inventory) {
            var inventoryButton = Instantiate(itemButtonPrefab, itemMenu.transform);
            inventoryButton.GetComponentInChildren<TextMeshProUGUI>().text = item.itemFlag;
            inventoryButton.GetComponent<OfferingButton>().Initialize(this, item);
        }
    }

    private void GenerateTakeOfferingButton()
    {
        foreach (Transform child in itemMenu.transform) {
            Destroy(child.gameObject);
        }
        
        var takeOfferingButton = Instantiate(takeOfferingButtonPrefab, itemMenu.transform);
        takeOfferingButton.GetComponentInChildren<TextMeshProUGUI>().text = "Take back the " + heldOffering.itemFlag;
        takeOfferingButton.GetComponent<TakeOfferingButton>().Initialize(this);
    }
    
    private void ExitMenu()
    {
        messageBox.SetActive(false);
        itemMenu.SetActive(false);
        FindFirstObjectByType<Player>().SetIsInMenu(false);
        FindFirstObjectByType<PlayerInteraction>().ChangeInventoryVisibility(true);
    }

    public void OfferItem(InteractableItem offering)
    {
        heldOffering = offering;
        FindFirstObjectByType<PlayerInteraction>().RemoveItemFromInventory(heldOffering.itemFlag);
        message.text = "You offer the " + offering.itemFlag + " to the tomb.";
        if (requiredOffering == offering) {
            correctOffering = true;
        }
        GenerateTakeOfferingButton();
    }

    public void TakeOfferItem()
    {
        FindFirstObjectByType<PlayerInteraction>().AddItemToInventory(heldOffering);
        message.text = "You take the " + heldOffering.itemFlag + " back from the tomb.";
        heldOffering = null;
        if (correctOffering) {
            correctOffering = false;
        }
        GenerateItemButtons();
    }

    public bool HasCorrectOffering()
    {
        return correctOffering;
    }
}
