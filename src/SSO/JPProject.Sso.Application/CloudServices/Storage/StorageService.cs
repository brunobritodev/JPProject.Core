using JPProject.Sso.Application.Interfaces;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Domain.ViewModels.Settings;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.CloudServices.Storage
{
    public class StorageService : IStorage
    {
        private readonly IGlobalConfigurationAppService _globalConfigurationAppService;
        public StorageService(IGlobalConfigurationAppService globalConfigurationAppService)
        {
            _globalConfigurationAppService = globalConfigurationAppService;
        }
        public async Task<string> Upload(FileUploadViewModel image)
        {
            var settings = await _globalConfigurationAppService.GetPrivateSettings();
            var provider = GetProvider(settings.Storage.Provider, settings);
            return await provider.Upload(image);
        }

        private IStorageService GetProvider(StorageProviderService storageProvider, PrivateSettings settings)
        {
            switch (storageProvider)
            {
                case StorageProviderService.Azure:
                    return new AzureStorageService(settings.Storage);
                case StorageProviderService.S3:
                    return new AwsStorageService(settings.Storage);
                default:
                    return new LocalStorageService(settings.Storage);
            }
        }

        public async Task Remove(string filename, string virtualLocation)
        {
            var settings = await _globalConfigurationAppService.GetPrivateSettings();
            var provider = GetProvider(settings.Storage.Provider, settings);
            await provider.RemoveFile(filename, virtualLocation);
        }
    }
}
