using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataLogicLibrary.Services;
using DataLogicLibrary.DTO;
using DataLogicLibrary.DirectionStrategies;
using DataLogicLibrary.Infrastructure.Enums;

namespace CarSimulatorApp.Tests
{
    [TestClass]
    public class SimulationLogicServiceTests
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

        [TestMethod]
        public void PerformAction_WhenRefuel_SetsGasTo20()
        {
            var status = new StatusDTO { GasValue = 0, EnergyValue = 10 };

            var result = sut.PerformAction(6, status);

            Assert.AreEqual(20, result.GasValue);
        }

        [TestMethod]
        public void PerformAction_WhenRest_DoesNotChangeGas()
        {
            var status = new StatusDTO { GasValue = 10, EnergyValue = 10 };

            var result = sut.PerformAction(5, status);

            Assert.AreEqual(10, result.GasValue);
        }

        [TestMethod]
        public void DecreaseStatusValues_WhenResting_DoesNotReduceGas()
        {
            var status = new StatusDTO { GasValue = 20, EnergyValue = 20 };

            var result = sut.DecreaseStatusValues(5, status);

            Assert.AreEqual(20, result.GasValue);
        }
        [TestMethod]
        public void PerformAction_Rest_SetsEnergyTo20()
        {
            var status = new StatusDTO { EnergyValue = 5 };

            var result = sut.PerformAction(5, status);

            Assert.AreEqual(20, result.EnergyValue);
        }

        [TestMethod]
        public void PerformAction_Refuel_SetsGasTo20_AndKeepsEnergy()
        {
            var status = new StatusDTO { GasValue = 1, EnergyValue = 10 };

            var result = sut.PerformAction(6, status);

            Assert.AreEqual(20, result.GasValue);
            Assert.AreEqual(10, result.EnergyValue);
        }

        [TestMethod]
        public void DecreaseStatusValues_EnergyDoesNotGoNegative()
        {
            var status = new StatusDTO { GasValue = 10, EnergyValue = 0 };

            var result = sut.DecreaseStatusValues(3, status);

            Assert.IsTrue(result.EnergyValue >= 0);
        }


    }
}
