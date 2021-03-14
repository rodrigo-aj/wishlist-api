using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace dockerapi.Models
{
    public class Usuario
    {
        public int id { get; set; }

        public string nome { get; set; }

        public string email { get; set; }

        [JsonIgnore]
        public string senha { get; set; }
    }
}
