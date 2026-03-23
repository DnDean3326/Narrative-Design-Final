using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UItext : MonoBehaviour
{
  
    public TextMeshProUGUI message;
    public string text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
     
    }

    private void OnTriggerEnter(Collider other)
    {
        message.text = text;
        message.gameObject.SetActive(true);

    }

    private void OnTriggerExit(Collider other)
    {
        message.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (message != null)
        {
            message.gameObject.SetActive(false);
        }
    }
}
