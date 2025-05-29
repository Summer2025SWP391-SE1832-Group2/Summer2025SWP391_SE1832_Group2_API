using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BDNAT_Repository.DTO
{
    public class APIResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }

        public APIResponse(bool success, string message, object data = null)
        {
            Success = success;
            Message = message;
            Data = data;
        }

        public static APIResponse Ok(object data = null, string message = "Success")
        {
            return new APIResponse(true, message, data);
        }

        public static APIResponse Error(string message = "Error", object data = null)
        {
            return new APIResponse(false, message, data);
        }
    }
}
