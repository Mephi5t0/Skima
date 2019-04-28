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
        [DataMember(IsRequired = true)]
        public string UserId { get; set; }

        /// <summary>
        /// Идентификатор активности
        /// </summary>
        [DataMember(IsRequired = true)]
        public string ActivityId { get; set; }

        /// <summary>
        /// Статус активности
        /// </summary>
        [DataMember(IsRequired = true)]
        public Status Status { get; set; }
    }
}