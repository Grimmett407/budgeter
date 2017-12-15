using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace budgeter.Models.CodeFirst
{
    public class EmailModel
    {
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}