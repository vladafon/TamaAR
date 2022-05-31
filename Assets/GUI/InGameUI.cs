using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] private Button _menuButton;
        [SerializeField] private Button _statsOpenButton;
        [SerializeField] private Button _statsCloseButton;
        [SerializeField] private Button _waterButton;
        [SerializeField] private Button _petButton;
        [SerializeField] private Button _medicineButton;

        [SerializeField] private RectTransform _statsPanel;
        [SerializeField] private Slider _thirstySlider;
        [SerializeField] private Slider _loveSlider;

        private void Start()
        {
            _statsPanel.gameObject.SetActive(false);
            _menuButton.onClick.AddListener(ServiceLocator.Instance.Menu.Open);
            _statsOpenButton.onClick.AddListener(OpenStats);
            _statsCloseButton.onClick.AddListener(CloseStats);
            _waterButton.onClick.AddListener(ServiceLocator.Instance.Flower.Water);
            _petButton.onClick.AddListener(ServiceLocator.Instance.Flower.Pet);
            _medicineButton.onClick.AddListener(ServiceLocator.Instance.Flower.Heal);

            ServiceLocator.Instance.Flower.OnDeath += DisableButtons;
            ServiceLocator.Instance.Menu.OnRestartButtonClick += EnableButtons;
        }

        private void EnableButtons()
        {
            Debug.Log("Buttons enabled");
            _waterButton.interactable = true;
            _petButton.interactable = true;
            _medicineButton.interactable = true;
        }

        private void DisableButtons()
        {
            Debug.Log("Buttons disabled");
            _waterButton.interactable = false;
            _petButton.interactable = false;
            _medicineButton.interactable = false;
        }

        private void CloseStats()
        {
            _statsPanel.gameObject.SetActive(false);
        }

        private void OpenStats()
        {
            _statsPanel.gameObject.SetActive(true);

            var stats = ServiceLocator.Instance.Flower.GetStats();

            _thirstySlider.value = stats.Thirsty;
            _loveSlider.value = stats.Love;
        }
    }
}
