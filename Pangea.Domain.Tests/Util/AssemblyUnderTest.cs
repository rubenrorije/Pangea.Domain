using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests.Util
{
    public static class AssemblyUnderTest
    {
        private static readonly Lazy<Assembly> _instanceProvider;

        public static Assembly Instance => _instanceProvider.Value;

        static AssemblyUnderTest()
        {
            _instanceProvider = new Lazy<Assembly>(() => typeof(CreditCard).Assembly);
        }
    }
}
