using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Domain.ViewModels.Settings;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
using JPProject.Domain.Core.Util;

namespace JPProject.Sso.Application.CloudServices.Storage
{
    public class AzureStorageService : IStorageService
    {
        private readonly StorageSettings _privateSettings;

        public AzureStorageService(StorageSettings privateSettings)
        {
            _privateSettings = privateSettings;
        }


        public async Task<string> Upload(FileUploadViewModel file)
        {
            var container = await GetBlobContainer(file.VirtualLocation);


            var newFile = container.GetBlockBlobReference(file.Filename);
            byte[] imageBytes = Convert.FromBase64String(file.Value);
            newFile.Properties.ContentType = file.FileType;
            await newFile.UploadFromByteArrayAsync(imageBytes, 0, imageBytes.Length);

            return newFile.StorageUri.PrimaryUri.AbsoluteUri;
        }

        private async Task<CloudBlobContainer> GetBlobContainer(string virtualLocation)
        {
            var storageCredentials = new StorageCredentials(_privateSettings.Username, _privateSettings.Password);
            var cloudStorageAccount = new CloudStorageAccount(storageCredentials, true);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var container = cloudBlobClient.GetContainerReference(virtualLocation ?? string.Empty);
            await container.CreateIfNotExistsAsync();
            return container;
        }

        public async Task RemoveFile(string fileName, string virtualLocation)
        {
            var container = await GetBlobContainer(virtualLocation);
            if (fileName.IsPresent())
            {
                var file = Path.GetFileName(fileName);
                var oldFile = container.GetBlockBlobReference(file);
                await oldFile.DeleteIfExistsAsync();
            }
        }
    }
}