using UnityEngine;
using BindingOfChars;

public class PurchaseableBomb : Purchaseable
{
    public int Quantity;

    protected override bool Pick(Collider2D picker)
    {
        if (base.Pick(picker))
        {
            PlayerInventory.Bombs.Value++;
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
