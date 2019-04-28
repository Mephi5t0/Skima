using System;
using System.Runtime.Serialization;

namespace Client.Models.Activity
{
    public class ActivityCreationInfo
    {
        /// <summary>
        /// Id марафона, к которому принадлежит активность
        /// </summary>
        [DataMember(IsRequired = true)]
        public string MaraphoneId { get; set; }

        /// <summary>
        /// Тэги
        /// </summary>
        public string[] Tags { get; set; }

        /// <summary>
        /// Идентификаторы экспертов
        /// </summary>
        public string[] Experts { get; set; }
        
        /// <summary>
        /// Дата начала активности
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime StartAt { get; set; }
    }
}