using Application.DTOs;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;

namespace Tests.ApplicationTests;

public class RoleServiceTest
{
    private readonly IRepository _repository;
    private readonly IRoleService _roleService;

    private readonly Role _fakeRole;

    public RoleServiceTest()
    {
        _repository = Substitute.For<IRepository>();
        _roleService = new RoleService(_repository);

        _fakeRole = new Role("admin", "Administrador del sistema");
    }

    // ══════════════════════════════════════════════
    //  GetAll  (CC = 2)
    // ══════════════════════════════════════════════

    [Fact]
    public async Task GetAll_WhenNoRolesExist_ReturnsEmptyList()
    {
        // Arrange
        _repository.GetAll<Role>().Returns(new List<Role>());

        // Act
        var result = await _roleService.GetAll();

        // Assert
        Assert.Empty(result);
        await _repository.Received(1).GetAll<Role>();
    }

    [Fact]
    public async Task GetAll_WhenRolesExist_ReturnsMappedDtos()
    {
        // Arrange
        _repository.GetAll<Role>().Returns(new List<Role> { _fakeRole });

        // Act
        var result = await _roleService.GetAll();

        // Assert
        Assert.Single(result);
        var dto = result.First();
        Assert.Equal(_fakeRole.Name, dto.rolName);
        Assert.Equal(_fakeRole.Description, dto.description);
        await _repository.Received(1).GetAll<Role>();
    }

    // ══════════════════════════════════════════════
    //  Create  (CC = 2)
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Create_WhenRoleAlreadyExists_ThrowsBusinessConflictException()
    {
        // Arrange
        var request = new RoleDto.Request("admin", "Administrador del sistema");

        _repository
            .GetTheFirstOne<Role>(Arg.Any<Expression<Func<Role, bool>>>())
            .Returns(_fakeRole);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessConflictException>(
            () => _roleService.Create(request));

        Assert.Equal("The role already exists", exception.Message);
        await _repository.Received(1).GetTheFirstOne<Role>(Arg.Any<Expression<Func<Role, bool>>>());
        await _repository.DidNotReceive().Add(Arg.Any<Role>());
    }

    [Fact]
    public async Task Create_WhenRoleIsUnique_ReturnsNewRoleDto()
    {
        // Arrange
        var request = new RoleDto.Request("supervisor", "Supervisor de ventas");

        _repository
            .GetTheFirstOne<Role>(Arg.Any<Expression<Func<Role, bool>>>())
            .ReturnsNull();

        // Act
        var result = await _roleService.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.rolName, result.rolName);
        Assert.Equal(request.description, result.description);
        await _repository.Received(1).Add(Arg.Any<Role>());
    }
}