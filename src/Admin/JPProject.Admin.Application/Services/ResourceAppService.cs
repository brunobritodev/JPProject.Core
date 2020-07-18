using JPProject.Admin.Application.Interfaces;
using JPProject.Admin.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JPProject.Admin.Application.Services
{
    public class ScopesAppService : IScopesAppService
    {
        private readonly IResourceRepository _resourceRepository;

        public ScopesAppService(
            IResourceRepository resourceRepository)
        {
            _resourceRepository = resourceRepository;
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public async Task<IEnumerable<string>> GetScopes(string search)
        {
            var resources = await _resourceRepository.Search(search);
            return resources.OrderBy(a => a);
        }
    }
}