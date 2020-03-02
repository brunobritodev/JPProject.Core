using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Domain.ViewModels.Settings;
using System;
using System.IO;
using System.Threading.Tasks;
using JPProject.Domain.Core.Util;

namespace JPProject.Sso.Application.CloudServices.Storage
{
    public class LocalStorageService : IStorageService
    {
        private readonly StorageSettings _privateSettings;

        public LocalStorageService(StorageSettings privateSettings)
        {
            _privateSettings = privateSettings;
        }

        public async Task<string> Upload(FileUploadViewModel image)
        {
            var bytes = Convert.FromBase64String(image.Value);
            var path = Path.Combine(_privateSettings.PhysicalPath, _privateSettings.VirtualPath ?? string.Empty, image.VirtualLocation ?? string.Empty, image.Filename);

            if (!Directory.Exists(Path.GetDirectoryName(path)))
                Directory.CreateDirectory(Path.GetDirectoryName(path));

            await File.WriteAllBytesAsync(path, bytes);


            return _privateSettings.BasePath.UrlPathCombine(_privateSettings.VirtualPath ?? string.Empty, image.VirtualLocation ?? string.Empty, image.Filename);
        }

        public Task RemoveFile(string fileName, string virtualLocation)
        {
            var directory = Path.Combine(_privateSettings.PhysicalPath, _privateSettings.VirtualPath ?? string.Empty, virtualLocation ?? string.Empty);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var file = Path.Combine(directory, fileName);
            if (File.Exists(file))
                File.Delete(file);
            return Task.CompletedTask;
        }
    }
}