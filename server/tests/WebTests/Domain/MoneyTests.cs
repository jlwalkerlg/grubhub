using System;
using Shouldly;
using Web.Domain;
using Xunit;

namespace WebTests.Domain
{
    public class MoneyTests
    {
        [Fact]
        public void Cant_Have_Greater_Precision_Than_1p()
        {
            Should.Throw<ArgumentException>(() => Money.FromPounds(1.001m));
        }

        [Fact]
        public void Cant_Have_Negative_Amounts()
        {
            Should.Throw<ArgumentOutOfRangeException>(() => Money.FromPence(-1));
            Should.Throw<ArgumentOutOfRangeException>(() => Money.FromPounds(-1m));
        }
    }
}
