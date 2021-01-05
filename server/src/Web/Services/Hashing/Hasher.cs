using BCrypt.Net;
using System;

namespace Web.Services.Hashing
{
    public class Hasher : IHasher
    {
        public string Hash(string unhashed)
        {
            return BCrypt.Net.BCrypt.HashPassword(unhashed);
        }

        public bool CheckMatch(string unhashed, string hashed)
        {
            try
            {
                return BCrypt.Net.BCrypt.Verify(unhashed, hashed);
            }
            catch (HashInformationException)
            {
                return false;
            }
            catch (BcryptAuthenticationException)
            {
                return false;
            }
            catch (SaltParseException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
