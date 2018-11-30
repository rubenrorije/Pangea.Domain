using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pangea.Domain.Tests
{
    [TestClass]
    public class DefaultCountryCodeProviderTests
    {
        [TestMethod]
        public void Validate_The_Netherlands_Is_Registered()
        {
            var sut = new DefaultCountryProvider();
            sut.GetCountryCallingCodeFrom("31").Should().Be(31);
        }

        [TestMethod]
        public void Resolve_The_Country_Calling_Code_For_Germany()
        {
            var sut = new DefaultCountryProvider();
            sut.GetCountryCallingCodeFor("DE").Should().Be(49);
        }

        [TestMethod]
        public void Resolve_The_Country_Calling_Code_Is_Not_Case_Sensitive()
        {
            var sut = new DefaultCountryProvider();
            sut.GetCountryCallingCodeFor("de").Should().Be(49);
        }

        [TestMethod]
        public void Resolve_The_Country_Calling_Code_With_A_Null_String_Returns_Null()
        {
            var sut = new DefaultCountryProvider();
            sut.GetCountryCallingCodeFor(null).Should().Be(null);
        }

        [TestMethod]
        public void Resolve_The_Country_Calling_Code_With_An_Empty_String_Returns_Null()
        {
            var sut = new DefaultCountryProvider();
            sut.GetCountryCallingCodeFor(string.Empty).Should().Be(null);
        }

        [TestMethod]
        public void Resolve_The_Country_Calling_Code_With_An_Unknown_Country_Returns_Null()
        {
            var sut = new DefaultCountryProvider();
            sut.GetCountryCallingCodeFor("XX").Should().Be(null);
        }

        [TestMethod]
        public void All_Countries_Are_Specified_In_The_List()
        {
            var sut = new DefaultCountryProvider();

            var regions =
                CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(c => c.LCID != CultureInfo.InvariantCulture.LCID)
                .Where(c => !c.IsNeutralCulture)
                .Select(c => new RegionInfo(c.LCID))
                .Distinct()
                .ToList();

            regions.Should().HaveCountGreaterThan(20);

            Console.WriteLine(string.Join(Environment.NewLine, regions.Select(r => r.TwoLetterISORegionName).OrderBy(x => x)));

            foreach (var region in regions)
            {
                if (region.TwoLetterISORegionName.Length > 2) continue;
                var callingCode = sut.GetCountryCallingCodeFor(region.TwoLetterISORegionName);
                callingCode.Should().NotBeNull($"{region} should have a country calling code");
            }
        }
    }
}
