using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public bool IsSuccess { get; set; }

        public int UpdateCount { get; set; }

        public string Message { get; set; }

        public List<string> Errors { get; set; }

        public bool HasErrors => this.Errors != null && this.Errors.Any();
    }
}
