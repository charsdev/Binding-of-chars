using BindingOfChars;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.Scripts
{
    public class Health : MonoBehaviour 
    {
        [SerializeField] private bool _isInvencible;

        private float _initialInvencibleDelay = 1;
        private float _invencibleDelay;
        public UnityEvent OnDead;
        public UnityEvent<GameObject> OnDamage;
        public UnityEvent OnAddLife;

        public CharacterStats CharacterStats; 

        public bool IsDead { get; private set; }

        private void Start()
        {
            CharacterStats.Life = CharacterStats.MaxLife;
            _invencibleDelay = _initialInvencibleDelay;
        }

        private void Update()
        {
            if (IsDead) return;

            if (_isInvencible)
            {
                _invencibleDelay -= Time.deltaTime;

                if (_invencibleDelay <= 0)
                {
                    _isInvencible = false;
                    _invencibleDelay = _initialInvencibleDelay;
                }
            }

            if (CharacterStats.Life <= 0)
            {
                StopAllCoroutines();
                OnDead.Invoke();
                IsDead = true;
            }
        }

        public void AddDeadEvent(UnityAction call)
        {
            OnDead.AddListener(call);
        }
        public void AddDamageEvent(UnityAction<GameObject> call)
        {
            OnDamage.AddListener(call);
        }
        public void AddLifeEvent(UnityAction call)
        {
            OnAddLife.AddListener(call);
        }

        public void AddLife(float lifeBonus)
        {
            CharacterStats.Life += lifeBonus;
            OnAddLife.Invoke();
        }

        public void ReceiveDamage(GameObject damager, float damage)
        {
            if (!_isInvencible)
            {
                //_isInvencible = true;
                CharacterStats.Life -= damage;
                OnDamage.Invoke(damager);
            }
        }
    }
}
