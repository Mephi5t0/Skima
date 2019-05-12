using System;
using System.Linq;

namespace Models.Converters.Maraphone
{
    using Client = global::Client.Models.Maraphone;
    using Model = global::Models.Maraphone;
    
    /// <summary>
    /// Предоставляет методы конвертирования марафона между клиентской и серверной моделями
    /// </summary>
    public class MaraphoneConverter
    {
        public static Client.Maraphone Convert(Model.Maraphone modelMaraphone)
        {
            if (modelMaraphone == null)
            {
                throw new ArgumentNullException();
            }

            var clientSprints = modelMaraphone.Sprints.Select(SprintConverter.Convert).ToArray();
                
            var clientMaraphone = new Client.Maraphone
            {
                Id = modelMaraphone.Id,
                Title = modelMaraphone.Title,
                Sprints = clientSprints,
                Duration = modelMaraphone.Duration,
                CreatedAt = modelMaraphone.CreatedAt,
                CreatedBy = modelMaraphone.CreatedBy,
                Description = modelMaraphone.Description
            };

            return clientMaraphone;
        }
    }
}