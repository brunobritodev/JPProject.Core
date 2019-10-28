using JPProject.Sso.Domain.Commands.UserManagement;

namespace JPProject.Sso.Domain.Validations.UserManagement
{
    public class UpdateProfilePictureCommandValidation : ProfileValidation<UpdateProfilePictureCommand>
    {
        public UpdateProfilePictureCommandValidation()
        {
            ValidatePicture();
        }
    }
}