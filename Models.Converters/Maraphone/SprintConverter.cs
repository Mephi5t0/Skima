using System;
using System.Linq;

namespace Models.Converters.Maraphone
{
    using Client = global::Client.Models.Maraphone;
    using Model = global::Models.Maraphone;
    
    /// <summary>
    /// Предоставляет методы конвертирования спринта между серверной и клиентской моделями
    /// </summary>
    public class SprintConverter
    {   
        /// <summary>
        /// Переводит спринт из серверной модели в клиентскую
        /// </summary>
        /// <param name="modelSprint">Спринт в серверной модели</param>
        /// <returns>Спринт в клиентской модели</returns>
        
        public static Client.Sprint Convert(Model.Sprint modelSprint)
        {
            if (modelSprint == null)
            {
                throw new ArgumentNullException();
            }

            var clientTasks = modelSprint.Tasks.Select(TaskConverter.Convert).ToArray();
            
            var clientSprint = new Client.Sprint
            {
                Number = modelSprint.Number,
                Tasks = clientTasks,
                Duration = modelSprint.Duration,
                CreatedBy = modelSprint.CreatedBy,
                Description = modelSprint.Description
            };

            return clientSprint;
        }
    }
}