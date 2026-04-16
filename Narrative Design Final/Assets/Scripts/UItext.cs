using UnityEngine;
using TMPro;

public class UItext : MonoBehaviour
{
    public GameObject messageBox;
    public TextMeshProUGUI message;
    public string text;

    public virtual void Awake()
    {
        GameObject hud = GameObject.FindWithTag("PlayerHUD");
        messageBox = hud.transform.Find("Textbox").gameObject;
        message = messageBox.GetComponentInChildren<TextMeshProUGUI>();
    }
    
    public virtual void OnTriggerEnter(Collider other)
    {
        message.text = text;
        messageBox.SetActive(true);
    }

    public virtual void OnTriggerExit(Collider other)
    {
        messageBox.SetActive(false);
    }

    private void OnDestroy()
    {
        if (message != null)
        {
            messageBox.SetActive(false);
        }
    }
}
