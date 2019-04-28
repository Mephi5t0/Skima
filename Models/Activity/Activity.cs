using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Activity
{
    public class Activity
    {
        /// <summary>
        /// Идентификатор активности
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Id марафона, к которому принадлежит активность
        /// </summary>
        [BsonElement("MaraphoneId")]
        public string MaraphoneId { get; set; }

        /// <summary>
        /// Тэги
        /// </summary>
        [BsonElement("Tags")]
        public string[] Tags { get; set; }

        /// <summary>
        /// Идентификатор создателя активности
        /// </summary>
        [BsonElement("CreatedBy")]
        public string CreatedBy { get; set; }
        
        /// <summary>
        /// Идентификаторы экспертов
        /// </summary>
        [BsonElement("Experts")]
        public string[] Experts { get; set; }
        
        /// <summary>
        /// Статус активности
        /// </summary>
        [BsonElement("Status")]
        public Status Status { get; set; }

        /// <summary>
        /// Дата создания активности
        /// </summary>
        [BsonElement("CreatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Дата начала активности
        /// </summary>
        [BsonElement("StartAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartAt { get; set; }
        
        /// <summary>
        /// Дата окончания активности
        /// </summary>
        [BsonElement("EndAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime EndAt { get; set; }
    }
}