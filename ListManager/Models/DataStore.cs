// Пространство имён моделей данных
namespace ListManager.Models;

/// <summary>
/// Хранилище данных
/// </summary>
public class DataStore
{
    /// <summary>
    ///  Версия модели данных
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// ВременнАя метка
    /// </summary>
    public DateTime TimeStamp { get; set; }

    /// <summary>
    /// Список типов списков приложения
    /// </summary>
    public List<ListKind> ListKinds { get; set; }

    /// <summary>
    /// Список списков покупок
    /// </summary>
    public List<ShoppingList> ShoppingLists { get; set; }

    /// <summary>
    /// Максимальный идентификатор списка покупок
    /// </summary>
    public int ShoppingListMaxId { get; set; }

    /// <summary>
    /// Максимальный идентификатор товара.
    /// </summary>
    public int ProductMaxId { get; set; }

    /// <summary>
    /// Список товаров из всех списоко покупок
    /// </summary>
    public List<Product> ProductList { get; set; }

    public DataStore()
    {
        Version = 1;
        TimeStamp = DateTime.Now;
        ShoppingListMaxId = 0;
        ProductMaxId = 0;
        ListKinds =
        [
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
        ];
        ShoppingLists = [];
        ProductList = [];

    }
}
