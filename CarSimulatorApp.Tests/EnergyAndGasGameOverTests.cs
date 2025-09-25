using DataLogicLibrary.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSimulatorApp.Tests
{
    [TestClass]
    public class EnergyAndGasGameOverTests
    {
        [TestMethod]
        public void Energy_Zero_DoesNotGoNegative()
        {
            var status = new StatusDTO { EnergyValue = 0, GasValue = 10 };
            var sut = HungerAndGameOverTests.BuildSut();

            var result = sut.DecreaseStatusValues(3, status);

            Assert.AreEqual(0, result.EnergyValue);
        }

        [TestMethod]
        public void Gas_Zero_DoesNotGoNegative()
        {
            var status = new StatusDTO { EnergyValue = 10, GasValue = 0 };
            var sut = HungerAndGameOverTests.BuildSut();

            var result = sut.DecreaseStatusValues(3, status);

            Assert.AreEqual(0, result.GasValue);
        }
    }
}
