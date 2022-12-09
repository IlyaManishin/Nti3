using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace TheGameIdk
{
    public class PlayerHealth : MonoBehaviour {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private int _maxLevelCurse = 5;
        [SerializeField] private float _curseDamagePerFrame;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Text _helpText;

        [SerializeField] private GameObject _gameManager;

        private float _currentHealth;
        private int _currentLevelCurse = 0;
        private float _currentCurseTime = 0;

        private void Awake() {
            _currentHealth = _maxHealth;
        }

        public void setCurse(float curseTime) {
            if(_currentLevelCurse < _maxLevelCurse) {
                _currentLevelCurse += 1;
            }
            _currentCurseTime = curseTime;
            if (_currentCurseTime != 0) {
                _helpText.text = "Ouch, it hurts!";
                _helpText.color = new Color(1, 1, 1, 1);
                StartCoroutine(HelpText());
            }
        }

        IEnumerator HelpText() {
            while(_helpText.color.a > 0) {
                _helpText.color = Color.Lerp(_helpText.color, new Color(1,1,1,0),  Time.deltaTime);
                yield return null;
            }
        }

        private void FixedUpdate() {
            if(_currentCurseTime > 0) {
                _currentHealth -= _curseDamagePerFrame * _currentLevelCurse;
                if(_currentHealth <= 0) {
                    _currentCurseTime = 0;
                    _currentLevelCurse = 0;
                    _gameManager.GetComponent<TheEnd>().TheEndding();
                }
                _currentCurseTime -= Time.deltaTime;
                _healthBar.value = _currentHealth/_maxHealth;
            }
            else {
                _currentCurseTime = 0;
                _currentLevelCurse = 0;
            }
        }
    }
}
