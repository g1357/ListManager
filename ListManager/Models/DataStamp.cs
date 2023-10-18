using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ListManager.Models;

public class DataStamp
{
    /// <summary>
    ///  Версия модели данных
    /// </summary>
    public int Version { get; set; }

    /// <summary>
    /// ВременнАя метка
    /// </summary>
    public DateTime TimeStamp { get; set; }
}
