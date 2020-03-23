namespace EnterpriseApplicationIntegration.Core.HTTP
{
    /// <summary>
    /// This class is defining the validation error information return for the API.
    /// </summary>
    public class ValidationError
    {
        /// <summary>
        /// The ErrorCode property contains the error code that is associated with the validation error that caused the exception.
        /// </summary>
        public string ErrorCode { get; set; }

        /// <summary>
        /// The ErrorMessage property contains the massage that is associated with the validation error that caused the exception based on the langauge defind in the AcceptLanguage.
        /// </summary>
        public string ErrorMessage { get; set; }

        public string Message { get; set; }
        public string Code { get; set; }
    }
}