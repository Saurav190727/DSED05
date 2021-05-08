using DSED05.Business;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTests
{
    [TestClass]
    public class ClassTests
    {
        [TestMethod]
        public void RunningFactoryToCreatePunters()
        {
            Punter[] myPunters = new Punter[3];
            for (int i = 0; i < 3; i++)
            {
                myPunters[i] = Factory.GetAPunter(i);
            }
            Assert.IsTrue(myPunters[0].name == "Jack" && myPunters[1].name == "Jeremy" && myPunters[2].name == "Vaughn");
        }
    }
}
