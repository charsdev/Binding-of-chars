using UnityEngine;

namespace BindingOfChars
{
    public class CharacterStats : MonoBehaviour
    {
        public float Life;
        public int MaxLife;
        public float Speed;
        public float RateOfFire;
        public float Damage;
        public float ProjectileSpeed;
        public float Lucky;
    }

    public class Stats : ScriptableObject
    {
        public float Life;
        public int MaxLife;
        public float Speed;
        public float RateOfFire;
        public float Damage;
        public float ProjectileSpeed;
        public float Lucky;
    }
}