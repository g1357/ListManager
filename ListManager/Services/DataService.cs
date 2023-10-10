using ListManager.Models;
using System.Text.Json;
using System.Xml.Linq;

// Пространство имён моделей данных
namespace ListManager.Services;

public class DataService : IDataService
{
    // Каталог для хранения данных приложения
    private static readonly string dir = FileSystem.Current.AppDataDirectory;

    // Имя файла для сохранения данных
    private static readonly string fileName = @"ListManager.data";

    // Путь к файлу данных
    private readonly string filePath = Path.Combine(dir, fileName);

    /// <summary>
    /// Данные нашего приложения
    /// </summary>
    private DataStore Data { get; set; }

    /// <summary>
    /// Получить следующий идентификатор списка покупок
    /// </summary>
    private int NextShoppingListId => ++Data.ShoppingListMaxId;

    /// <summary>
    /// Получить следующий идентификатор продукта
    /// </summary>
    private int NextProductId => ++Data.ProductMaxId;

    public DataService()
    {
        Data = new DataStore(); 
    }

    /// <summary>
    /// Установить типы списков.
    /// </summary>
    private void InitListKinds()
    {
        if (Data.ListKinds == null)
        {
            Data.ListKinds = new List<ListKind>
            {
                new ListKind
                {
                    Id = 1,
                    Name = "Список покупок",
                    Description = "Список, содержит товары, которые необходимо купить"
                },
                new ListKind
                {
                    Id = 2,
                    Name = "Список дел",
                    Description = "Список, содержит текущие дела, которые необходимо сделать"
                }
            };
        }
    }

    #region Реализация интерфейса IDataService
    /// <summary>
    /// Получить перечень поддерживаемых приложением типов списков.
    /// </summary>
    /// <returns>Перечень типов списков</returns>
    public IEnumerable<ListKind> GetListTypes()
    {
        return Data.ListKinds;
    }

    /// <summary>
    /// Получить перечень списков покупок.
    /// </summary>
    /// <returns>Перечень списков покупок</returns>
    public IEnumerable<ShoppingList> GetShoppingLists()
    {
        return Data.ShoppingLists;
    }

    /// <summary>
    /// Создать список покупок.
    /// </summary>
    /// <param name="name">наименование списка покупок</param>
    /// <param name="description">Краткое описание списка покупок</param>
    /// <returns>Идентификатор списка покупок</returns>
    public int CreateShoppingList(string name, string description)
    {
        // Создать новый список покупок
        var list = new ShoppingList
        {
            ListKindId = 1,
            Id = NextShoppingListId,
            Name = name, 
            Description = description 
        };
        // Добавить список покупок к пепечню списков покупок
        Data.ShoppingLists.Add(list);
        return list.Id;
    }

    /// <summary>
    /// Удалить список покупок.
    /// </summary>
    /// <param name="listId">Идентификатор списка покупок</param>
    /// <returns>Признак успешности удаления списка покупок</returns>
    public bool DeleteShoppingList(int listId)
    {
        var item = Data.ShoppingLists.FirstOrDefault(x => x.Id == listId);  
        if (item == null)
        {
            return false;
        }
        else
        {
            Data.ShoppingLists.Remove(item);
            return true;
        }
    }

    /// <summary>
    /// Получить данные о заголовке списка покупок.
    /// </summary>
    /// <param name="listId">Идентификатор списка покупок</param>
    /// <returns>Заголовок списка покупок или null, если список не найден</returns>
    public ShoppingList? GetShoppingList(int listId)
    {
        // Вернуть список покупок с заданныи идентификатором или null
        // при его отсутствии
        return Data.ShoppingLists.FirstOrDefault(x => x.Id == listId);
    }

    /// <summary>
    /// Обновить данные заголовка списка покупок.
    /// </summary>
    /// <param name="shoppingList">Данные заголовка списка покупок.</param>
    /// <returns>Признак успешности обновления</returns>
    public bool UpdateShoppingList(ShoppingList shoppingList)
    {
        // Получить элемент по его идентификатору или null
        var item = Data.ShoppingLists.FirstOrDefault(x => 
            x.Id == shoppingList.Id);
        // Если эдемент не найден, то
        if (item == null)
        {
            // Вернуть ложь - неудачное выполнение опрерации
            return false;
        }
        else // Если элементнайдет, то
        {
            // Обновить значения
            item.Name = shoppingList.Name;
            item.Description = shoppingList.Description;
            // Вернуть истину - успешное выполнение операции
            return true;
        }
    }

    /// <summary>
    /// Очистить список продуктов в списке покупок.
    /// </summary>
    /// <param name="listId">Идентификатор списка покупок</param>
    /// <returns>Признак успешности очистки</returns>
    public bool ClearShoppingList(int listId)
    {
        // Получить список всех товаров списка покупок с заданным идентификатором
        var list = Data.ProductList.Where(prod => prod.ListId == listId);
        // Для каждого элеимента списка
        foreach (var item in list)
        {
            // Удалить элемент из перечня продуктов
            Data.ProductList.Remove(item);
        }
        // Вернуть истину
        return true;
    }

