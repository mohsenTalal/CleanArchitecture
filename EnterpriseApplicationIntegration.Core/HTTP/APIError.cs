using System.Collections.Generic;

namespace EnterpriseApplicationIntegration.Core.HTTP
{
    /// <summary>
    /// This class is defining the error information return for the API.
    /// </summary>
    public class APIError
    {
        /// <summary>
        /// The ErrorCode property contains the error code that is associated with the error that caused the exception.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// The ErrorMessage property contains the massage that is associated with the error that caused the exception based on the langauge defind in the AcceptLanguage.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// When the error is a validation error the API will return a list error that associate with the validation.
        /// </summary>
        public List<ValidationError> ValidationList { get; set; }
    }
}