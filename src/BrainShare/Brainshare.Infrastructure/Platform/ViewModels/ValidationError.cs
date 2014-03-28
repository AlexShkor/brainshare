namespace Brainshare.Infrastructure.Platform.ViewModels
{
    public class ValidationError
    {
        public string PropertyName { get; set; }
        public string ErrorMessage { get; set; }

        public ValidationError()
        {
            
        }

        public ValidationError(string message,string propertyName = null)
        {
            ErrorMessage = message;
            PropertyName = propertyName;
        }
    }
}