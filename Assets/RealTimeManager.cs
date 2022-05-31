using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets
{
    public class RealTimeManager : MonoBehaviour
    {
        [SerializeField]
        [Range(2, 500)]
        private int _decreaseMinutesMean = 30;
        [SerializeField]
        [Range(1, 500)]
        private int _decreaseMinutesDeviation = 5;

        [SerializeField]
        [Range(1, 500)]
        private int _deathInMinutes = 3 * 60;

        private Coroutine _thirstyCoroutine;
        private Coroutine _loveCoroutine;
        private Coroutine _healthCoroutine;
        private Coroutine _deathCoroutine;

        private void Start()
        {
            StartCycle();

            ServiceLocator.Instance.Menu.OnRestartButtonClick += StartCycle;
        }

        public void Update()
        {
            var flower = ServiceLocator.Instance.Flower;
            var stats = flower.GetStats();

            if (stats.Thirsty < flower.CriticalNumber || stats.Love < flower.CriticalNumber)
            {
                if (_healthCoroutine == null)
                {
                    Debug.Log("Starting decreasing health");
                    _healthCoroutine = StartCoroutine(DecreaseHealth());
                }
            }
            else if (_healthCoroutine != null)
            {
                Debug.Log("Stop decreasing health");
                StopCoroutine(_healthCoroutine);
                _healthCoroutine = null;
            }

            if (stats.Health < flower.CriticalNumber)
            {
                if (_deathCoroutine == null)
                {
                    Debug.Log("Starting counting to death");
                    _deathCoroutine = StartCoroutine(CountingForDeath());
                }
            }
            else if (_deathCoroutine != null)
            {
                Debug.Log("Stop counting for death");
                StopCoroutine(_deathCoroutine);
                _deathCoroutine = null;
            }
        }

        private IEnumerator CountingForDeath()
        {
            yield return new WaitForSecondsRealtime(_deathInMinutes * 60);

            ServiceLocator.Instance.Flower.Kill();

            StopCoroutine(_thirstyCoroutine);
            StopCoroutine(_loveCoroutine);
            if (_healthCoroutine != null)
            {
                StopCoroutine(_healthCoroutine);
            }

        }

        private IEnumerator DecreaseThirsty()
        {
            while(true)
            {
                var decreaseMinutes = Random.Range
                    (_decreaseMinutesMean - _decreaseMinutesDeviation
                    , _decreaseMinutesMean + _decreaseMinutesDeviation);

                yield return new WaitForSecondsRealtime(decreaseMinutes * 60);

                ServiceLocator.Instance.Flower.UnWater();

            }
        }
        private IEnumerator DecreaseLove()
        {
            while (true)
            {
                var decreaseMinutes = Random.Range
                    (_decreaseMinutesMean - _decreaseMinutesDeviation
                    , _decreaseMinutesMean + _decreaseMinutesDeviation);


                yield return new WaitForSecondsRealtime(decreaseMinutes * 60);

                ServiceLocator.Instance.Flower.UnPet();

            }
        }
        private IEnumerator DecreaseHealth()
        {
            while (true)
            {
                var decreaseMinutes = Random.Range
                    (_decreaseMinutesMean - _decreaseMinutesDeviation
                    , _decreaseMinutesMean + _decreaseMinutesDeviation);

                yield return new WaitForSecondsRealtime(decreaseMinutes * 60);

                var flower = ServiceLocator.Instance.Flower;
                var stats = flower.GetStats();

                if (stats.Thirsty < flower.CriticalNumber)
                {
                    ServiceLocator.Instance.Flower.UnHeal();
                }

                if (stats.Love < flower.CriticalNumber)
                {
                    ServiceLocator.Instance.Flower.UnHeal();
                }


            }
        }

        private void StartCycle()
        {
            StopAllCoroutines();
            _thirstyCoroutine = StartCoroutine(DecreaseThirsty());
            _loveCoroutine = StartCoroutine(DecreaseLove());

            _healthCoroutine = null;
            _deathCoroutine = null;
        }
    }
}
