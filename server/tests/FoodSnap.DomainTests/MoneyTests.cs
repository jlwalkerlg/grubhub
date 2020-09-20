using System;
using FoodSnap.Domain;
using Xunit;

namespace FoodSnap.DomainTests
{
    public class MoneyTests
    {
        [Fact]
        public void Amount_Cant_Be_Less_Than_Zero()
        {
            Assert.Throws<ArgumentException>(() => new Money(-1));
        }

        [Fact]
        public void Equal_When_Amounts_Are_The_Same()
        {
            var money1 = new Money(1);
            var money2 = new Money(1);

            Assert.Equal(money1, money2);
            Assert.True(money1 == money2);
            Assert.Equal(money1.GetHashCode(), money2.GetHashCode());
        }
    }
}
