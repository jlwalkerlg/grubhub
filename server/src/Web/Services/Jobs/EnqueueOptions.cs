using System;

namespace Web.Services
{
    public class EnqueueOptions
    {
        private int? maxAttempts;

        public int? MaxAttempts
        {
            get => maxAttempts;

            init
            {
                if (value.HasValue && value.Value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(MaxAttempts));
                }

                maxAttempts = value;
            }
        }
    }
}
