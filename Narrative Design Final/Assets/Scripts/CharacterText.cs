using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CharacterText : UItext
{
    private GameObject nameplateObject;
    private TextMeshProUGUI nameplate;
    private Image displayImage;
    [SerializeField] private Sprite characterSprite;
    [SerializeField] private string characterName;

    protected override void Awake()
    {
        GameObject hud = GameObject.FindWithTag("PlayerHUD");
        messageBox = hud.transform.Find("Textbox").gameObject;
        message = messageBox.GetComponentInChildren<TextMeshProUGUI>();
        
        nameplateObject = hud.transform.Find("Nameplate").gameObject;
        nameplate = nameplateObject.GetComponentInChildren<TextMeshProUGUI>();
        displayImage = hud.transform.Find("CharacterImage").gameObject.GetComponent<Image>();
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        message.text = text;
        messageBox.SetActive(true);
        
        displayImage.gameObject.SetActive(true);
        nameplateObject.SetActive(true);
        displayImage.sprite = characterSprite;
        nameplate.text = characterName;
    }

    protected override void OnTriggerExit(Collider other)
    {
        messageBox.SetActive(false);
        
        displayImage.gameObject.SetActive(false);
        nameplateObject.SetActive(false);
    }
}
