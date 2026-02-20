using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Transversal.Interfaces;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IRepository _repository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher; 
    public AuthService(IRepository repository, ITokenService tokenService
        , IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _tokenService = tokenService;
        _passwordHasher = passwordHasher;
    }
    public async Task<AuthDto.Response> Login(AuthDto.Login authDto)
    {
        var user = await _repository.GetTheFirstOne<User>
            (u => u.Username.Equals(authDto.userName), nameof(Employee), "Employee.Role");
        if (user == null || !(_passwordHasher.Verify(authDto.password, user.Password)))
            throw new EntityNotFoundException($"Username or password incorrect");
        var tokenGenerated = _tokenService.GenerateToken(user);
        return new AuthDto.Response(user.Id.ToString(), user.Employee.FirstName, user.Employee.LastName, 
            user.Employee.Role.Name, tokenGenerated); 
    }
}