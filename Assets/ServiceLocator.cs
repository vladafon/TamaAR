using Assets.GUI;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets
{
    public class ServiceLocator : MonoBehaviour
    {
        public static ServiceLocator Instance;

        [SerializeField] private MenuUI menu;
        public MenuUI Menu => menu;

        [SerializeField] private InGameUI inGamePanel;
        public InGameUI InGamePanel => inGamePanel;

        [SerializeField] private Flower.Flower flower;
        public Flower.Flower Flower => flower;

        [SerializeField] private SaveManager saveManager;
        public SaveManager SaveManager => saveManager;

        private void Awake()
        {
            Instance = this;
            // other instances here

            Validate();
        }

        private void Validate()
        {
            // in order to throw exception on awake, not only on referencing
            Assert.IsNotNull(menu);
            Assert.IsNotNull(inGamePanel);
            Assert.IsNotNull(flower);
            Assert.IsNotNull(saveManager);
        }
    }
}
