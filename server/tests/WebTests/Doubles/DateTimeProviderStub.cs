using System;
using Web.Services.DateTimeServices;

namespace WebTests.Doubles
{
    public class DateTimeProviderStub : IDateTimeProvider
    {
        public DateTime UtcNow { get; set; }
        public DateTime GmtNow => UtcNow;
    }
}
