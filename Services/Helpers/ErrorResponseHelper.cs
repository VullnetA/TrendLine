using Microsoft.AspNetCore.Mvc;

namespace TrendLine.Services.Helpers
{
    public static class ErrorResponseHelper
    {
        /// <summary>
        /// Generates a standardized 404 Not Found response.
        /// </summary>
        public static ActionResult NotFoundResponse(ControllerBase controller, string message)
        {
            return controller.NotFound(new
            {
                Status = 404,
                Message = message,
                Timestamp = DateTime.UtcNow.ToString("o")
            });
        }

        /// <summary>
        /// Generates a standardized 500 Internal Server Error response.
        /// </summary>
        public static ActionResult InternalServerErrorResponse(ControllerBase controller, string message, Exception ex)
        {
            return controller.StatusCode(500, new
            {
                Status = 500,
                Message = message,
                Details = ex.Message,
                Timestamp = DateTime.UtcNow.ToString("o")
            });
        }

        /// <summary>
        /// Generates a standardized 400 Bad Request response.
        /// </summary>
        public static ActionResult BadRequestResponse(ControllerBase controller, string message, object details = null)
        {
            return controller.BadRequest(new
            {
                Status = 400,
                Message = message,
                Details = details,
                Timestamp = DateTime.UtcNow.ToString("o")
            });
        }

        /// <summary>
        /// Generates a standardized 401 Unauthorized response.
        /// </summary>
        public static ActionResult UnauthorizedResponse(ControllerBase controller, string message)
        {
            return controller.Unauthorized(new
            {
                Status = 401,
                Message = message,
                Timestamp = DateTime.UtcNow.ToString("o")
            });
        }

        /// <summary>
        /// Generates a standardized 403 Forbidden response.
        /// </summary>
        public static ActionResult ForbiddenResponse(ControllerBase controller, string message)
        {
            return controller.StatusCode(403, new
            {
                Status = 403,
                Message = message,
                Timestamp = DateTime.UtcNow.ToString("o")
            });
        }

        /// <summary>
        /// Generates a standardized response for any HTTP status code.
        /// </summary>
        public static ActionResult CustomErrorResponse(
            ControllerBase controller,
            int statusCode,
            string message,
            object details = null)
        {
            return controller.StatusCode(statusCode, new
            {
                Status = statusCode,
                Message = message,
                Details = details,
                Timestamp = DateTime.UtcNow.ToString("o")
            });
        }
    }
}
