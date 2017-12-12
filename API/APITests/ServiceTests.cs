using Microsoft.VisualStudio.TestTools.UnitTesting;
using API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.Tests
{
    [TestClass()]
    public class ServiceTests
    {
        [TestMethod()]
        public void CreateUserTest()
        {
            var service = new API.Service();
            var newStaffMember = new Staff() { Email = "testMail" , Full_Name = "FullNameTest", Username = "usertest", Password = "pass"};
            Assert.AreEqual("OK", service.CreateStaffUser(newStaffMember));
        }
        [TestMethod()]
        public void LogInTest()
        {
            var service = new API.Service();
            var dataPack = new DataPack();
            var staff = service.Login("usertest", "pass");
            if (staff == null) Assert.Fail();
            Assert.AreEqual("usertest", staff.Username);
        }
        [TestMethod()]
        public void SaveDataTest()
        {
            var service = new API.Service();
            var dataPack = new DataPack() { Humidity = 54, IR1 = 1042, IR2 = 34, Temperature = 25, Name = "testSensor" };
            Assert.AreEqual("OK", service.SaveData(dataPack));
        }
        [TestMethod()]
        public void GetDataByPeriodTest()
        {
            var service = new API.Service();
            var dataPack = new DataPack() { Humidity = 54, IR1 = 1042, IR2 = 34, Temperature = 25, Name = "testSensor" };
            Assert.AreEqual("OK", service.SaveData(dataPack));
        }
    }
}