using System;
using System.Linq;

namespace Models.Maraphone
{
    using Client = global::Client.Models.Maraphone;
    using Model = global::Models.Maraphone;
    
    /// <summary>
    /// Информация для создания марафона
    /// </summary>
    public class MaraphoneCreationInfo
    {
        /// <summary>
        /// Название марафона
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Описание марафона
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Спринты марафона
        /// </summary>
        public Sprint[] Sprints { get; set; }
        
        /// <summary>
        /// Id создателя марафона
        /// </summary> 
        public string CreatedBy { get; set; }
        
        /// <summary>
        /// Продолжительность марафона
        /// </summary> 
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Создает марафон
        /// </summary>
        /// <param name="maraphoneBuildInfo">Информация для создания марафона</param>
        /// <param name="userId">Идентификатор создателя марафона</param>
        public MaraphoneCreationInfo(Client.MaraphoneBuildInfo maraphoneBuildInfo, string userId)
        {
            if (maraphoneBuildInfo == null)
            {
                throw new ArgumentNullException(nameof(maraphoneBuildInfo));
            }

            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var modelSprints = maraphoneBuildInfo.SprintsBuildInfo.Select(x => GetModelSprint(x, userId)).ToArray();
            
            this.Title = maraphoneBuildInfo.Title;
            this.Description = maraphoneBuildInfo.Description;
            this.Sprints = modelSprints;
            this.CreatedBy = userId;
            this.Duration = maraphoneBuildInfo.Duration;
        }
        
        private Model.Sprint GetModelSprint(Client.SprintBuildInfo sprintBuildInfo, string userId)
        {
            if (sprintBuildInfo == null)
            {
                throw new ArgumentNullException();
            }

            var modelTasks = sprintBuildInfo.Tasks.Select(x => GetModelTask(x, userId)).ToArray();
            
            var modelSprint = new Model.Sprint
            {
                Number = sprintBuildInfo.Number,
                Tasks = modelTasks,
                Duration = sprintBuildInfo.Duration,
                CreatedAt = DateTime.Now,
                CreatedBy = userId,
                Description = sprintBuildInfo.Description
            };

            return modelSprint;
        }
        
        private Model.Task.Task GetModelTask(global::Client.Models.Maraphone.Task.TaskBuildInfo taskBuildInfo,  string userId)
        {
            if (taskBuildInfo == null)
            {
                throw new ArgumentNullException();
            }

            var modelTask = new Model.Task.Task
            {
                Title = taskBuildInfo.Title,
                ContentId = taskBuildInfo.ContentId,
                CreatedAt = DateTime.Now,
                CreatedBy = userId
            };

            return modelTask;
        }
    }
}