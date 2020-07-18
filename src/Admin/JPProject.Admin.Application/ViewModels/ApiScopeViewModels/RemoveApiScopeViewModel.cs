using IdentityServer4.Models;
using System.ComponentModel.DataAnnotations;

namespace JPProject.Admin.Application.ViewModels.ApiScopeViewModels
{
    public class UpdateApiScopeViewModel
    {
        [Required]
        public string OldName { get; }
        public ApiScope ApiScope { get; }

        public UpdateApiScopeViewModel(string oldName, ApiScope apiScope)
        {
            OldName = oldName;
            ApiScope = apiScope;
        }
    }
}