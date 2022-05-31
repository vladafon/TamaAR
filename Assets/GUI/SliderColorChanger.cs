using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.GUI
{
    [RequireComponent(typeof(Image))]
    public class SliderColorChanger : MonoBehaviour
    {
        [SerializeField]
        private Color _neutralColor = new Color(0, 1, 0.08235294f);
        [SerializeField]
        private Color _badColor = new Color(1, 0.8950032f, 0);
        [SerializeField]
        private Color _criticalColor = new Color(1, 0.01960784f, 0);
        [SerializeField]
        private Color _deathColor = new Color(0, 0, 0);

        private Image _image;

        void Start()
        {
            _image = GetComponent<Image>();

            ServiceLocator.Instance.Flower.OnDeath += ChangeToDeathColor;
            ServiceLocator.Instance.Flower.OnBadState += ChangeToBadColor;
            ServiceLocator.Instance.Flower.OnCriticalState += ChangeToCriticalColor;
            ServiceLocator.Instance.Flower.OnNeutralState += ChangeToNeutralColor;

            ServiceLocator.Instance.Menu.OnRestartButtonClick += ChangeToNeutralColor;
        }

        private void ChangeToNeutralColor()
        {
            _image.color = _neutralColor;
        }

        private void ChangeToCriticalColor()
        {
            _image.color = _criticalColor;
        }

        private void ChangeToBadColor()
        {
            _image.color = _badColor;
        }

        private void ChangeToDeathColor()
        {
            _image.color = _deathColor;
        }
    }
}