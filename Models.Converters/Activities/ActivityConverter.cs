using System;

namespace Models.Converters.Activities
{
    using Client = global::Client.Models.Activity;
    
    /// <summary>
    /// Предоставляет методы конвертирования активность между серверной и клиентской моделями
    /// </summary>
    public static class ActivityConverter
    {
        
        /// <summary>
        /// Переводит активность из серверной модели в клиентскую
        /// </summary>
        /// <param name="modelActivity">Активность в серверной модели</param>
        /// <returns>Активность в клиентской модели</returns>
        
        public static Client.Activity Convert(global::Models.Activity.Activity modelActivity)
        {
            if (modelActivity == null)
            {
                throw new ArgumentNullException();
            }

            var clientActivity = new Client.Activity
            {
                Id = modelActivity.Id,
                Tags = modelActivity.Tags,
                Experts = modelActivity.Experts,
                StartAt = modelActivity.StartAt,
                CreatedAt = modelActivity.CreatedAt,
                CreatedBy = modelActivity.CreatedBy,
                MaraphoneId = modelActivity.MaraphoneId
            };

            return clientActivity;
        }
    }
}