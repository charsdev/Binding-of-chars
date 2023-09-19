using Assets.Scripts;
using UnityEngine;

namespace BindingOfChars
{
    public class PickUpLife : MonoBehaviour
    {
        [SerializeField] private float lifeBonus;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Health>().AddLife(lifeBonus);
                Destroy(gameObject);
            }    
        }
    }
}