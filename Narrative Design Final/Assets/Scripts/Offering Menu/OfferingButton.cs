using UnityEngine;

public class OfferingButton : MonoBehaviour
{
    private OfferingText offeringText;
    private InteractableItem myItem;
    
    public void Initialize(OfferingText parent, InteractableItem item)
    {
        offeringText = parent;
        myItem = item;
    }

    public void OfferItemButton()
    {
        offeringText.OfferItem(myItem);
    }
}
