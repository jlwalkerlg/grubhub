using Shouldly;
using System;
using System.Net;
using Web;
using Web.Features.Restaurants;

namespace WebTests
{
    public static class TestExtensions
    {
        public static void ShouldBeSuccessful(this Result result)
        {
            result.IsSuccess.ShouldBe(true);
        }

        public static void ShouldBeAnError(this Result result)
        {
            result.IsSuccess.ShouldBe(false);
        }
        
        public static void ShouldBeAnError(this Result result, ErrorType type)
        {
            result.IsSuccess.ShouldBe(false);
            result.Error.Type.ShouldBe(type);
        }

        public static void ShouldBe(this HttpStatusCode code, int expected)
        {
            ((int)code).ShouldBe(expected);
        }

        public static void ShouldBe(this OpeningHoursDto dto, TimeSpan? open, TimeSpan? close)
        {
            dto.Open.ShouldBe(open?.ToString(@"hh\:mm"));
            dto.Close.ShouldBe(close?.ToString(@"hh\:mm"));
        }

        public static void ShouldBe(this string sut, Enum expected)
        {
            sut.ShouldBe(expected.ToString());
        }
    }
}
