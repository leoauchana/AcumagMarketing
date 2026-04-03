using Application.DTOs;
using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;
using Transversal.Interfaces;

namespace Tests.ApplicationTests;

public class AuthServiceTests
{
    private readonly IRepository _repository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly AuthService _authService;

    private readonly Role _fakeRole;
    private readonly Employee _fakeEmployee;
    private readonly User _fakeUser;

    public AuthServiceTests()
    {
        _repository = Substitute.For<IRepository>();
        _tokenService = Substitute.For<ITokenService>();
        _passwordHasher = Substitute.For<IPasswordHasher>();

        _authService = new AuthService(_repository, _tokenService, _passwordHasher);

        _fakeRole = new Role("admin", "Administrador del sistema");

        _fakeEmployee = new Employee(
            "Juan", "Pérez",
            Email.Create("juan@mail.com"),
            Dni.Create("12345678"),
            new Domicilie("Mitre", "Tucumán", 100, "4000"),
            _fakeRole,
            new User("admin1", "admin123")
        );

        _fakeUser = new User("admin1", "admin123");

        // Employee es una propiedad de navegación que EF resuelve en runtime.
        // Como no hay setter público ni constructor que la reciba, se asigna
        // mediante reflection para simular el estado que tendría al venir del repositorio.
        typeof(User)
            .GetProperty(nameof(User.Employee))!
            .SetValue(_fakeUser, _fakeEmployee);
    }

    // ══════════════════════════════════════════════
    //  Login  (CC = 3)
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Login_WhenUserDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        var request = new AuthDto.Login("admin1", "admin123");

        _repository
            .GetTheFirstOne<User>(
                Arg.Any<Expression<Func<User, bool>>>(),
                nameof(Employee), "Employee.Role")
            .ReturnsNull();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _authService.Login(request));

        Assert.Equal("Username or password incorrect", exception.Message);
        await _repository.Received(1).GetTheFirstOne<User>(
            Arg.Any<Expression<Func<User, bool>>>(),
            nameof(Employee), "Employee.Role");
    }

    [Fact]
    public async Task Login_WhenPasswordIsIncorrect_ThrowsEntityNotFoundException()
    {
        // Arrange
        var request = new AuthDto.Login("admin1", "wrongpassword");

        _repository
            .GetTheFirstOne<User>(
                Arg.Any<Expression<Func<User, bool>>>(),
                nameof(Employee), "Employee.Role")
            .Returns(_fakeUser);

        _passwordHasher.Verify(request.password, _fakeUser.Password).Returns(false);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _authService.Login(request));

        Assert.Equal("Username or password incorrect", exception.Message);
        _passwordHasher.Received(1).Verify(request.password, _fakeUser.Password);
    }

    [Fact]
    public async Task Login_WhenCredentialsAreValid_ReturnsAuthResponseWithToken()
    {
        // Arrange
        var request = new AuthDto.Login("admin1", "admin123");
        var fakeToken = "fake.jwt.token";

        _repository
            .GetTheFirstOne<User>(
                Arg.Any<Expression<Func<User, bool>>>(),
                nameof(Employee), "Employee.Role")
            .Returns(_fakeUser);

        _passwordHasher.Verify(request.password, _fakeUser.Password).Returns(true);
        _tokenService.GenerateToken(_fakeUser).Returns(fakeToken);

        // Act
        var result = await _authService.Login(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(fakeToken, result.token);
        _tokenService.Received(1).GenerateToken(_fakeUser);
    }
}