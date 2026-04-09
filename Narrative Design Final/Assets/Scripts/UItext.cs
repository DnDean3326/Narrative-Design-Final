using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UItext : MonoBehaviour
{

    [SerializeField] public GameObject messageBox;
    [SerializeField] public TextMeshProUGUI message;
    [SerializeField] private string text;

    private void OnTriggerEnter(Collider other)
    {
        print("UI trigger entered");
        message.text = text;
        messageBox.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
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
