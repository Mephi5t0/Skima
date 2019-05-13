using System.Runtime.Serialization;

namespace Client.Models.Entry
{
    public class Entry
    {
        /// <summary>
        /// Идентификатор entry
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Идентификатор активности
        /// </summary>
        public string ActivityId { get; set; }

        /// <summary>
        /// Статус записи на марафон
        /// </summary>
        public Status Status { get; set; }
    }
}