    /// <summary>
    /// Получить список продуктов заданного списка покупок.
    /// </summary>
    /// <param name="listId">Идентификатор списка покупок</param>
    /// <returns>Список продуктов</returns>
    public IEnumerable<Product> GetProductList(int listId)
    {
        // Вернуть список всех товаров списка покупок с заданным идентификатором
        return Data.ProductList.Where(prod => prod.ListId == listId);
    }

    /// <summary>
    /// Добавить товар в список покупок.
    /// </summary>
    /// <param name="listId">Идентификатор списка покупок</param>
    /// <param name="name">Наиметование товара</param>
    /// <param name="description">Краткое описание товара</param>
    /// <param name="qty">Количество товара</param>
    /// <returns>признак успешности добавления товара к списку</returns>
    public bool AddProduct(int listId, string name, string description, int qty)
    {
        // Создать новый продукт
        var prod = new Product
        {
            ListId = listId,    // Идентификатор списка
            Id = NextProductId, // Очередной идентификатор продукта
            Name = name,        // Наименование продукта
            Description = description,  // Краткое описание продукта
            Qty = qty,   // Количество товара
            Marked = false  // Отсеченность продукта
        };
        // Добавить продукт в общий список продуктов
        Data.ProductList.Add(prod);
        // Вернуть истину
        return true;
    }

    /// <summary>
    /// Удалить продукт из списка покупок.
    /// </summary>
    /// <param name="prodId">Идентификатор продукта</param>
    /// <returns>Признак успешности удаления товара</returns>
    public bool DeleteProduct(int prodId)
    {
        // Найти продукт по идентификатору
        var item = Data.ProductList.FirstOrDefault(x => x.Id == prodId);
        // Если продукт не найден, то
        if (item == null)
        {
            // Вернуть ложь
            return false;
        }
        else // Если продукт найден, то
        {
            // Удалить продукт из общего списка продуктов
            Data.ProductList.Remove(item);
            // Вернуть истину
            return true;
        }
    }

    /// <summary>
    /// Получить данные о продукте.
    /// </summary>
    /// <param name="prodId">Идентификатор продукта</param>
    /// <returns>Данные о продукте</returns>
    public Product? GetProduct(int prodId)
    {
        // Вернуть продукт по идентификатору
        return Data.ProductList.FirstOrDefault(x => x.Id == prodId);
    }

    /// <summary>
    /// Обновить данные о продукте.
    /// </summary>
    /// <param name="product">Данные о продукте</param>
    /// <returns>Признак успешности обновления данных</returns>
    public bool UpdateProduct(Product product)
    {
        // Получить элемент по его идентификатору или null
        var item = Data.ProductList.FirstOrDefault(x =>
            x.Id == product.Id);
        // Если эдемент не найден, то
        if (item == null)
        {
            // Вернуть ложь - неудачное выполнение опрерации
            return false;
        }
        else // Если элементнайдет, то
        {
            // Обновить значения
            item.Name = product.Name;
            item.Description = product.Description;
            item.Qty = product.Qty;
            item.Marked = product.Marked;
            // Вернуть истину - успешное выполнение операции
            return true;
        }
    }
    #endregion Реализация интерфейса IDataService

    public async Task<bool> SaveData()
    {
        try
        {
            using FileStream outputStream = System.IO.File.OpenWrite(filePath);
            using StreamWriter streamWriter = new StreamWriter(outputStream);

            string json = JsonSerializer.Serialize<DataStore>(Data);

            await streamWriter.WriteAsync(json);
            return true;
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Восстановить данные из локального хранилища.
    /// </summary>
    /// <returns>Признак успешного восстановления данных</returns>
    public async Task<bool> RestoreData()
    {
        try
        {
            // Если файл существует, то
            if (File.Exists(filePath))
            {
                using Stream fileStream = File.OpenRead(filePath);
                using StreamReader reader = new StreamReader(fileStream);
                var json = await reader.ReadToEndAsync();

                var data = JsonSerializer.Deserialize<DataStore>(json);
                if (data != null)
                {
                    Data = data;
                    data = null;
                }
                return true;
            }
            else // Если файл отсутствует, то
            {
                DataSeed();
                //InitListKinds();
                return true;
            }
        }
        catch (Exception ex)
        {
            Console.Write(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Заполнить демонстрационные данные
    /// </summary>
    public void DataSeed()
    {
        Data = new DataStore();
        var listId_1 = CreateShoppingList("МЕТРО", "Еженедельные покупки в МЕТРО");
        AddProduct(listId_1, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_1, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_1, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_1, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_1, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_1, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        var listId_2 = CreateShoppingList("Перекрёсток", "Разовые покупки в Перекрёстке");
        AddProduct(listId_2, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_2, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_2, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_2, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_2, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_2, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        var listId_3 = CreateShoppingList("На дачу", "Покупки для дачи");
        AddProduct(listId_3, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_3, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_3, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_3, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_3, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_3, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        var listId_4 = CreateShoppingList("Крупные", "Крупные покупки");
        AddProduct(listId_4, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_4, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_4, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_4, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_4, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
        AddProduct(listId_4, "Кефир 1", "Кефир Молочная культура в зелёном стаканчике", 4);
    }
}
