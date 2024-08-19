using BCS.Core.BaseProvider;
using BCS.Core.Utilities;
using BCS.Entity.DomainModels;
using BCS.Entity.DTO.Project;
using System.Threading.Tasks;

namespace BCS.Business.IServices
{
    public partial interface ISys_UserService
    {

        Task<WebResponseContent> Login(LoginInfo loginInfo, bool verificationCode = true);
        Task<WebResponseContent> ReplaceToken();
        Task<WebResponseContent> ModifyPwd(string oldPwd, string newPwd);
        Task<WebResponseContent> GetCurrentUserInfo();
        Task<WebResponseContent> GetPagerList(PageDataOptions pageDataOptions);
        Task<WebResponseContent> GetUsersByRole(string roleName);
    }
}

