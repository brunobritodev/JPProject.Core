using JPProject.Domain.Core.Util;

namespace JPProject.Sso.Domain.ViewModels.Settings
{
    public class StorageSettings
    {
        public string Username { get; }
        public string Password { get; }
        public string VirtualPath { get; }
        public StorageProviderService Provider { get; }
        public string StorageName { get; }
        public string BasePath { get; }
        public string Region { get; }
        public string PhysicalPath { get; }

        /// <summary>
        /// Repository settings
        /// </summary>
        /// <param name="username">Service username</param>
        /// <param name="password">Service key</param>
        /// <param name="storageService">S3 / Azure / Local</param>
        /// <param name="storageName">The service name of storage (AWS S3 name)</param>
        /// <param name="virtualPath">For local storage will concate to end of path: https://sso.jpproject.net/virtual-path/ </param>
        /// <param name="basePath">The base path to return in local storage: https://sso.jpproject.net/ </param>
        /// <param name="region">AWS Region</param>
        public StorageSettings(string username, string password, string storageService, string storageName, string physicalPath, string virtualPath, string basePath, string region)
        {
            Username = username;
            Password = password;
            VirtualPath = virtualPath;
            BasePath = basePath;
            Region = region;
            StorageName = storageName;
            PhysicalPath = physicalPath;

            if (storageService.IsPresent())
            {
                if (storageService.Equals("Azure"))
                    Provider = StorageProviderService.Azure;
                if (storageService.Equals("S3"))
                    Provider = StorageProviderService.S3;
                if (storageService.Equals("Local"))
                    Provider = StorageProviderService.Local;
            }
        }
    }
}