using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UItext : MonoBehaviour
{

    [SerializeField] public GameObject messageBox;
    [SerializeField] public TextMeshProUGUI message;
    private string text;

    private void OnTriggerEnter(Collider other)
    {
        message.text = text;
        messageBox.gameObject.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        messageBox.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (message != null)
        {
            messageBox.gameObject.SetActive(false);
        }
    }
}
