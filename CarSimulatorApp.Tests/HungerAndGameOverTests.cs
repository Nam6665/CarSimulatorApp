using DataLogicLibrary.DirectionStrategies;
using DataLogicLibrary.DTO;
using DataLogicLibrary.Infrastructure.Enums;
using DataLogicLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSimulatorApp.Tests
{
    [TestClass]
    public class HungerAndGameOverTests
    {
        public static SimulationLogicService BuildSut()
        {
            var ctx = new DirectionContext();
            SimulationLogicService.DirectionStrategyResolver resolver = a => a switch
            {
                MovementAction.Left => new TurnLeftStrategy(),
                MovementAction.Right => new TurnRightStrategy(),
                MovementAction.Forward => new DriveForwardStrategy(),
                MovementAction.Backward => new ReverseStrategy(),
                _ => new DriveForwardStrategy()
            };
            return new SimulationLogicService(ctx, resolver);
        }

        private static StatusDTO MakeStatus(
            int gas = 20, int energy = 20, int hunger = 0,
            CardinalDirection dir = CardinalDirection.North,
            MovementAction last = MovementAction.Forward)
        {
            return new StatusDTO
            {
                GasValue = gas,
                EnergyValue = energy,
                HungerValue = hunger,
                CardinalDirection = dir,
                MovementAction = last
            };
        }

        [TestMethod]
        public void EatAction_ResetsHunger_ToZero_AndSetsMessage()
        {
            var sut = BuildSut();
            var s = MakeStatus(hunger: 12);

            var after = sut.PerformAction(7, s);

            Assert.AreEqual(0, after.HungerValue);
            StringAssert.Contains(after.CurrentActionMessage, "Ate food");
        }

        [TestMethod]
        public void MoveAction_IncreasesHunger_ByTwo()
        {
            var sut = BuildSut();
            var s = MakeStatus(hunger: 4);

            var after = sut.PerformAction(1, s);

            Assert.AreEqual(6, after.HungerValue);
        }

        [TestMethod]
        public void NonMovement_Rest_DoesNotIncreaseHunger()
        {
            var sut = BuildSut();
            var s = MakeStatus(hunger: 5);

            var after = sut.PerformAction(5, s);

            Assert.AreEqual(5, after.HungerValue);
        }

        [TestMethod]
        public void Hunger_IsClamped_To20_OnMovement()
        {
            var sut = BuildSut();
            var s = MakeStatus(hunger: 19);

            var after = sut.PerformAction(4, s);

            Assert.AreEqual(20, after.HungerValue);
        }

        [TestMethod]
        public void GameOver_When_Hunger_Reaches20()
        {
            var sut = BuildSut();
            var s = MakeStatus(hunger: 19);

            var after = sut.PerformAction(3, s);

            Assert.IsTrue(after.IsGameOver);
            StringAssert.Contains(after.CurrentActionMessage, "Game over");
        }

        [TestMethod]
        public void HungerStatus_Full_When_0_To_5()
        {
            var s = MakeStatus(hunger: 5);
            Assert.AreEqual(HungerStatus.Full, s.GetHungerStatus());
        }

        [TestMethod]
        public void HungerStatus_Hungry_When_6_To_10()
        {
            var s = MakeStatus(hunger: 8);
            Assert.AreEqual(HungerStatus.Hungry, s.GetHungerStatus());
        }

        [TestMethod]
        public void HungerStatus_Starving_When_Above10()
        {
            var s = MakeStatus(hunger: 12);
            Assert.AreEqual(HungerStatus.Starving, s.GetHungerStatus());
        }
        [TestMethod]
        public void EatAction_SetsActionMessage()
        {
            var sut = BuildSut();
            var s = MakeStatus(hunger: 15);

            var after = sut.PerformAction(7, s);

            StringAssert.Contains(after.CurrentActionMessage, "Ate food");
        }

        [TestMethod]
        public void Hunger_DoesNotGoBelowZero_WhenEating()
        {
            var sut = BuildSut();
            var s = MakeStatus(hunger: 0);

            var after = sut.PerformAction(7, s);

            Assert.AreEqual(0, after.HungerValue);
        }
        [TestMethod]
        public void Hunger_IncreasesOnlyOnMovement()
        {
            var sut = BuildSut();
            var s = MakeStatus(hunger: 5);

            var rest = sut.PerformAction(5, s);
            Assert.AreEqual(5, rest.HungerValue);

            var move = sut.PerformAction(3, s);
            Assert.AreEqual(7, move.HungerValue);
        }

    }
}
