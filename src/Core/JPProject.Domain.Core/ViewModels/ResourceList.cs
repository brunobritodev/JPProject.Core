using System;
using System.Collections.Generic;
using System.Text;

namespace JPProject.Domain.Core.ViewModels
{
    public class ResourceList : List<string>
    {
        public ResourceList(IEnumerable<string> resources)
        {
            this.AddRange(resources);
        }
    }
}
