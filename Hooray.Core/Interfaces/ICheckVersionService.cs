using Hooray.Core.ViewModels;
using System.Threading.Tasks;

namespace Hooray.Core.Interfaces
{
    public interface ICheckVersionService
    {
        Task<CheckVersionViewModel> CheckVersion(string versionapp, string devicetype, string lang);
    }
}
