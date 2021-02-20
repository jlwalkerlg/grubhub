using System;

namespace Web.Services
{
    public class EnqueueOptions
    {
        private int maxAttempts = 1;
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

        public int[] DelaysInSeconds { get; init; }
    }
}
