namespace ListManager.xUnit
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        [Fact]
        //[DeploymentItem("testFile1.xml")]
        public void ConstructorTest()
        {
            string file = "testFile1.xml";
            var filePath = Path.Combine(Microsoft.Maui.Storage.FileSystem.AppDataDirectory, "ListManager.data");

            Assert.False(File.Exists(file), "deployment failed: " + file +
                " did not get deployed");
        }

    }
}