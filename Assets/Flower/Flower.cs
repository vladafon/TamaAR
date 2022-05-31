using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Flower
{
    [RequireComponent(typeof(Animator))]
    public class Flower : MonoBehaviour
    {
        public event Action OnDeath;

        public event Action OnBadState;
        public event Action OnNeutralState;
        public event Action OnCriticalState;

        [SerializeField]
        [Range(0, 100)]
        private int _changeNumber = 25;

        [SerializeField]
        [Range(0, 100)]
        private int _criticalNumber = 25;

        [SerializeField]
        [Range(2, 500)]
        private int _randomStateMean = 5;
        [SerializeField]
        [Range(1, 500)]
        private int _randomStateDeviation = 2;

        private bool _isInBadState;
        private bool _isInCriticalState;
        private bool _isInDeadState;

        private Animator _animator;

        private Coroutine _randomStateCoroutine;

        //STATS
        private int _thirsty;
        private int _love;

        private int _health;

        private bool _killedBySave;

        private bool _isDead;

        public int CriticalNumber => _criticalNumber;
        public int ChangeNumber => _changeNumber;

        public bool IsDead => _isDead;


        void Start()
        {
            _animator = GetComponent<Animator>();

            _randomStateCoroutine = StartCoroutine(SetRandomState());

            ServiceLocator.Instance.Menu.OnRestartButtonClick += Restart;   

            var stats = ServiceLocator.Instance.SaveManager.LoadGame();
            _thirsty = stats.Thirsty;
            _health = stats.Health;
            _love = stats.Love;
            _killedBySave = stats.KilledBySave;
        }

        private void Update()
        {
            if (_killedBySave)
            {
                Kill();

                //ѕосле убийства и совершени€ всех соответствующих действий, флаг снимаем
                _killedBySave = false;
            }

            if (_isDead && _isInDeadState == false)
            {
                _animator.SetBool("DeadStateBool", true);
                _isInDeadState = truFie;
            }

            if (_isDead == false && _isInDeadState)
            {
                _animator.SetBool("DeadStateBool", false);
                _isInDeadState = false;
            }

            if (_health < 100 && _isInCriticalState == false)
            {
                _animator.SetBool("CriticalStateBool", true);
                _isInCriticalState = true;
            }

            if (_health >= 100 && _isInCriticalState)
            {
                _animator.SetBool("CriticalStateBool", false);
                _isInCriticalState = false;
            }

            if ((_thirsty < _criticalNumber
                 || _love < _criticalNumber) && _isInBadState == false)
            {
                _animator.SetBool("BadStateBool", true);
                _isInBadState = true;
            }

            if ((_thirsty >= _criticalNumber
                 && _love >= _criticalNumber) && _isInBadState)
            {
                _animator.SetBool("BadStateBool", false);
                _isInBadState = false;
            }


            if (!_isInDeadState)
            {
                if (!_isInBadState && !_isInCriticalState)
                {
                    OnNeutralState?.Invoke();
                }

                if (_isInBadState)
                {
                    OnBadState?.Invoke();
                }

                if (_isInCriticalState)
                {
                    OnCriticalState?.Invoke();
                }
            }
        }

        public StateModel GetStats()
        {
            return new StateModel
            {
                Health = _health,
                Love = _love,
                Thirsty = _thirsty,
                KilledBySave = _killedBySave,
                SaveDateTime = DateTime.UtcNow
            };
        }

        public void Water()
        {
            Debug.Log($"Water {_thirsty}");

            if (_thirsty == 100 || _health < 100)
            {
                Negation();
                return;
            }

            _animator.SetTrigger("WateringTrigger");

            _thirsty += _changeNumber;

            if (_thirsty > 100)
            {
                _thirsty = 100;
            }
        }

        public void Pet()
        {
            Debug.Log($"Pet {_love}");

            if (_love == 100 || _health < 100)
            {
                Negation();
                return;
            }

            _animator.SetTrigger("PettingTrigger");

            _love += _changeNumber;

            if (_love > 100)
            {
                _love = 100;
            }
        }

        public void Heal()
        {
            Debug.Log($"Heal {_health}");

            if (_health == 100)
            {
                Negation();
                return;
            }

            _animator.SetTrigger("HealingTrigger");

            _health += _changeNumber * 2;

            if (_health > 100)
            {
                _health = 100;
            }
        }

        public void UnWater()
        {
            Debug.Log($"UnWater {_thirsty}");

            if (_thirsty == 0)
            {
                return;
            }
            _thirsty -= _changeNumber;

            if (_thirsty < 0)
            {
                _thirsty = 0;
            }
        }

        public void UnPet()
        {
            Debug.Log($"UnPet {_love}");

            if (_love == 0)
            {
                return;
            }

            _love -= _changeNumber;

            if (_love < 0)
            {
                _love = 0;
            }
        }

        public void UnHeal()
        {
            Debug.Log($"UnHeal {_health}");

            if (_health == 0)
            {
                return;
            }

            _health -= _changeNumber / 2;

            if (_health < 0)
            {
                _health = 0;
            }
        }

        public void Kill()
        {
            Debug.Log("DEAD");

            StopCoroutine(_randomStateCoroutine);

            OnDeath?.Invoke();

            _isDead = true;
        }
        public void Agreement()
        {
            Debug.Log("Want!");
            _animator.SetTrigger("AgreementTrigger");
        }


        private void Restart()
        {
            _randomStateCoroutine = StartCoroutine(SetRandomState());

            _thirsty = 50;
            _love = 50;
            _health = 100;
            _isDead = false;

            ServiceLocator.Instance.SaveManager.SaveGame(GetStats());
        }


        private void Negation()
        {
            Debug.Log("Don' want!");
            _animator.SetTrigger("NegationTrigger");
        }

        private IEnumerator SetRandomState()
        {
            while (true)
            {
                var decreaseMinutes = Random.Range
                (_randomStateMean - _randomStateDeviation
                    , _randomStateMean + _randomStateDeviation);

                yield return new WaitForSecondsRealtime(decreaseMinutes * 60);

                _animator.SetTrigger("RandomStateTrigger");
            }
        }
    }
}
