using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestEF_SQLExpress_Deploy
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public partial class TestEFDeployEntities : DbContext
    {
        public TestEFDeployEntities(string connectionString)
            : base(connectionString)
        {
        }
    }
}
