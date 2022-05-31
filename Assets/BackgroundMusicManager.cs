using UnityEngine;

namespace Assets
{
    [RequireComponent(typeof(AudioSource))]
    public class BackgroundMusicManager : MonoBehaviour
    {
        [SerializeField]
        private AudioClip _regularTheme;
        [SerializeField]
        private AudioClip _deathTheme;

        private AudioSource _audioSource;

        void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            ServiceLocator.Instance.Flower.OnDeath += SetDeathTheme;
            ServiceLocator.Instance.Menu.OnRestartButtonClick += SetRegularTheme;
        }

        private void SetRegularTheme()
        {
            Debug.Log("Set normal music");
            _audioSource.clip = _regularTheme;
            _audioSource.Play();
        }

        private void SetDeathTheme()
        {
            Debug.Log("Set scary music");
            _audioSource.clip = _deathTheme;
            _audioSource.Play();
        }

    }
}
