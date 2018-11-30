using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests
{
    /// <summary>
    /// Use the dispose pattern to allow an easy way to set a Currencies 
    /// provider and remove it when the test is finished
    /// </summary>
    internal class RegisterCurrencies : IDisposable
    {
        private readonly Currencies _instance;
        public RegisterCurrencies(Currencies instance)
        {
            _instance = instance;
            Currencies.SetProvider(() => _instance);
        }

        public void Dispose()
        {
            Currencies.SetProvider(null);
        }
    }
}
