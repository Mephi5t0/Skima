using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace EventGenerator.Models
{
    public class ActivityFinishedInfo
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        
        
        /// <summary>
        /// Идентификатор активности
        /// </summary>
        [BsonElement("ActivityId")]
        public string ActivityId { get; set; }
        
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
        /// Событие обработано
        /// </summary>
        [BsonElement("IsChecked")]
        public bool IsChecked  { get; set; }
    }
}