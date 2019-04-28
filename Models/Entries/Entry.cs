using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Entries
{
    public class Entry
    {
        /// <summary>
        /// Идентификатор entry
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        [BsonElement("UserId")]
        public string UserId { get; set; }

        /// <summary>
        /// Идентификатор активности
        /// </summary>
        [BsonElement("ActivityId")]
        public string ActivityId { get; set; }

        /// <summary>
        /// Дата создания entry
        /// </summary>
        [BsonElement("CreatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Статус активности
        /// </summary>
        [BsonElement("Status")]
        public Status Status { get; set; }
    }
}