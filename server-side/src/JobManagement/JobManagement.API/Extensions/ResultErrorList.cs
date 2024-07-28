using FluentResults;

namespace JobManagement.API.Extensions
{
    public static class ResultErrorList
    {
        public static List<string> ToStringList(this List<IError> errors)
        {
            return errors.Select(error => error.Message).ToList();
        }
    }
}
