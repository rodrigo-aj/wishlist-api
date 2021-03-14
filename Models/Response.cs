using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dockerapi.Models
{
    public class Response
    {
        public long status { get; set; }
        public string mensagem { get; set; }
        public object objeto { get; set; }
    }
}
