using JPProject.Sso.Application.CloudServices.Storage;
using JPProject.Sso.Application.ViewModels;
using JPProject.Sso.Domain.ViewModels.Settings;
using System;
using System.IO;
using System.Threading.Tasks;
using Xunit;

namespace JPProject.Sso.Integration.Tests.CloudServicesTest
{

    public class S3Test
    {

        private string _username;
        private string _password;
        private string _storagename;
        private string _region;

        public S3Test()
        {
            // set data here, AND DO NOT COMMIT. In future I change to get from secrets

        }

        [Fact
        (Skip = "Only for dev tests")
        ]
        public async Task ShouldRemoveFileS3()
        {
            var service = new AwsStorageService(new StorageSettings(_username,
                _password, "S3", _storagename,
                null, null, null, _region));

            await service.RemoveFile(@"oauth-2-sm.png", "images");
        }

        [Fact
        (Skip = "Only for dev tests")
        ]
        public async Task ShouldUploadFileS3()
        {
            var service = new AwsStorageService(new StorageSettings(_username,
                _password, "S3", _storagename,
                null, null, null, _region));

            var file = new FileUploadViewModel()
            {
                Filename = "oauth-2-sm.png",
                FileType = "image/png",
                Value = Convert.ToBase64String(File.ReadAllBytes(@"D:\oauth-2-sm.png")),
                VirtualLocation = "images"
            };
            await service.Upload(file);
        }

    }
}
