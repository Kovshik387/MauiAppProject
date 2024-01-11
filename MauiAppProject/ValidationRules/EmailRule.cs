using System.Text.RegularExpressions;

namespace MauiAppProject.ValidationRules
{
    public class EmailRule : IValidationRule<string>
    {
        private readonly Regex _regex = new(@"^([w.-]+)@([w-]+)((.(w){2,3})+)$");

        public string ValidationMessage { get; set; }

        public bool Check(string value) =>
            value is string str && _regex.IsMatch(str);
    }
}
