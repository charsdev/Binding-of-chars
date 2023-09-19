using System.Collections.Generic;
using UnityEngine;

namespace BindingOfChars
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        public List<BoxCollider2DExtent> Colliders = new List<BoxCollider2DExtent>();
        public bool HasCollision = false;

        private void FixedUpdate()
        {
            int count = 0;

            for (int i = 0; i < Colliders.Count; i++)
            {
                var element = Colliders[i];

                if (element.Collider2D != null)
                {
                    HasCollision = true;
                }
                else
                {
                    count++;
                }
            }

            if (count == Colliders.Count)
            {
                HasCollision = false;
            }
        }
    }
}