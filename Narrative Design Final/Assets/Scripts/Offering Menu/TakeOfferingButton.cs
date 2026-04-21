using System;
using UnityEngine;

public class TakeOfferingButton : MonoBehaviour
{
    private OfferingText offeringText;

    public void Initialize(OfferingText parent)
    {
        offeringText = parent;
    }

    public void OfferItemButton()
    {
        offeringText.TakeOfferItem();
    }
}
