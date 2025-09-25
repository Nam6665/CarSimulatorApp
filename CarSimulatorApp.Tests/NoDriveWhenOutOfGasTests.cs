using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLogicLibrary.Services;
using DataLogicLibrary.DTO;
using DataLogicLibrary.DirectionStrategies;
using DataLogicLibrary.Infrastructure.Enums;

namespace CarSimulatorApp.Tests
{
    [TestClass]
    public class NoDriveWhenOutOfGasTests
    {
        private SimulationLogicService sut = null!;


        [TestInitialize]
        public void Setup()
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
            sut = new SimulationLogicService(ctx, resolver);
        }

        private static StatusDTO MakeStatus(CardinalDirection dir, int gas = 0, int energy = 10, MovementAction last = MovementAction.Forward)
            => new StatusDTO { CardinalDirection = dir, GasValue = gas, EnergyValue = energy, MovementAction = last };

        [DataTestMethod]
        [DataRow(1)] // vänster
        [DataRow(2)] // höger
        [DataRow(3)] // framåt
        [DataRow(4)] // backa
        public void PerformAction_DriveAttempt_WithZeroGas_DoesNotChangeDirectionOrMovement(int userInput)
        {
            
            var start = MakeStatus(CardinalDirection.East, gas: 0, energy: 10, last: MovementAction.Forward);

            var result = sut.PerformAction(userInput, start);

            Assert.AreEqual(CardinalDirection.East, result.CardinalDirection, "Direction should not change when gas is 0.");
            Assert.AreEqual(MovementAction.Forward, result.MovementAction, "MovementAction should remain unchanged when gas is 0.");
            Assert.AreEqual(0, result.GasValue, "Gas should remain 0.");
        }

        [TestMethod]
        public void DecreaseStatusValues_WithZeroGas_DoesNotGoNegative()
        {
            var start = MakeStatus(CardinalDirection.North, gas: 0, energy: 10);

            var after = sut.DecreaseStatusValues(3, start);

            Assert.IsTrue(after.GasValue >= 0, "Gas should never go negative.");
        }

    }
}
