using UnityEngine;

namespace BindingOfChars
{
    public class PlayerInventory : MonoBehaviour
    {
        public Property<int> Coins = new Property<int>("COINS", 0);
        public Property<int> Bombs = new Property<int>("BOMBS", 0);
    }
}