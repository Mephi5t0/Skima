namespace Client.Models.Maraphone.Task
{
    public class Task
    {
        /// <summary>
        /// Идентификатор создателя задачи
        /// </summary>
        public string CreatedBy { get; set; }
        
        /// <summary>
        /// Название задачи
        /// </summary>
        public string Title { get; set; }
        
        /// <summary>
        /// Содержание задачи
        /// </summary>
        public string ContentId { get; set; }
    }
}