using System.Runtime.Serialization;

namespace Client.Models.Users
{
    /// <summary>
    /// Информация для регистрации пользователя
    /// </summary>
    [DataContract]
    public class UserRegistrationInfo
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
        
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string LastName { get; set; }
        
        /// <summary>
        /// Почта пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Email { get; set; }
        
        /// <summary>
        /// Телефон пользователя
        /// </summary>
        [DataMember(IsRequired = true)]
        public string Phone { get; set; }
    }
}
