using ListManager.Models;
using System.Diagnostics;
using System.Text.Json;
using System.Xml.Linq;

// Пространство имён моделей данных
namespace ListManager.Services;

public class DataService : IDataService
{
    // Каталог для хранения данных приложения
    private static readonly string dir = FileSystem.AppDataDirectory;

    // Имя файла для сохранения данных
    private static readonly string dataFileName = @"ListManager.lmd";

    // Имя файла для сохранения штампа
    private static readonly string stampFileName = @"ListManager.lms";

    // Путь к файлу данных
    private readonly string dataFilePath;

    // Путь к файлу штампа
    private readonly string stampFilePath;

    // Признак изменения данных (требуется их сохранение)
    private bool dataChanged = false;

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
        dataFilePath = Path.Combine(dir, dataFileName);
        stampFilePath = Path.Combine(dir, stampFileName);
        
        Data = new DataStore();
        //DataSeed(); 
    }

    /// <summary>
    /// Установить типы списков.
    /// </summary>
    private void InitListKinds()
    {
        // Если типы списков не заданы, то
        if (Data.ListKinds == null)
        {
            // Создать список типов списков
            Data.ListKinds = new List<ListKind>
            {
                new() {
                    Id = 1,
                    Name = "Список покупок",
                    Description = "Список, содержит товары, которые необходимо купить"
                },
                new() {
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
        // Весрнуть перечень типов списков, поддерживаемых приложением
        return Data.ListKinds;
    }

    /// <summary>
    /// Получить перечень списков покупок.
    /// </summary>
    /// <returns>Перечень списков покупок</returns>
    public IEnumerable<ShoppingList> GetShoppingLists()
    {
        // Вернуть перечень списков покупок
        return Data.ShoppingLists;
    }

    /// <summary>
    /// Создать список покупок.
    /// </summary>
    /// <param name="name">наименование списка покупок</param>
    /// <param name="description">Краткое описание списка покупок</param>
    /// <returns>Идентификатор списка покупок</returns>
    public int CreateShoppingList(string name, string? description = null)
    {
        // Создать новый список покупок
        var list = new ShoppingList
        {
            ListKindId = 1, // Тип списка: Список покупок
            Id = NextShoppingListId, // Идентификатор списка покупок
            Name = name, // Наименование списка покупок
            Description = description // Краткое описание списка покупок
        };
        // Добавить список покупок к пепечню списков покупок
        Data.ShoppingLists.Add(list);
        // Установить признак изменения данных
        dataChanged = true;
        // Вернуть идентификатор созданного списка покупок
        return list.Id;
    }

    /// <summary>
    /// Удалить список покупок.
    /// </summary>
    /// <param name="listId">Идентификатор списка покупок</param>
    /// <returns>Признак успешности удаления списка покупок</returns>
    public bool DeleteShoppingList(int listId)
    {
        // Получить список по его идентификатору или null
        var item = Data.ShoppingLists.FirstOrDefault(x => x.Id == listId);  
        // Если список с заданым идентификатором не найден, то
        if (item == null)
        {
            // Вернуть неуспешное выполнение опрерации
            return false;
        }
        else
        {
            // Удалить найденный элемент из перечня списков покупок
            Data.ShoppingLists.Remove(item);
            // Удалить продукты из заданного списка покупок
            ClearShoppingList(listId);
            // Установить признак изменения данных
            dataChanged = true;
            // Вернуть успешность выполнения операции
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
            // Если наименование изменилось, то
            if (item.Name != shoppingList.Name)
            {
                // Установить новое наименование списка
                item.Name = shoppingList.Name;
                // Установить признак изменения данных
                dataChanged = true;
            }
            // Если краткое описание изменилось, то
            if (item.Description != shoppingList.Description)
            {
                // Установить новове краткое описание
                item.Description = shoppingList.Description;
                // Установить признак изменения данных
                dataChanged = true;
            }
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
        // Если товары есть, то
        if (list != null)
        {
            // Для каждого элеимента списка
            foreach (var item in list)
            {
                // Удалить элемент из перечня продуктов
                Data.ProductList.Remove(item);
            }
            // Установить признак изменения данных
            dataChanged = true;
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
    /// <param name="unit">Единица измерения количества товара</param>
    /// <returns>признак успешности добавления товара к списку</returns>
    public bool AddProduct(int listId, string name, string description, int qty, string? unit = null)
    {
        // Создать новый продукт
        var prod = new Product
        {
            ListId = listId,    // Идентификатор списка
            Id = NextProductId, // Очередной идентификатор продукта
            Name = name,        // Наименование продукта
            Description = description,  // Краткое описание продукта
            Qty = qty,   // Количество товара
            Unit = unit,  // Единица измерения количества
            Marked = false  // Отсеченность продукта
        };
        // Добавить продукт в общий список продуктов
        Data.ProductList.Add(prod);
        // Установить признак изменения данных
        dataChanged = true;
        // Вернуть истину
        return true;
    }

    /// <summary>
    /// Добавить товар в список покупок.
    /// </summary>
    /// <param name="newProduct">Добавляемый товар. 
    /// Поле Id будет переопределено.</param>
    /// <returns>признак успешности добавления товара к списку</returns>
    public bool AddProduct(Product newProduct)
    {
        // Проверить допустимость типа списка
        var item = Data.ShoppingLists.FirstOrDefault(x =>
            x.Id == newProduct.ListId);
        if (item == null)
        {
            return false;
        }
        newProduct.Id = NextProductId;
        // Добавить продукт в общий список продуктов
        Data.ProductList.Add(newProduct);
        // Установить признак изменения данных
        dataChanged = true;
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
            // Установить признак изменения данных
            dataChanged = true;
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
        else // Если элемент найдет, то
        {
            // Если наименование изменилось, то
            if (item.Name != product.Name)
            {
                // Установить новое значение
                item.Name = product.Name;
                // Установить признак изменения данных
                dataChanged = true;
            }
            // Если краткое описание изменилось, то
            if (item.Description != product.Description)
            {
                // Установить новое значение
                item.Description = product.Description;
                // Установить признак изменения данных
                dataChanged = true;
            }
            if (item.Qty != product.Qty)
            {
                item.Qty = product.Qty;
                // Установить признак изменения данных
                dataChanged = true;
            }
            if (item.Marked != product.Marked)
            {
                item.Marked = product.Marked;
                // Установить признак изменения данных
                dataChanged = true;
            }
            // Вернуть истину - успешное выполнение операции
            return true;
        }
    }

    /// <summary>
    /// Очистить все данные
    /// </summary>
    public void ClearAllData()
    {
        // Создать объект для хранилища данных
        Data = new DataStore();
        // Добавить типы списков
        InitListKinds();
        // Установить признак изменения данных
        dataChanged = true;
    }
    #endregion Реализация интерфейса IDataService

    /// <summary>
    /// Обновить значение, если оно изменилось 
    /// и установить признак изменения данных
    /// </summary>
    /// <typeparam name="T">Тип аргументов</typeparam>
    /// <param name="value">Старое значение</param>
    /// <param name="newValue">Новое значение</param>
    private void UpdateValue<T>(T value, T newValue)
    {
        // Если значение не задано, то вернуться
        if (value == null) return;
        // Если значение изменилось, то
        if (!value.Equals(newValue))
        {
            // Установить новое значение
            value = newValue;
            // Установить признак изменения данных
            dataChanged = true;
        }
    }

    /// <summary>
    /// Локально охранить данные приложения
    /// </summary>
    /// <returns>Признак успешности сохранения данных</returns>
    public async Task<bool> SaveData()
    {
        Debug.WriteLine("==== SaveData is called!");
        // Если данные не изменяль, то выйти
        if (!dataChanged) return true;
        try // Блок с отслеживанием возникновения исключений
        {
            // Открыти заданный файл для записи и создать поток
            //using FileStream outputStream = File.OpenWrite(dataFilePath);
            // Создать "записыватель" потока
            //using StreamWriter streamWriter = new StreamWriter(outputStream, false);
            using StreamWriter streamWriter = File.CreateText(dataFilePath);

            // Сериализовать данные приложения в строку в формате JSON
            string json = JsonSerializer.Serialize<DataStore>(Data);

            // Записать данные в файл
            await streamWriter.WriteAsync(json);
            // Сросить признак изменения данных
            dataChanged = false;
            // Вернуть признак успешности восстановления данных
            return true;
        }
        catch (Exception ex) // если возникло исключение, то
        {
            // Выдать сообщение рб ошибке
            Console.Write(ex.Message);
            // Вернуть признак неуспешности восстановления данных
            return false;
        }
    }

    /// <summary>
    /// Восстановить данные из локального хранилища.
    /// </summary>
    /// <returns>Признак успешного восстановления данных</returns>
    public async Task<bool> RestoreData()
    {
        try // Блок с отслеживанием возникновения исключений
        {
            if (File.Exists(stampFilePath))
            {
                // Открыти заданный файл для чтения и создать поток
                using Stream fileStream = File.OpenRead(dataFilePath);
                // Создать считыватель потока
                using StreamReader reader = new StreamReader(fileStream);
                // Считать все текстовые данные из файла в строку
                var json = await reader.ReadToEndAsync();

                // Десериализовать JSON в заданный объект
                var dataStamp = JsonSerializer.Deserialize<DataStamp>(json);
            }
                // Если файл существует, то
            if (File.Exists(dataFilePath))
            {
                // Открыти заданный файл для чтения и создать поток
                using Stream fileStream = File.OpenRead(dataFilePath);
                // Создать считыватель потока
                using StreamReader reader = new StreamReader(fileStream);
                // Считать все текстовые данные из файла в строку
                var json = await reader.ReadToEndAsync();

                // Десериализовать JSON в заданный объект
                var data = JsonSerializer.Deserialize<DataStore>(json);
                // Если данные успешно десериализовались в объект, то
                if (data != null)
                {
                    // Сохранить считанные данные в локальную переменную
                    Data = data;
                    // Очистить рабочую переменную
                    data = null;
                }
            }
            else // Если файл отсутствует, то
            {
                // Заполнить локальную переменную, хранящую данные,
                // демонстрационными данными
                DataSeed();
            }
            // Сросить признак изменения данных
            dataChanged = false;
            // Вернуть признак успешности восстановления данных
            return true;
        }
        catch (Exception ex) // если возникло исключение, то
        {
            // Выдать сообщение рб ошибке
            Console.Write(ex.Message);
            // Вернуть признак неуспешности восстановления данных
            return false;
        }
    }

    /// <summary>
    /// Заполнить демонстрационные данные
    /// </summary>
    public void DataSeed()
    {
        // Создать новый список покупок
        var listId_1 = CreateShoppingList("МЕТРО", "Еженедельные покупки в МЕТРО");
        // Добавить в список товары
        AddProduct(listId_1, "Кефир", "Кефир Молочная культура в зелёном стаканчике", 4, "шт.");
        AddProduct(listId_1, "Масло сливочное", "масло вологодское из Вологды", 2, "пач.");
        AddProduct(listId_1, "Сметена 20%", "Домик в деревне", 1, "уп.");
        AddProduct(listId_1, "Багет длинный", "Выпечка МЕТРО", 1, "шт.");
        AddProduct(listId_1, "Колбава варёная", "В натуральной оболочке, уп.", 1, "шт.");
        AddProduct(listId_1, "Помидоры", "Розовые, кг.", 1, "кг.");
        // Создать новый список покупок
        var listId_2 = CreateShoppingList("Перекрёсток", "Разовые покупки в Перекрёстке");
        // Добавить в список товары
        AddProduct(listId_2, "Печенье овсянное", "уп.", 1, "уп.");
        AddProduct(listId_2, "Молоко топлёное можайское 4", "бут.", 4, "бут.");
        AddProduct(listId_2, "Огурцы свежие", "маленькие, уп.", 2, "уп.");
        AddProduct(listId_2, "Блины", "уп.", 1, "уп");
        AddProduct(listId_2, "Творог", "Жирный, уп.", 1);
        AddProduct(listId_2, "Хлеб чёрный", "Нарезка, уп.", 1, "шт.");
        // Создать новый список покупок
        var listId_3 = CreateShoppingList("На дачу", "Покупки для дачи");
        // Добавить в список товары
        AddProduct(listId_3, "Угли", "Упаковка 10 кг., уп.", 1, "пак.");
        AddProduct(listId_3, "Рожиг", "Жидкость для розжига парафиновая, бут.", 2, "бут.");
        AddProduct(listId_3, "Решётка для гриля", "Большая., шт.", 1, "шт.");
        AddProduct(listId_3, "Шашалык", "Свиной., уп.", 2, "Эуп");
        AddProduct(listId_3, "Вино красное", "Красное сухое вино., бут.", 2, "бут.");
        AddProduct(listId_3, "Боржоми", "0.75 л,х 6 бут.", 4, "уп.");
        // Создать новый список покупок
        var listId_4 = CreateShoppingList("Крупные", "Крупные покупки");
        // Добавить в список товары
        AddProduct(listId_4, "Газонокосмлка", "Бензиновая не менее 50 см., самоходная", 1, "шт.");
        AddProduct(listId_4, "Ручной триммер", "Электрический аккумуляторный с аккумуляторами и зарядным устройством", 1, "шт.");
        AddProduct(listId_4, "Насос фикальный", "Лучше Керхер", 1, "шт.");
        AddProduct(listId_4, "Шланг к насосу", "20 м.", 1, "шт.");
    }

    /// <summary>
    ///  Получить хранилище данных.
    /// </summary>
    public DataStore GetData => Data;
}
