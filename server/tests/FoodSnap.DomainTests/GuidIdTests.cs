using System;
using FoodSnap.Domain;
using Xunit;

namespace FoodSnap.DomainTests.Menus
{
    public class GuidIdTests
    {
        [Fact]
        public void Underlying_Guid_Cant_Be_Empty()
        {
            Assert.Throws<ArgumentException>(() => new GuidId(Guid.Empty));
        }

        [Fact]
        public void Two_Ids_Are_Equal_When_They_Contain_Equal_Guids()
        {
            var guid = Guid.NewGuid();

            var id1 = new GuidId(guid);
            var id2 = new GuidId(guid);

            Assert.Equal(id1, id2);
            Assert.True(id1 == id2);
            Assert.Equal(id1.GetHashCode(), id2.GetHashCode());
        }

        [Fact]
        public void Two_Ids_Are_Not_Equal_When_They_Contain_Different_Guids()
        {
            var id1 = new GuidId(Guid.NewGuid());
            var id2 = new GuidId(Guid.NewGuid());

            Assert.NotEqual(id1, id2);
            Assert.False(id1 == id2);
            Assert.NotEqual(id1.GetHashCode(), id2.GetHashCode());
        }
    }
}
