using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DialogueScene : CharacterText
{
    [SerializeField] private GameObject fullscreenButton;
    [SerializeField] private List<Dialogue> dialogues;
    [SerializeField] private int currentDialogue = 0;
    
    protected override void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        message.text = dialogues[0].text;
        nameplate.text = dialogues[0].speaker;
        messageBox.SetActive(true);
        nameplateObject.SetActive(true);
        fullscreenButton.SetActive(true);
        
        FindFirstObjectByType<Player>().SetIsInMenu(true);
        FindFirstObjectByType<Player>().SetIsInScene(true);
        FindFirstObjectByType<PlayerInteraction>().ChangeInventoryVisibility(false);
    }

    public void OnClick()
    {
        if (currentDialogue < dialogues.Count) {
            currentDialogue++;

            if (dialogues[currentDialogue].speaker == "Player") {
                nameplateObject.SetActive(false);
                message.color = Color.darkBlue;
            } else {
                nameplateObject.SetActive(true);
                message.color = Color.white;
            }
            
            message.text = dialogues[currentDialogue].text;
            nameplate.text = dialogues[currentDialogue].speaker;
        } else {
            messageBox.SetActive(false);
            nameplateObject.SetActive(false);
            fullscreenButton.SetActive(false);
            FindFirstObjectByType<Player>().SetIsInMenu(false);
            FindFirstObjectByType<Player>().SetIsInScene(false);
            FindFirstObjectByType<PlayerInteraction>().ChangeInventoryVisibility(true);
        }
    }
}

[System.Serializable]
public class Dialogue
{
    public string speaker;
    public string text;
}