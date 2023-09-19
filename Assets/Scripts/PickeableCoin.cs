using BindingOfChars;
using UnityEngine;

public class PickeableCoin : Pickeable
{
    public int Quantity;

    protected override bool Pick(Collider2D picker)
    {
        if (picker.CompareTag("Player"))
        {
            picker.GetComponent<PlayerInventory>().Coins.Value += Quantity;
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
