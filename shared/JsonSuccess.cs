using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    [Serializable]
    public class JsonSuccess
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public JsonSuccess(string status, string message)
        {
            this.Status = status;
            this.Message = message;
        }

    }
}
