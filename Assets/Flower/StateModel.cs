using System;

namespace Assets.Flower
{
    [System.Serializable]
    public class StateModel
    {
        public int Thirsty { get; set; }
        public int Love { get; set; }
        public int Health { get; set; }

        public bool KilledBySave { get; set; }

        public DateTime SaveDateTime { get; set; }
    }
}
