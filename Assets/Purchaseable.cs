using UnityEngine;
using TMPro;
using BindingOfChars;

public abstract class Purchaseable : Pickeable
{
    [SerializeField] protected int _price;
    [SerializeField] private TextMeshProUGUI _priceText;
    protected PlayerInventory PlayerInventory;

    private void Start()
    {
        _priceText.text = _price.ToString();
    }

    protected bool CanBePurchaseable(PlayerInventory playerInventory)
    {
        return playerInventory.Coins.Value >= _price;
    }

    protected void Buy(PlayerInventory playerInventory)
    {
        playerInventory.Coins.Value -= _price;
    }


    protected override bool Pick(Collider2D picker)
    {
        if (!picker.CompareTag("Player")) return false;

        PlayerInventory = picker.GetComponent<PlayerInventory>();

        if (!CanBePurchaseable(PlayerInventory)) return false;

        Buy(PlayerInventory);

        return true;
    }
}
