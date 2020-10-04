using System;
using FoodSnap.Domain.Menus;
using Xunit;

namespace FoodSnap.DomainTests.Menus
{
    public class MenuIdTests
    {
        [Fact]
        public void Test_Equality()
        {
            var id1 = new MenuId(Guid.NewGuid());
            var id2 = new MenuId(Guid.NewGuid());
            var id3 = new MenuId(id1.Value);

            Assert.NotEqual(id1, id2);
            Assert.False(id1 == id2);
            Assert.False(id1.Equals(id2));

            Assert.Equal(id1, id3);
            Assert.True(id1 == id3);
            Assert.True(id1.Equals(id3));
        }
    }
}
