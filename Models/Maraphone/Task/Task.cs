using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Maraphone.Task
{
    public class Task
    {
        /// <summary>
        /// Дата создания задачи
        /// </summary>
        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Идентификатор создателя задачи
        /// </summary>
        [BsonElement("CreatedBy")]
        public string CreatedBy { get; set; }
        
        /// <summary>
        /// Название задачи
        /// </summary>
        [BsonElement("Title")]
        public string Title { get; set; }
        
        /// <summary>
        /// Содержание задачи
        /// </summary>
        [BsonElement("ContentId")]
        public string ContentId { get; set; }
    }
}