namespace FoodSnap.Application.Services.Hashing
{
    public interface IHasher
    {
        string Hash(string unhashed);
        bool CheckMatch(string unhashed, string hashed);
    }
}
