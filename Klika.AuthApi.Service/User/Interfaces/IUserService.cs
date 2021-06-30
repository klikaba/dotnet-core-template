using IdentityModel.Client;
using Klika.AuthApi.Model.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Klika.AuthApi.Service.User.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> Register(ApplicationUserDTO user);
        Task<TokenResponse> Token(TokenRequestDTO request);
        Task<TokenResponse> RefreshToken(TokenRequestDTO request);
        Task<TokenRevocationResponse> RevokeToken(TokenRequestDTO request);
    }
}
