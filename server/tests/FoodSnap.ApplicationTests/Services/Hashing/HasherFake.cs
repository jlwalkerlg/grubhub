using FoodSnap.Application.Services.Hashing;

namespace FoodSnap.ApplicationTests.Services.Hashing
{
    public class HasherFake : IHasher
    {
        public bool CheckMatch(string unhashed, string hashed)
        {
            return Hash(unhashed) == hashed;
        }

        public string Hash(string unhashed)
        {
            return $"HASHED_{unhashed}";
        }
    }
}
