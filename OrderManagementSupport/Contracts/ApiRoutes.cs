using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSupport.Contracts
{
    public static class ApiRoutes
    {
        public static class Clients
        {
            public const string GetAll = "api/clients";
            public const string GetById = "api/clients/";
            public const string Post = "api/clients";
            public const string Delete = "api/clients/";
        }

        public static class Orders
        {
            public const string GetAll = "api/Orders";
        }
    }
}
