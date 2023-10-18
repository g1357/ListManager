using ListManager.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.MSTests;

[TestClass]
public class DataServiceTests
{
    [TestMethod]
    public void ConstructorTest()
    {
        // Arrange
        // Act
        var dt1 = DateTime.Now;
        var dataService = new DataService();
        var dt2 = DateTime.Now;
        // Assert
        var data = dataService.GetData;
        Assert.AreEqual(data.Version, 1);
        Assert.IsTrue(data.TimeStamp > dt1);
        Assert.IsTrue(data.TimeStamp < dt2);
        Assert.AreEqual(data.ShoppingListMaxId, 0);
        Assert.AreEqual(data.ProductMaxId, 0);
        Assert.AreNotEqual(data.ListKinds, null);
        Assert.AreNotEqual(data.ShoppingLists, null);
        Assert.AreNotEqual(data.ProductList, null);
        Assert.AreEqual(data.ListKinds.Count, 2);
        Assert.AreEqual(data.ListKinds[0].Id, 1);
        Assert.AreEqual(data.ListKinds[0].Name, "Список покупок");
        Assert.AreEqual(data.ListKinds[0].Description, 
            "Список, содержит товары, которые необходимо купить");
        Assert.AreEqual(data.ListKinds[1].Id, 2);
        Assert.AreEqual(data.ListKinds[1].Name, "Список дел");
        Assert.AreEqual(data.ListKinds[1].Description,
            "Список, содержит текущие дела, которые необходимо сделать");
    }
}
