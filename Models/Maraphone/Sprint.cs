using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Maraphone
{
    public class Sprint
    {        
        /// <summary>
        /// Номер спринта в марафоне
        /// </summary>
        [BsonElement("Number")]
        public int Number { get; set; }
        
        /// <summary>
        /// Название спринта
        /// </summary>
        [BsonElement("Title")]
        public string Title { get; set; }
        
        /// <summary>
        /// Описание спринта
        /// </summary>
        [BsonElement("Description")]
        public string Description { get; set; }
        
        /// <summary>
        /// Дата создания спринта
        /// </summary>
        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Индентификатор создателя спринта
        /// </summary>
        [BsonElement("CreatedBy")]
        public string CreatedBy { get; set; }
        
        /// <summary>
        /// Продолжительность спринта
        /// </summary>
        [BsonElement("Duration")]
        public TimeSpan Duration { get; set; }
        
        /// <summary>
        /// Коллекция задач
        /// </summary>
        [BsonElement("Tasks")]
        public Task.Task[] Tasks { get; set; }
    }
}