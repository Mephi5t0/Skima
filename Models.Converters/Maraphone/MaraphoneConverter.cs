using System;

namespace Models.Converters.Maraphone
{
    using Client = global::Client.Models.Maraphone;
    using Model = global::Models.Maraphone;
    
    /// <summary>
    /// Предоставляет методы конвертирования активность между серверной и клиентской моделями
    /// </summary>
    public class MaraphoneConverter
    {
        /// <summary>
        /// Переводит активность из серверной модели в клиентскую
        /// </summary>
        /// <param name="maraphoneCreationInfo">Активность в серверной модели</param>
        /// <returns>Активность в клиентской модели</returns>
        
        public static Model.Maraphone Convert(Client.MaraphoneCreationInfo maraphoneCreationInfo)
        {
            if (maraphoneCreationInfo == null)
            {
                throw new ArgumentNullException();
            }

            var modelMaraphone = new Model.Maraphone
            {
                Title = maraphoneCreationInfo.Title,
                Sprints = maraphoneCreationInfo.Sprints,
                Duration = maraphoneCreationInfo.Duration,
                CreatedAt = DateTime.Now,
                CreatedBy = maraphoneCreationInfo.CreatedBy,
                Description = maraphoneCreationInfo.Description
            };

            return modelMaraphone;
        }
        
        public static Client.Maraphone ConvertToClientModel(Model.Maraphone modelMaraphone)
        {
            if (modelMaraphone == null)
            {
                throw new ArgumentNullException();
            }

            var clientMaraphone = new Client.Maraphone
            {
                Id = modelMaraphone.Id,
                Title = modelMaraphone.Title,
                Sprints = modelMaraphone.Sprints,
                Duration = modelMaraphone.Duration,
                CreatedAt = modelMaraphone.CreatedAt,
                CreatedBy = modelMaraphone.CreatedBy,
                Description = modelMaraphone.Description
            };

            return clientMaraphone;
        }
    }
}