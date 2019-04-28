namespace Client.Models.Users
{
    public class UserPatchInfo
    {
        /// <summary>
        /// Имя пользователя
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия пользователя
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Почта пользователя
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Телефон пользователя
        /// </summary>
        public string Phone { get; set; }
    }
}