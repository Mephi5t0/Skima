using System;

namespace Models.Converters.Activities
{
    using Client = global::Client.Models.Activity;
    using Model = global::Models.Activity;

    /// <summary>
    /// Предоставляет методы конвертирования запроса активностей между клиентской и серверной моделями
    /// </summary>
    public class ActivitySearchInfoQueryConverter
    {
        /// <summary>
        /// Переводит запрос активностей из клиентсокой модели в серверную
        /// </summary>
        /// <param name="clientQuery">Запрос задач в клиентской модели</param>
        /// <returns>Запрос задач в серверной модели</returns>
        public static Model.ActivityInfoSearchQuery Convert(Client.ActivityInfoSearchQuery clientQuery)
        {
            if (clientQuery == null)
            {
                throw new ArgumentNullException(nameof(clientQuery));
            }

            var modelQuery = new Model.ActivityInfoSearchQuery
            {
                Tags = clientQuery.Tags,
                Experts = clientQuery.Experts,
                StartAt = clientQuery.StartAt,
                CreatedAt = clientQuery.CreatedAt,
                CreatedBy = clientQuery.CreatedBy,
                MaraphoneId = clientQuery.MaraphoneId,
                Limit = clientQuery.Limit,
                Offset = clientQuery.Offset
            };

            return modelQuery;
        }
    }
}