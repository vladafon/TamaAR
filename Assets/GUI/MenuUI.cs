using System;
using Assets.Flower;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class MenuUI : MonoBehaviour
    {
        public event Action<StateModel> OnSaveButtonClick;
        public event Action OnRestartButtonClick;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _panel;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private GameObject _menuText;
        [SerializeField] private GameObject _saveText;

        private void Awake()
        {
            _canvas.enabled = false;
            _menuText.SetActive(false);
            _saveText.SetActive(false);
            _restartButton.gameObject.SetActive(false);
        }

        private void Start()
        {
            _closeButton.onClick.AddListener(Close);
            _saveButton.onClick.AddListener(Save);
            _restartButton.onClick.AddListener(Restart);
            _exitButton.onClick.AddListener(Exit);

            ServiceLocator.Instance.Flower.OnDeath += ShowRestartButton;
        }

        private void ShowRestartButton()
        {
            _restartButton.gameObject.SetActive(true);
        }

        private void OnDestroy()
        {
            _closeButton.onClick.RemoveListener(Close);
            _saveButton.onClick.RemoveListener(Save);
            _restartButton.onClick.RemoveListener(Restart);
            _exitButton.onClick.RemoveListener(Exit);
        }

        private void Exit()
        {
            Debug.Log("Exit");
            Application.Quit();
        }

        public void Open()
        {
            _canvas.enabled = true;
            _menuText.SetActive(true);
            _panel.gameObject.SetActive(true);
            Debug.Log("[UI] Open Menu");
        }

        private void Close()
        {
            _canvas.enabled = false;
            _menuText.SetActive(false);
            _saveText.SetActive(false);
            _panel.gameObject.SetActive(false);
            Debug.Log("[UI] Close Menu");
        }

        private void Save()
        {
            var stats = ServiceLocator.Instance.Flower.GetStats();
            OnSaveButtonClick?.Invoke(stats);

            _saveText.SetActive(true);
        }

        private void Restart()
        {
            Debug.Log("Restart");

            OnRestartButtonClick?.Invoke();
            _restartButton.gameObject.SetActive(false);
        }
    }
}
