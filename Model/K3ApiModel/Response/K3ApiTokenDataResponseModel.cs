using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;

namespace K3ApiModel
{
   public class K3ApiTokenDataResponseModel
    {
        public int AcctID { get; set; }

        public int UserID { get; set; }

        public string Token { get; set; }

        public string Code { get; set; }

        public double Validity { get; set; }

        public string IPAddress { get; set; }

        public string Language { get; set; }

        public DateTime Create { get; set; }

    }
}
