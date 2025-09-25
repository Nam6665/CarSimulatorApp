using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIServiceLibrary.DTO
{
    public class ResultDTO
    {
        public string Gender { get; set; } = string.Empty;
        public NameDTO Name { get; set; } = new();
        public LocationDTO Location { get; set; } = new();
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Cell { get; set; } = string.Empty;
    }

}
