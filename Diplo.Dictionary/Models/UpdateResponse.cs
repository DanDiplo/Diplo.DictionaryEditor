using System;
using System.Collections.Generic;
using System.Linq;

namespace Diplo.Dictionary.Models
{
    /// <summary>
    /// Response from updating dictionary items
    /// </summary>
    public class UpdateResponse
    {
        public UpdateResponse()
        {
            this.Errors = new List<string>();
            this.Warnings = new List<string>();
        }

        public bool IsSuccess { get; set; }

        public int UpdateCount { get; set; }

        public string Message { get; set; }

        public List<string> Errors { get; set; }

        public bool HasErrors => this.Errors != null && this.Errors.Any();

        public List<string> Warnings { get; set; }

        public bool HasWarnings => this.Warnings != null && this.Warnings.Any();
    }
}