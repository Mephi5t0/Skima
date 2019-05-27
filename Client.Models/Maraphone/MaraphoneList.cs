using System.Collections.Generic;

namespace Client.Models.Maraphone
{
    /// <summary>
    ///  Список c описанием марафонов
    /// </summary>
    public class MaraphoneList
    {
        /// <summary>
        /// Краткая информация по марафонам
        /// </summary>
        public IReadOnlyCollection<Maraphone> Maraphones { get; set; }
    }
}