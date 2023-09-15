using System.Text.RegularExpressions;

namespace IdentityServer.Helpers
{
    public class ValidateInput
    {
        public static bool IsValidEmail(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            try
            {
                return Regex.IsMatch(input,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
        public static bool IsValidPhoneNumber(string input)
        {
            if (string.IsNullOrEmpty(input))
                return false;
            try
            {
                string pattern = @"^(\+\d{1,3}\s?)?(\(\d{1,3}\)\s?)?\d{1,4}[\s.-]?\d{1,4}[\s.-]?\d{1,9}$";

                return Regex.IsMatch(input, pattern);
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
