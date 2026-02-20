using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<AuthDto.Response> Login(AuthDto.Login authDto);
}