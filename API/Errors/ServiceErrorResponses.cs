using System;
using System.Net;
using Client.Models.Errors;

namespace API.Errors
{
    public static class ServiceErrorResponses
    {
        public static ServiceErrorResponse TodoNotFound(string activityId)
        {
            if (activityId == null)
            {
                throw new ArgumentNullException(nameof(activityId));
            }

            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.NotFound,
                    Message = $"An activity with \"{activityId}\" not found.",
                    Target = "activity"
                }
            };

            return error;
        }
        
        public static ServiceErrorResponse Unauthenticated()
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.Unauthorized,
                    Message = "User is not unauthorized.",
                    Target = "user"
                }
            };

            return error;
        }
        
        public static ServiceErrorResponse ConflictLogin(string login)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.Conflict,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.Conflict,
                    Message = $"User with such login: \"{login}\" already exist.",
                    Target = "user"
                }
            };

            return error;
        }
        
        public static ServiceErrorResponse Forbidden()
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.Forbidden,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.Forbidden,
                    Message = "Access is forbidden.",
                    Target = "user"
                }
            };

            return error;
        }

        public static ServiceErrorResponse BodyIsMissing(string target)
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = "Request body is empty or has inappropriate form.",
                    Target = target
                }
            };

            return error;
        }
        
        public static ServiceErrorResponse UserIdIsNull()
        {
            var error = new ServiceErrorResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Error = new ServiceError
                {
                    Code = ServiceErrorCodes.BadRequest,
                    Message = "User Id is null.",
                    Target = "user"
                }
            };

            return error;
        }
    }
}
