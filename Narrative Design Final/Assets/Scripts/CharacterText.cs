using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterText : UItext
{
    [SerializeField] private GameObject namePlateObject;
    [SerializeField] private TextMeshProUGUI namePlate;
    [SerializeField] private Image displayImage;
    [SerializeField] private Sprite characterSprite;
    [SerializeField] private string characterName;
    
    public override void OnTriggerEnter(Collider other)
    {
        message.text = text;
        messageBox.SetActive(true);
        
        displayImage.gameObject.SetActive(true);
        namePlateObject.SetActive(true);
        displayImage.sprite = characterSprite;
        namePlate.text = characterName;
    }

    public override void OnTriggerExit(Collider other)
    {
        messageBox.SetActive(false);
        
        displayImage.gameObject.SetActive(false);
        namePlateObject.SetActive(false);
    }
}
