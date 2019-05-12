using System;

namespace Models.Converters.Maraphone
{
    using Client = global::Client.Models.Maraphone;
    using Model = global::Models.Maraphone;
    
    /// <summary>
    /// Предоставляет методы конвертирования контента между клиентской и серверной моделями
    /// </summary>
    public class ContentConverter
    {
        /// <summary>
        /// Переводит контент из сервеной модели в клиентскую
        /// </summary>
        /// <param name="modelContent">Задача в серверной модели</param>
        /// <returns>Задача в клиентской модели</returns>
        
        public static Client.Task.Content Convert(Model.Task.Content modelContent)
        {
            if (modelContent == null)
            {
                throw new ArgumentNullException(nameof(modelContent));
            }

            var clientContent = new Client.Task.Content
            {
                Id = modelContent.Id,
                Data = modelContent.Data
            };

            return clientContent;
        }
    }
}