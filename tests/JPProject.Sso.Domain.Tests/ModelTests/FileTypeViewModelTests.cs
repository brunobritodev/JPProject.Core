using FluentAssertions;
using JPProject.Sso.Application.ViewModels;
using System.IO;
using Xunit;

namespace JPProject.Sso.Domain.Tests.ModelTests
{
    public class FileTypeViewModelTests
    {
        [Fact]
        public void ShouldNormalizeFileTypePng()
        {
            var file = new FileUploadViewModel()
            {
                Filename = "oauth-2-sm.png",
                FileType = "image/png",
            };

            file.Normalize();

            Path.GetExtension(file.Filename).Should().Be(".png");

        }

        [Fact]
        public void ShouldNormalizeFileTypePdf()
        {
            var file = new FileUploadViewModel()
            {
                Filename = "oauth-2-sm.pdf",
            };

            file.Normalize();

            Path.GetExtension(file.Filename).Should().Be(".pdf");
        }
        [Fact]
        public void ShouldNormalizeChangeFilename()
        {
            var file = new FileUploadViewModel()
            {
                Filename = "oauth-2-sm.pdf",
            };
            var originalName = file.Filename;
            file.Normalize();

            Path.GetFileName(file.Filename).Should().NotBe(originalName);
        }
    }
}
