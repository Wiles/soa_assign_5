using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace shared
{
    [Serializable]
    public class JsonError
    {
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }

        public JsonError()
        {
            this.TimeStamp = DateTime.Now;
        }

        public JsonError(string Message)
            : this()
        {
            this.Message = Message;
        }

        public JsonError(Exception ex) : this(ex.Message)
        {
        }

        public override string ToString()
        {
            return this.Message;
        }
    }
}
