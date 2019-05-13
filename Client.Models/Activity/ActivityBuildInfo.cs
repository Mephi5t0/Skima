using System;
using System.Runtime.Serialization;
using Client.Models.Entry;

namespace Client.Models.Activity
{
    public class ActivityBuildInfo
    {
        /// <summary>
        /// Id марафона, к которому принадлежит активность
        /// </summary>
        [DataMember(IsRequired = true)]
        public string MaraphoneId { get; set; }
        
        /// <summary>
        /// Тэги
        /// </summary>
        [DataMember(IsRequired = false)]
        public string[] Tags { get; set; }

        /// <summary>
        /// Идентификаторы экспертов
        /// </summary>
        [DataMember(IsRequired = false)]
        public string[] Experts { get; set; }
        
        /// <summary>
        /// Дата начала активности
        /// </summary>
        [DataMember(IsRequired = true)]
        public DateTime StartAt { get; set; }
    }
}