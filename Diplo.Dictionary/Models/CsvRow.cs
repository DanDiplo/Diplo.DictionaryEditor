using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Diplo.Dictionary.Models
{
    public class CsvRow
    {
        public int Id { get; set; }

        public Guid Key { get; set; }

        public int LangId { get; set; }

        public string Language { get; set; }

        public string Name { get; set; }

        public string Translation { get; set; }
    }
}
