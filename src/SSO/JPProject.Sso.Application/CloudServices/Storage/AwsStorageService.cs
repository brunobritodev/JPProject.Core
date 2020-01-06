using Amazon.S3;
using Amazon.S3.Model;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Domain.ViewModels.Settings;
using System;
using System.IO;
using System.Threading.Tasks;

namespace JPProject.Sso.Application.CloudServices.Storage
{
    public class AwsStorageService : IStorageService
    {
        private readonly StorageSettings _privateSettings;

        public AwsStorageService(StorageSettings privateSettings)
        {
            _privateSettings = privateSettings;
        }

        public async Task<string> Upload(FileUploadViewModel image)
        {
            var client = GetClient();

            var putRequest = new PutObjectRequest
            {
                BucketName = _privateSettings.StorageName,
                Key = image.Filename,
                ContentType = image.FileType,
                InputStream = new MemoryStream(Convert.FromBase64String(image.Value)),
                CannedACL = S3CannedACL.PublicRead,
            };
            putRequest.Metadata.Add("Location", image.VirtualLocation);
            var response = await client.PutObjectAsync(putRequest);

            return Path.Combine(_privateSettings.StorageName, image.VirtualLocation, image.Filename);
        }

        private IAmazonS3 GetClient()
        {
            return new AmazonS3Client(_privateSettings.Username, _privateSettings.Password);

        }


        public async Task RemoveFile(string fileName, string virtualLocation)
        {
            var client = GetClient();
            await client.DeleteObjectAsync(new DeleteObjectRequest() { BucketName = virtualLocation, Key = fileName });
        }
    }
}