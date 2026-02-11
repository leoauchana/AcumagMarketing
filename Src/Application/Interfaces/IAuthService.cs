using Application.DTOs;

namespace Application.Interfaces;

public interface IAuthService
{
    Task<EmployeeDto.Response> Login(AuthDto.Login authDto);
}