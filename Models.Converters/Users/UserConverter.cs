using System;

namespace Models.Converters.Users
{
    using Client = global::Client.Models.Users;

    /// <summary>
    /// Предоставляет методы конвертирования пользователя между серверной и клиентской моделями
    /// </summary>
    public static class UserConverter
    {
        /// <summary>
        /// Переводит подьзователя из серверной модели в клиентскую
        /// </summary>
        /// <param name="modelUser">Пользователь в серверной модели</param>
        /// <returns>Пользователь в клиентской модели</returns>
        public static Client.User Convert(global::Models.Users.User modelUser)
        {
            if (modelUser == null)
            {
                throw new ArgumentNullException(nameof(modelUser));
            }

            var clientUser = new Client.User
            {
                Id = modelUser.Id,
                Login = modelUser.Login,
                RegisteredAt = modelUser.RegisteredAt
            };

            return clientUser;
        }
    }
}
