using APIServiceLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarSimulatorApp.Tests
{
    [TestClass]
    public class RandomUserApiIntegrationTests
    {
        [TestMethod]
        public async Task GetOneDriver_ReturnsValidDriver()
        {
            var service = new APIService();
            var results = await service.GetOneDriver();

            Assert.IsNotNull(results, "Results should not be null");
            Assert.IsTrue(results.Results.Count > 0, "Results should contain at least one driver");

            var driver = results.Results[0];

            Assert.IsNotNull(driver.Name, "Driver.Name should not be null");
            Assert.IsFalse(string.IsNullOrEmpty(driver.Name.First), "Driver.Name.First should not be empty");
            Assert.IsFalse(string.IsNullOrEmpty(driver.Name.Last), "Driver.Name.Last should not be empty");

            Assert.IsNotNull(driver.Location, "Driver.Location should not be null");
            Assert.IsFalse(string.IsNullOrEmpty(driver.Location.City), "Driver.Location.City should not be empty");
            Assert.IsFalse(string.IsNullOrEmpty(driver.Location.Country), "Driver.Location.Country should not be empty");
        }
    }
}
