using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CRMApiModel.Response
{
    public class TokenResponseModel
    {
        public long id { get; set; }

        public string access_token { get; set; }

        public string refresh_token { get; set; }

        public string issued_at { get; set; }

        public string token_type { get; set; }

    }
}
