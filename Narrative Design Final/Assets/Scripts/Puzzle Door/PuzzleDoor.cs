using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class PuzzleDoor : UItext
{
    [TextArea] [SerializeField] private string failText;
    [TextArea] [SerializeField] private string successText;
    [SerializeField] private GameObject openDoorButtonPrefab;
    
    private GameObject buttonMenu;
    private GameObject myButton;
    
    private bool exitedTrigger = true;
    
    protected override void Awake()
    {
        GameObject hud = GameObject.FindWithTag("PlayerHUD");
        messageBox = hud.transform.Find("Textbox").gameObject;
        message = messageBox.GetComponentInChildren<TextMeshProUGUI>();
        
        buttonMenu = hud.transform.Find("Item Menu").gameObject;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (!exitedTrigger) return;
        exitedTrigger = false;
            
        message.text = text;
        messageBox.SetActive(true);
        
        FindFirstObjectByType<Player>().SetIsInMenu(true);
        FindFirstObjectByType<PlayerInteraction>().ChangeInventoryVisibility(false);
        buttonMenu.SetActive(true);
        GenerateOpenDoorButton();
    }

    protected override void OnTriggerExit(Collider other)
    {
        exitedTrigger = true;
        ExitMenu();
    }

    private void GenerateOpenDoorButton()
    {
        foreach (Transform child in buttonMenu.transform) {
            Destroy(child.gameObject);
        }
        
        myButton = Instantiate(openDoorButtonPrefab, buttonMenu.transform);
        myButton.GetComponent<PuzzleDoorButton>().Initialize(this);
    }
    
    private void ExitMenu()
    {
        messageBox.SetActive(false);
        buttonMenu.SetActive(false);
        FindFirstObjectByType<Player>().SetIsInMenu(false);
    }

    public void AttemptDoorOpen()
    {
        List<bool> correctOfferings = new List<bool>();
        List<OfferingText> offeringTexts = new List<OfferingText>();
        
        offeringTexts.AddRange(FindObjectsByType<OfferingText>(FindObjectsSortMode.None));
        foreach (OfferingText offeringText in offeringTexts) {
            correctOfferings.Add(offeringText.HasCorrectOffering());
        }

        if (correctOfferings.All(t => t)) {
            message.text = successText;
            this.gameObject.SetActive(false);
        } else {
            int correctCount = 0;
            foreach (bool response in correctOfferings) {
                if (response) {
                    correctCount++;
                }
            }
            message.text = failText + correctCount + " of the gems light up. It seems your solution was not correct.";
        }
        
        foreach (Transform child in buttonMenu.transform) {
            Destroy(child.gameObject);
        }
    }
}
