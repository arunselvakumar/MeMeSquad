﻿namespace Jericho.Services.Interfaces
{
    using System.Threading.Tasks;

    using Jericho.Identity;

    using Models.v1;
    using Providers;

    public interface IUserService
    { 
        Task<ServiceResult<AuthTokenModel>> SaveUserAsync(ApplicationUser user, string password);

        Task<ServiceResult<AuthTokenModel>> AuthorizeUserAsync(string username, string password);

        Task<ServiceResult<object>> ActivateEmailAsync(string id, string token);

        Task<ServiceResult<ApplicationUser>> GetUserByIdAsync(string id);

        Task<ServiceResult<ApplicationUser>> GetUserByUserNameAsync(string username);

        Task<ServiceResult<object>> ChangePasswordAsync(string userId, string oldPassword, string newPassword);

        Task<ServiceResult<object>> ChangeEmailAddressAsync(string newEmailAddress);

        Task<ServiceResult<object>> ForgotPasswordAsync(string username);

        Task<ServiceResult<object>> ResetPasswordAsync(string token, string username, string password);
    }
}