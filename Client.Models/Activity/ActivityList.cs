using System.Collections.Generic;

namespace Client.Models.Activity
{
    /// <summary>
    ///  Список c описанием активностей
    /// </summary>
    public class ActivityList
    {
        /// <summary>
        /// Краткая информация об активностях
        /// </summary>
        public IReadOnlyCollection<Activity> Activities { get; set; }
    }
}