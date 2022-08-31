using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homework1
{
    public class Unit : MonoBehaviour
    {
        #region Fields

        private int _health = 0;
        private int _maxHealth = 100;
        private int _healCount = 5;
        private float _healDelay = 0.5f;
        private float _healTime = 3.0f;
        private bool _isHealing = false;

        #endregion

        #region Methods

        public void RecieveHealing()
        {
            if (!_isHealing)
            {
                var healingCoroutine = StartCoroutine(InitHealingCoroutine());
            }
            else
                Debug.Log($"Healing doesn`t complete!");
        }

        IEnumerator InitHealingCoroutine()
        {            
            _isHealing = true;
            for (float i = 0; i < _healTime; i+= _healDelay)
            {
                yield return StartCoroutine(HealingCoroutine());
            }
            _isHealing = false;
        }

        IEnumerator HealingCoroutine()
        {
            Heal(_healCount);
            yield return new WaitForSeconds(_healDelay);
        }

        private void Heal(int count)
        {
            if (_health >= _maxHealth)
            {
                _isHealing = false;
                StopAllCoroutines();
                Debug.Log($"Health is full");
            }
            else
            {
                if (count < _maxHealth - _health)
                    _health += count;
                else
                    _health = _maxHealth;
                Debug.Log($"Health:{_health}");
            }
        }

        #endregion
    }
}
