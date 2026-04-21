using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractableItem : MonoBehaviour
{
    public string itemFlag;
    public string requiredItem = null;

    public UnityEvent onInteractEvent;

    public void AddToPlayerInventory()
    {
        FindFirstObjectByType<PlayerInteraction>().GetComponent<PlayerInteraction>().AddItemToInventory(this);
        FindFirstObjectByType<Player>().HideTextbox();
    }

    public void RemoveItemFromPlayerInventory(string item)
    {
        FindFirstObjectByType<PlayerInteraction>().GetComponent<PlayerInteraction>().RemoveItemFromInventory(item);
        FindFirstObjectByType<Player>().HideTextbox();
    }

    public void DestroyObject()
    {
        this.gameObject.SetActive(false);
    }
}
