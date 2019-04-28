using System;

namespace Client.Models.Maraphone
{
    public class Maraphone
    {
        /// <summary>
        /// Идентификатор марафона
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Название марафона
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание марафона
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Спринты марафона
        /// </summary>
        public Sprint[] Sprints { get; set; }
        
        /// <summary>
        /// Id создателя марафона
        /// </summary> 
        public string CreatedBy { get; set; }
        
        /// <summary>
        /// Дата создания марафона
        /// </summary> 
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Продолжительность марафона
        /// </summary> 
        public TimeSpan Duration { get; set; }
    }
}