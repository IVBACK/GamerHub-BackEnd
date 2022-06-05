using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace GamerHub.SERVICE.Validations
{
    public class UserRegisterValidation
    {
        public bool IsEmailValid(string email)
        {
            EmailAddressAttribute e = new EmailAddressAttribute();
            if (e.IsValid(email))
                return true;

            return false;
        }

        public bool IsNameValid(string name)
        {
            Regex regex = new Regex(@"^[a-zA-Z\s]+$");

            if (regex.IsMatch(name))
                return true;

            return false;
        }
    }
}
