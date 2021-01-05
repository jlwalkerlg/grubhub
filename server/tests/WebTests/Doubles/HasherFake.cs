using Web.Services.Hashing;

namespace WebTests.Doubles
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
