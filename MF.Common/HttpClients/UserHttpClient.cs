using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MF.Common.HttpClients;

public class MembershipHttpClient
{
    public MembershipHttpClient(HttpClient client)
	{
        Client = client;
    }

    public HttpClient Client { get; }  
}
