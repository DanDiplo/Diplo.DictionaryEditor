using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplo.Dictionary.Models
{
    /// <summary>
    /// Represents the response returned from importing a dictionary file (ie. CSV)
    /// </summary>
    public class ImportResponse
    {
        public ImportResponse()
        {
            this.Update = new UpdateResponse()
            {
                Message = "Nothing was updated."
            };

            this.Validation = new ValidationResponse()
            {
                Message = "Nothing was validated."
            };
        }

        public string Filename { get; set; }

        public string Message => this.Validation.Message + " " + this.Update.Message;

        public bool IsSuccess => this.HasValidation && this.Validation.IsValid && this.HasUpdate && this.Update.IsSuccess;

        public UpdateResponse Update { get; set; }

        public bool HasUpdate => this.Update != null;

        public ValidationResponse Validation { get; set; }

        public bool HasValidation => this.Validation != null;
    }
}
