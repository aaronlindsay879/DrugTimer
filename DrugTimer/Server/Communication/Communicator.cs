using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DrugTimer.Server.Communication
{
    public class Communicator
    {
        protected static HttpClient _httpClient = new HttpClient();
    }
}
