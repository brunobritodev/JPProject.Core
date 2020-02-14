using IdentityServer4.EntityFramework.Entities;
using JPProject.EntityFrameworkCore.Interfaces;
using JPProject.Sso.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace JPProject.Sso.Infra.Data.Interfaces
{
    public interface ISsoContext : IJpEntityFrameworkStore
    {
        DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }
        DbSet<Template> Templates { get; set; }
        DbSet<Email> Emails { get; set; }
        DbSet<GlobalConfigurationSettings> GlobalConfigurationSettings { get; set; }
    }
}
