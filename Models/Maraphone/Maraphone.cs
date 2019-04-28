using System;
using Client.Models.Maraphone;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Maraphone
{
    public class Maraphone
    {
        /// <summary>
        /// Идентификатор марафона
        /// </summary>
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        /// <summary>
        /// Название марафона
        /// </summary>
        [BsonElement("TagTitles")]
        public string Title { get; set; }

        /// <summary>
        /// Описание марафона
        /// </summary>
        [BsonElement("Description")]
        public string Description { get; set; }
        
        /// <summary>
        /// Спринты марафона
        /// </summary>
        [BsonElement("Sprints")]
        public Sprint[] Sprints { get; set; }
        
        /// <summary>
        /// Id создателя марафона
        /// </summary> 
        [BsonElement("CreatedBy")]
        [BsonTimeSpanOptions(BsonType.String)]
        public string CreatedBy { get; set; }
        
        /// <summary>
        /// Дата создания марафона
        /// </summary> 
        [BsonElement("CreatedAt")]
        [BsonTimeSpanOptions(BsonType.String)]
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Продолжительность марафона
        /// </summary> 
        [BsonElement("Duration")]
        [BsonTimeSpanOptions(BsonType.String)]
        public TimeSpan Duration { get; set; }
    }
}