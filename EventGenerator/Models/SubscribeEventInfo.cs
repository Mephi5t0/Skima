using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventGenerator.Models
{
    public class SubscribeEventInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Имя пользователя
        /// </summary>
        [BsonElement("FirstName")]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [BsonElement("LastName")]
        public string LastName { get; set; }
        
        
        /// <summary>
        /// Почта пользователя
        /// </summary>
        [BsonElement("Email")]
        public string Email { get; set; }
        
        /// <summary>
        /// Название марафона
        /// </summary>
        [BsonElement("Title")]
        public string Title { get; set; }

        /// <summary>
        /// Описание марафона
        /// </summary>
        [BsonElement("Description")]
        public string Description { get; set; }
        
        /// <summary>
        /// Дата создания entry
        /// </summary>
        [BsonElement("CreatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; }
        
        
        /// <summary>
        /// Событие обработано
        /// </summary>
        [BsonElement("IsChecked")]
        public bool IsChecked  { get; set; }
        
    }
}