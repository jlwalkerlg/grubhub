namespace FoodSnap.Domain
{
    public abstract class Entity<T> where T : Entity<T>
    {
        protected abstract bool IdentityEquals(T other);
        public abstract override int GetHashCode();

        public sealed override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (GetType() != obj.GetType())
            {
                return false;
            }

            var other = obj as T;

            return IdentityEquals(other);
        }

        public static bool operator ==(Entity<T> a, Entity<T> b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Entity<T> a, Entity<T> b)
        {
            return !(a == b);
        }
    }
}
