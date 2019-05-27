using System.ComponentModel;

namespace Client.Models.Maraphone
{
    public class MaraphoneInfoSearchQuery
    {
        /// <summary>
        /// Позиция, начиная с которой нужно производить поиск
        /// </summary>
        [DefaultValue(0)]
        public int? Offset { get; set; }

        /// <summary>
        /// Максимальеное количество активностей, которое нужно вернуть
        /// </summary>
        [DefaultValue(10)]
        public int? Limit { get; set; }
    }
}