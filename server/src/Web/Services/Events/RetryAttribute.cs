using System;

namespace Web.Services.Events
{
    public class RetryAttribute : Attribute
    {
        private readonly int maxAttempts = 1;
        public int MaxAttempts
        {
            get => maxAttempts;
            init
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxAttempts));
                }

                maxAttempts = value;
            }
        }
    }
}
