using System;

namespace Web.Services.Jobs
{
    public class HangfireJobResultException : Exception
    {
        public HangfireJobResultException(Error error) : base(error.Message)
        {
        }
    }
}
