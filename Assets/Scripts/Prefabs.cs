using UnityEngine;

namespace BindingOfChars
{
    public static class Prefabs
    {
        public static GameObject Player => Resources.Load<GameObject>("Prefabs/Player");
        public static GameObject Coin => Resources.Load<GameObject>("Prefabs/Coin");

    }
}