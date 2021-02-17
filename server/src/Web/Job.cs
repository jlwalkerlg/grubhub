using System;
using MediatR;

namespace Web
{
    public abstract class Job : INotification
    {
        public long Id { get; }
        public virtual int Retries { get; } = 1;
        public int Attempts { get; private set; }
        public bool IsComplete { get; private set; }
        public bool Failed => !IsComplete && Attempts == Retries;

        public void RegisterAttempt()
        {
            if (IsComplete)
            {
                throw new InvalidOperationException("Job is already complete.");
            }

            if (Attempts == Retries)
            {
                throw new InvalidOperationException("Job already retried enough.");
            }

            Attempts++;
        }

        public void MarkComplete()
        {
            IsComplete = true;
        }
    }
}
