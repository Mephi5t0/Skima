using System;

namespace Models.Converters.Maraphone
{
    using Client = global::Client.Models.Maraphone;
    using Model = global::Models.Maraphone;
    
    /// <summary>
    /// Предоставляет методы конвертирования задачи между серверной и клиентской моделями
    /// </summary>
    public class TaskConverter
    {        
        /// <summary>
        /// Переводит задачу из сервеной модели в клиентскую
        /// </summary>
        /// <param name="modelTask">Задача в серверной модели</param>
        /// <returns>Задача в клиентской модели</returns>
        
        public static global::Client.Models.Maraphone.Task.Task Convert(global::Models.Maraphone.Task.Task modelTask)
        {
            if (modelTask == null)
            {
                throw new ArgumentNullException();
            }

            var clientTask = new global::Client.Models.Maraphone.Task.Task
            {
                Title = modelTask.Title,
                ContentId = modelTask.ContentId,
                CreatedBy = modelTask.CreatedBy
            };

            return clientTask;
        }
    }
}