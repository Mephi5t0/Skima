using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Models.Activity
{
    public class ActivityCreationInfo
    {
        public ActivityCreationInfo(string maraphoneId, string[] tags, string createdBy, string[] experts, DateTime startAt, DateTime endAt)
        {
            this.MaraphoneId = maraphoneId ?? throw new ArgumentNullException();
            this.Tags = tags;
            this.CreatedBy = createdBy ?? throw new ArgumentNullException();
            this.Experts = experts;
            this.Status = Status.Announced;
            this.StartAt = startAt;
            this.EndAt = endAt;
        }
        
        /// <summary>
        /// Id марафона, к которому принадлежит активность
        /// </summary>
        public string MaraphoneId { get; set; }

        /// <summary>
        /// Тэги
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// Идентификатор создателя активности
        /// </summary>
        public string CreatedBy { get; set; }
        
        /// <summary>
        /// Идентификаторы экспертов
        /// </summary>
        public string[] Experts { get; set; }
        
        /// <summary>
        /// Статус активности
        /// </summary>
        public Status Status { get; set; }

        /// <summary>
        /// Дата начала активности
        /// </summary>
        public DateTime StartAt { get; set; }
        
        /// <summary>
        /// Дата окончания активности
        /// </summary>
        [BsonElement("EndAt")]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime EndAt { get; set; }
    }
}