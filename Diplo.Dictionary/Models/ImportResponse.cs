using System;

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

        /// <summary>
        /// Gets the filename of the uploaded CSV file
        /// </summary>
        public string Filename { get; set; }

        /// <summary>
        /// Gets the message to show to the UI
        /// </summary>
        public string Message => this?.Validation?.Message + " " + this?.Update?.Message;

        /// <summary>
        /// Gets whether the import was a success
        /// </summary>
        public bool IsSuccess => this.HasValidation && this.Validation.IsValid && this.HasUpdate && this.Update.IsSuccess;

        /// <summary>
        /// Gets the response from the database update
        /// </summary>
        public UpdateResponse Update { get; set; }

        /// <summary>
        /// Gets whether an update has been performed
        /// </summary>
        public bool HasUpdate => this.Update != null;

        /// <summary>
        /// Gets the response from the CSV validation
        /// </summary>
        public ValidationResponse Validation { get; set; }

        /// <summary>
        /// Gets whether the validation has been performed
        /// </summary>
        public bool HasValidation => this.Validation != null;
    }
}
