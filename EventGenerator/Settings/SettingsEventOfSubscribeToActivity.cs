using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventGenerator
{
    public class SettingsEventOfSubscribeToActivity
    {
        
        /// <summary>
        /// Идентификатор entry
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        /// <summary>
        /// Дата создания entry
        /// </summary>
        [BsonElement("CreatedAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime CreatedAt { get; set; }
    }
}