namespace ListManager.MSTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
        }

        [TestMethod()]
        [DeploymentItem("testFile1.xml")]
        public void ConstructorTest()
        {
            string file = "testFile1.xml";
            //var filePath = Path.Combine(FileSystem.AppDataDirectory, "ListManager.data");
           
            Assert.IsFalse(File.Exists(file), "deployment failed: " + file +
                " did not get deployed");
        }

    }
}