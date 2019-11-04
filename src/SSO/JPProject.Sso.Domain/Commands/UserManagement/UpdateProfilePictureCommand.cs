using System;
using JPProject.Sso.Domain.Validations.UserManagement;

namespace JPProject.Sso.Domain.Commands.UserManagement
{
    public class UpdateProfilePictureCommand : ProfileCommand
    {
        public UpdateProfilePictureCommand(string id, string picture)
        {
            Picture = picture;
            Id = id;
        }

        public override bool IsValid()
        {
            ValidationResult = new UpdateProfilePictureCommandValidation().Validate(this);
            return ValidationResult.IsValid;
        }
    }
}