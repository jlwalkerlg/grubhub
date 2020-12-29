using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using FluentValidation;
using Application.Validation;

namespace Application.Restaurants.UpdateOpeningHours
{
    public class UpdateOpeningHoursValidator : FluentValidator<UpdateOpeningHoursCommand>
    {
        private static Regex regex = new(
            @"^\d{2}:\d{2}$",
            RegexOptions.Compiled,
            TimeSpan.FromMilliseconds(250));

        public UpdateOpeningHoursValidator()
        {
            CascadeRuleFor(x => x.RestaurantId)
                .Required();

            ValidateOpeningHours(x => x.MondayOpen, x => x.MondayClose);
            ValidateOpeningHours(x => x.TuesdayOpen, x => x.TuesdayClose);
            ValidateOpeningHours(x => x.WednesdayOpen, x => x.WednesdayClose);
            ValidateOpeningHours(x => x.ThursdayOpen, x => x.ThursdayClose);
            ValidateOpeningHours(x => x.FridayOpen, x => x.FridayClose);
            ValidateOpeningHours(x => x.SaturdayOpen, x => x.SaturdayClose);
            ValidateOpeningHours(x => x.SundayOpen, x => x.SundayClose);
        }

        private void ValidateOpeningHours(
            Expression<Func<UpdateOpeningHoursCommand, string>> openingTimeSelector,
            Expression<Func<UpdateOpeningHoursCommand, string>> closingTimeSelector)
        {
            var compiledOpeningTimeSelector = openingTimeSelector.Compile();
            var compiledClosingTimeSelector = closingTimeSelector.Compile();

            When(x => !string.IsNullOrWhiteSpace(compiledOpeningTimeSelector(x)), () =>
            {
                CascadeRuleFor(openingTimeSelector)
                    .Must(HaveValidFormat).WithMessage("Must have format hh:mm.")
                    .Must(BeWithinOpeningTimeRange).WithMessage("Must be between 00:00 and 23:59.");

                When(x => !string.IsNullOrWhiteSpace(compiledClosingTimeSelector(x)), () =>
                {
                    CascadeRuleFor(closingTimeSelector)
                        .Must(HaveValidFormat).WithMessage("Must have format hh:mm.")
                        .Must(BeWithinClosingTimeRange).WithMessage("Must be between 00:01 and 23:59 .")
                        .Must((x, closingTime) => BeLaterThanClosingTime(compiledOpeningTimeSelector(x), closingTime)).WithMessage("Must be later than closing time.");
                });
            });
        }

        private bool HaveValidFormat(string time)
        {
            return regex.IsMatch(time);
        }

        private bool BeLaterThanClosingTime(string openingTime, string closingTime)
        {
            var openingSplit = openingTime.Split(':');
            var openingHour = int.Parse(openingSplit[0]);
            var openingMinute = int.Parse(openingSplit[1]);

            var closingSplit = closingTime.Split(':');
            var closingHour = int.Parse(closingSplit[0]);
            var closingMinute = int.Parse(closingSplit[1]);

            if (openingHour == closingHour)
            {
                return openingMinute < closingMinute;
            }

            return openingHour < closingHour;
        }

        private bool BeWithinOpeningTimeRange(string time)
        {
            var split = time.Split(':');

            var hour = int.Parse(split[0]);
            if (hour < 0 || hour > 23)
            {
                return false;
            }

            var minute = int.Parse(split[1]);
            if (minute < 0 || minute > 59)
            {
                return false;
            }

            return true;
        }

        private bool BeWithinClosingTimeRange(string time)
        {
            var split = time.Split(':');

            var hour = int.Parse(split[0]);
            if (hour < 0 || hour > 23)
            {
                return false;
            }

            var minute = int.Parse(split[1]);
            if (minute < 0 || minute > 59)
            {
                return false;
            }

            if (hour == 0 && minute == 0)
            {
                return false;
            }

            return true;
        }
    }
}
