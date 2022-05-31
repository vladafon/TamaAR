using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Assets.Flower;
using UnityEngine;

namespace Assets
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField]
        [Range(0, 500)]
        private int _decreaseMinutes = 30;

        [SerializeField]
        [Range(0, 500)]
        private int _deathInMinutes = 3 * 60;

        private void Start()
        {
            ServiceLocator.Instance.Menu.OnSaveButtonClick += SaveGame;
        }
        public StateModel LoadGame()
        {
            Debug.Log("Loading");
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                FileStream file = File.Open(Application.persistentDataPath + "/gamesave.save", FileMode.Open);
                StateModel save = (StateModel)bf.Deserialize(file);
                file.Close();

                ApplyTime(save);

                Debug.Log("Loaded from file");
                return save;
            }
            catch(Exception ex)
            {
                Debug.Log(ex.Message);
                Debug.Log("Loaded default");
            }

            //если нет сейва, то грузим дефолтные значения
            return new StateModel
            {
                Thirsty = 50,
                Health = 100,
                Love = 50,
                KilledBySave = false,
                SaveDateTime = DateTime.Now
            };
        }

        private void ApplyTime(StateModel save)
        {
            var currentDateTime = DateTime.UtcNow;

            if (currentDateTime < save.SaveDateTime)
            {
                Debug.Log("Save date in the future!");
                return;
            }

            var timeSpan = currentDateTime - save.SaveDateTime;
            var totalMinutes = timeSpan.TotalMinutes;

            var changeNumber = ServiceLocator.Instance.Flower.ChangeNumber;

            //сначала вычитаем thirsty и love
            while (totalMinutes >= _decreaseMinutes)
            {
                if (save.Thirsty == 0 && save.Love == 0)
                {
                    break;
                }

                if (save.Thirsty > 0)
                {
                    save.Thirsty -= changeNumber;
                    if (save.Thirsty < 0)
                    {
                        save.Thirsty = 0;
                    }
                }

                if (save.Love > 0)
                {
                    save.Love -= changeNumber;
                    if (save.Love < 0)
                    {
                        save.Love = 0;
                    }
                }

                totalMinutes -= _decreaseMinutes;
            }

            //потом начинаем вычитать здоровье
            while (totalMinutes >= _decreaseMinutes)
            {
                if (save.Health == 0)
                {
                    break;
                }

                
                if (save.Health > 0)
                {
                    save.Health -= changeNumber / 2;
                    if (save.Health < 0)
                    {
                        save.Health = 0;
                    }
                }

                totalMinutes -= _decreaseMinutes;
            }

            //потом проверяем смерть
            if (totalMinutes >= _deathInMinutes)
            {
                save.KilledBySave = true;
            }
        }

        public void SaveGame(StateModel stats)
        {
            stats.SaveDateTime = DateTime.UtcNow;
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath + "/gamesave.save");
            bf.Serialize(file, stats);
            file.Close();

            Debug.Log("Saved!");
        }
    }
}
