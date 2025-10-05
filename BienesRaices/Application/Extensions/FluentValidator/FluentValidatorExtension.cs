using Application.Models.Paging;

namespace Application.Extensions.FluentValidator
{
    public static class FluentValidatorExtension
    {
        public const string EndDateGreaterThanStartDateMessage = "End date must be greater than start date";
        public static bool EndDateGreaterThanStartDate<T>(this T request, string startDatePropertyName, string endDatePropertyName)
        {
            var type = typeof(T);
            var startDate = (DateTime)type.GetProperty(startDatePropertyName)!.GetValue(request)!;
            var endDate = (DateTime)type.GetProperty(endDatePropertyName)!.GetValue(request)!;

            return endDate > startDate;
        }

        public const string PagingValuesMustBeValidMessage = "Page Number and Page Size must be greater than 0";
        public static bool PagingValuesMustBeValid<T>(this T request) where T : BasePagedModel
        {
            return request.PageSize > 0 && request.PageNumber > 0;
        }
    }
}
