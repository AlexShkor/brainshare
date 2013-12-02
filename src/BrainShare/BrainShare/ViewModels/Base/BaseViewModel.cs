using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace BrainShare.ViewModels.Base
{
    public abstract class BaseViewModel
    {
        private List<ValidationError> _errors;
        private string _referrerUrl;

        protected BaseViewModel()
        {
            Errors = new List<ValidationError>();
        }


        public string ReferrerUrl
        {
            get
            {
                var request = HttpContext.Current.Request;
                return _referrerUrl ?? (request.UrlReferrer != null ? request.UrlReferrer.LocalPath : "/");
            }
            set { _referrerUrl = value; }
        }

        public void RedirectToRefferer()
        {
            RedirectUrl = ReferrerUrl;
        }

        public string RedirectUrl { get; set; }
        public string SuccessMessage { get; set; }
        public bool Loading { get; set; }
        public string SubmitBtnText = "Сохранить";

        public List<ValidationError> Errors
        {
            get { return _errors; }
            set
            {
                _errors = value;
                if (value != null && value.Any()) SuccessMessage = null;
            }
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static string GenerateId()
        {
            return ObjectId.GenerateNewId().ToString();
        }

        public void AddError(string errorMessage)
        {
            if (string.IsNullOrEmpty(errorMessage))
                return;

            SuccessMessage = string.Empty;
            Errors.Add(new ValidationError { ErrorMessage = errorMessage });
        }

        public void AddSuccess(string successMessage)
        {
            if (string.IsNullOrEmpty(successMessage))
                return;

            Errors.Clear();
            SuccessMessage = successMessage;
        }

        public void AddModelStateErrors(IEnumerable<ModelError>errors, bool clearPreviouserrors = false)
        {
            if (clearPreviouserrors)
            {
                ClearErrors();
            }
          
            foreach (var modelError in errors)
            {
                Errors.Add(new ValidationError { ErrorMessage = modelError.ErrorMessage });
            }
        }

        public void ClearErrors()
        {
            if (Errors != null)
            {
                Errors.Clear();
            }
        }
    }
}