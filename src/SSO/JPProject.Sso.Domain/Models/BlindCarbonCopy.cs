using JPProject.Domain.Core.StringUtils;
using Microsoft.EntityFrameworkCore.Internal;
using System.Linq;

namespace JPProject.Sso.Domain.Models
{
    public class BlindCarbonCopy
    {
        public BlindCarbonCopy() { }
        public BlindCarbonCopy(string emails)
        {
            _recipientsCollection = emails;
        }
        private string _recipientsCollection;

        public string[] Recipients
        {
            get => _recipientsCollection?.Split(";");
            set => _recipientsCollection = value?.Join(";");
        }

        public bool IsValid()
        {
            return Recipients != null &&
                   Recipients.Any() &&
                   Recipients.All(a => a.IsEmail());
        }

        public static implicit operator BlindCarbonCopy(string value)
            => new BlindCarbonCopy(value);


        public override string ToString() => _recipientsCollection;
    }
}