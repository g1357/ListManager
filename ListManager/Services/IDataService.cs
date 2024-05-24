using ListManager.Models;

// Пространство имён сервисов
namespace ListManager.Services;

/// <summary>
/// Интерфейс работы с данными
/// </summary>
public interface IDataService
{
    #region Операции с типами списков
    /// <summary>
    /// Получить перечень поддерживаемых приложением типов списков.
    /// </summary>
    /// <returns>Перечень типов списков</returns>
    public IEnumerable<ListKind> GetListTypes();
    #endregion Операции с типами списков

    #region Операции со списками покупок
    /// <summary>
    /// Получить перечень списков покупок.
    /// </summary>
    /// <returns>Перечень списков покупок</returns>
    public IEnumerable<ShoppingList> GetShoppingLists();

    /// <summary>
    /// Создать список покупок.
    /// </summary>
    /// <param name="name">наименование списка покупок</param>
    /// <param name="description">Краткое описание списка покупок</param>
    /// <returns>Идентификатор списка покупок</returns>
    public int CreateShoppingList(string name, string description);

    /// <summary>
    /// Удалить список покупок.
    /// </summary>
    /// <param name="listId">Идентификатор списка покупок</param>
    /// <returns>Признак успешности удаления списка покупок</returns>
    public bool DeleteShoppingList(int listId);

    /// <summary>
    /// Получить данные о заголовке списка покупок.
    /// </summary>
    /// <param name="listId">Идентификатор списка покупок</param>
    /// <returns>Заголовок списка покупок или null, если список не найден</returns>
    public ShoppingList? GetShoppingList(int listId);

    /// <summary>
    /// Обновить данные заголовка списка покупок.
    /// </summary>
    /// <param name="shoppingList">Данные заголовка списка покупок.</param>
    /// <returns>Признак успешности обновления</returns>
    public bool UpdateShoppingList(ShoppingList shoppingList);

    /// <summary>
    /// Очистить список продуктов в списке покупок.
    /// </summary>
    /// <param name="listId">Идентификатор списка покупок</param>
    /// <returns>Признак успешности очистки</returns>
    public bool ClearShoppingList(int listId);
    #endregion Операции со списками покупок

    #region Операции с товарами в списке покупок
    
    /// <summary>
    /// Получить список продуктов заданного списка покупок.
    /// </summary>
    /// <param name="listId">Идентификатор списка покупок</param>
    /// <returns>Список продуктов</returns>
    public IEnumerable<Product> GetProductList(int listId);

    /// <summary>
    /// Добавить товар в список покупок.
    /// </summary>
    /// <param name="listId">Идентификатор списка покупок</param>
    /// <param name="name">Наиметование товара</param>
    /// <param name="description">Краткое описание товара</param>
    /// <param name="qty">Количество товара</param>
    /// <returns>признак успешности добавления товара к списку</returns>
    public bool AddProduct(int listId,
        string name, string description, int qty, string? unit);

    /// <summary>
    /// Добавить товар в список покупок.
    /// </summary>
    /// <param name="newProduct">Добавляемый товар. 
    /// Поле Id будет переопределено.</param>
    /// <returns>признак успешности добавления товара к списку</returns>
    public bool AddProduct(Product newProduct);

    /// <summary>
    /// Удалить продукт из списка покупок.
    /// </summary>
    /// <param name="prodId">Идентификатор продукта</param>
    /// <returns>Признак успешности удаления товара</returns>
    public bool DeleteProduct(int prodId);

    /// <summary>
    /// Получить данные о продукте.
    /// </summary>
    /// <param name="prodId">Идентификатор продукта</param>
    /// <returns>Данные о продукте</returns>
    public Product? GetProduct(int prodId);

    /// <summary>
    /// Обновить данные о продукте.
    /// </summary>
    /// <param name="product">Данные о продукте</param>
    /// <returns>Признак успешности обновления данных</returns>
    public bool UpdateProduct(Product product);
    #endregion Операции с товарами в списке покупок

    /// <summary>
    /// Очистить все данные
    /// </summary>
    public void ClearAllData();

    /// <summary>
    /// Локально охранить данные приложения
    /// </summary>
    /// <returns>Признак успешности сохранения данных</returns>
    public Task<bool> SaveData();

    /// <summary>
    /// Восстановить данные из локального хранилища.
    /// </summary>
    /// <returns>Признак успешного восстановления данных</returns>
    public Task<bool> RestoreData();

    /// <summary>
    /// Заполнить демонстрационные данные
    /// </summary>
    public void DataSeed();
}
