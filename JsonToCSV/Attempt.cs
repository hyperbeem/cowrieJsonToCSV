using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonToCSV
{
    public class Attempt
    {
        public string eventid { get; set; }
        public string username { get; set; }
        public DateTime timestamp { get; set; }
        public string message { get; set; }
        public string src_ip { get; set; }
        public string session { get; set; }
        public string password { get; set; }
        public string sensor { get; set; }
    }
}
