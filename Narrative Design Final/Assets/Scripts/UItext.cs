using UnityEngine;
using TMPro;

public class UItext : MonoBehaviour
{
    protected GameObject messageBox;
    protected TextMeshProUGUI message;
    [TextArea] [SerializeField] protected string text;

    protected virtual void Awake()
    {
        GameObject hud = GameObject.FindWithTag("PlayerHUD");
        messageBox = hud.transform.Find("Textbox").gameObject;
        message = messageBox.GetComponentInChildren<TextMeshProUGUI>();
    }
    
    protected virtual void OnTriggerEnter(Collider other)
    {
        message.text = text;
        messageBox.SetActive(true);
    }

    protected virtual void OnTriggerExit(Collider other)
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
