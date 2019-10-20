using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests.Util
{
    /// <summary>
    /// Use the dispose pattern to allow an easy way to set a Currencies 
    /// provider and remove it when the test is finished
    /// </summary>
    internal class RegisterCountryCodes : IDisposable
    {
        private readonly ICountryCodeProvider _instance;
        public RegisterCountryCodes(ICountryCodeProvider instance)
        {
            _instance = instance;
            CountryCodes.SetProvider(() => _instance);
        }

        public void Dispose()
        {
            CountryCodes.SetProvider(() => null);
        }
    }
}
