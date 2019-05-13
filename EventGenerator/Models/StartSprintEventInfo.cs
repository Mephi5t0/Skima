using System;
using Models.Maraphone.Task;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace EventGenerator.Repository
{
    public class StartSprintEventInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Номер спринта в марафоне
        /// </summary>
        [BsonElement("Number")]
        public int Number { get; set; }

                
        /// <summary>
        /// Дата начала спринта
        /// </summary>
        [BsonElement("StartAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime StartAt { get; set; }

        /// <summary>
        /// Описание спринта
        /// </summary>
        [BsonElement("Description")]
        public string Description { get; set; }


        /// <summary>
        /// Коллекция задач
        /// </summary>
        [BsonElement("Tasks")]
        public Task[] Tasks { get; set; }

        /// <summary>
        /// Дата создания спринта
        /// </summary>
        [BsonElement("CreatedAt")]
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Продолжительность спринта
        /// </summary>
        [BsonElement("Duration")]
        public TimeSpan Duration { get; set; }
        
        
        /// <summary>
        /// Событие обработано
        /// </summary>
        [BsonElement("IsChecked")]
        public bool IsChecked  { get; set; }
    }
}