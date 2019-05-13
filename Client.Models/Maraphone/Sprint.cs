using System;

namespace Client.Models.Maraphone
{
    public class Sprint
    {
        /// <summary>
        /// Номер спринта в марафоне
        /// </summary>
        public int Number { get; set; }
        
        /// <summary>
        /// Индентификатор создателя спринта
        /// </summary>
        public string CreatedBy { get; set; }
        
        /// <summary>
        /// Название спринта
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Описание спринта
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Продолжительность спринта
        /// </summary>
        public TimeSpan Duration { get; set; }
        
        /// <summary>
        /// Коллекция идентификаторов задач
        /// </summary>
        public Task.Task[] Tasks { get; set; }
    }
}