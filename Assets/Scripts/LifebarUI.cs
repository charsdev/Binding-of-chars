using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BindingOfChars
{
    public class LifebarUI : MonoBehaviour
    {
        [SerializeField] private Sprite _empty, _full, _half;
        [SerializeField] private CharacterStats _characterStats;
        [SerializeField] private Health _health;
        [SerializeField] private Image _lifeUIPrefab;
        [SerializeField] private List<Image> _hearts = new List<Image>();

        private void Start()
        {
            if (_characterStats != null)
            {
                var lifes = _characterStats.Life;
                
                for (int i = 0; i < lifes; i++)
                {
                    _hearts.Add(Instantiate(_lifeUIPrefab, transform.position, Quaternion.identity, transform));
                }
            }

            _health.AddDamageEvent(_ => SetUILifeBar());
            _health.AddLifeEvent(SetUILifeBar);
        }

        private void SetUILifeBar()
        {
            if (_characterStats != null)
            {
                if (_characterStats.Life > _hearts.Count)
                {
                    _hearts.Add(Instantiate(_lifeUIPrefab, transform.position, Quaternion.identity, transform));
                }

                for (int i = 0; i < _hearts.Count; i++)
                {
                    float remaindHealth = Mathf.Clamp(_characterStats.Life - i, 0, 1);

                    if (remaindHealth == 0)
                    {
                        _hearts[i].sprite = _empty;
                    }
                    else if (remaindHealth == 0.5f)
                    {
                        _hearts[i].sprite = _half;
                    }
                    else if (remaindHealth == 1)
                    {
                        _hearts[i].gameObject.SetActive(true);
                        _hearts[i].sprite = _full;
                    }
                }
            }
        }
    }
}