using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventGenerator
{
    public class RegistrationEventInfo
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
        /// Дата регистрации пользователя
        /// </summary>
        [BsonElement("RegisteredAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime RegisteredAt { get; set; }
        
        
        /// <summary>
        /// Событие обработано
        /// </summary>
        [BsonElement("IsChecked")]
        public bool IsChecked  { get; set; }

    }
}