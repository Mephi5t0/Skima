using System;

namespace Client.Models.Activity
{
    public class ActivityInfoSearchQuery
    {
        /// <summary>
        /// Позиция, начиная с которой нужно производить поиск
        /// </summary>
        public int? Offset { get; set; }

        /// <summary>
        /// Максимальеное количество активностей, которое нужно вернуть
        /// </summary>
        public int? Limit { get; set; }
        
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
        /// Дата создания активности
        /// </summary>
        public DateTime? CreatedAt { get; set; }
        
        /// <summary>
        /// Дата начала активности
        /// </summary>
        public DateTime? StartAt { get; set; }
    }
}