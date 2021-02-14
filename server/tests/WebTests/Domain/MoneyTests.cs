using System;
using Shouldly;
using Web.Domain;
using Xunit;

namespace WebTests.Domain
{
    public class MoneyTests
    {
        [Fact]
        public void Cant_Have_Less_Than_1p()
        {
            Should.Throw<ArgumentException>(() => Money.FromPence(-1));
            Should.Throw<ArgumentException>(() => Money.FromPounds(1.001m));
        }
    }
}
