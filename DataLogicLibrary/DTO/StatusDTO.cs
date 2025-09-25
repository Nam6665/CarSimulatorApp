using DataLogicLibrary.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLogicLibrary.DTO
{
    public class StatusDTO
    {
        public CardinalDirection CardinalDirection { get; set; }
        public MovementAction MovementAction { get; set; }
        public int GasValue { get; set; } = 20;
        public int EnergyValue { get; set; } = 20;
        public int HungerValue { get; set; } = 0;
        public string CurrentActionMessage { get; set; } = string.Empty;
        public bool IsGameOver { get; set; } = false;
        public HungerStatus GetHungerStatus()
        {
            if (HungerValue <= 5)
                return HungerStatus.Full;
            if (HungerValue <= 10)
                return HungerStatus.Hungry;
            return HungerStatus.Starving;
        }
    }
}
