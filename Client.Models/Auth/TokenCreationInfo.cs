using System.Runtime.Serialization;

namespace Client.Models.Auth
{
    public class TokenCreationInfo
    {
        /// <summary>
        /// Логин пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Password { get; set; }
    }
}