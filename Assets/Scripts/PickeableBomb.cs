using BindingOfChars;
using UnityEngine;

public class PickeableBomb : Pickeable
{
    protected override bool Pick(Collider2D picker)
    {
        if (picker.CompareTag("Player"))
        {
            var playerInventory = picker.GetComponent<PlayerInventory>();
            playerInventory.Bombs.Value += 1;
            Destroy(gameObject);
            return true;
        }

        return false;
    }
}
