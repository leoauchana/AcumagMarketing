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

public class EmployeeServiceTests
{
    private readonly IRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly EmployeeService _employeeService;

    private readonly Guid _validId = Guid.NewGuid();
    private readonly Role _fakeRole;
    private readonly Employee _fakeEmployee;

    public EmployeeServiceTests()
    {
        _repository = Substitute.For<IRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _employeeService = new EmployeeService(_repository, _passwordHasher);

        _fakeRole = new Role("admin", "Administrador del sistema");

        _fakeEmployee = new Employee(
            "Juan", "Pérez",
            Email.Create("juan@mail.com"),
            Dni.Create("12345678"),
            new Domicilie("Mitre", "Tucumán", 100, "4000"),
            _fakeRole,
            new User("admin1", "admin123")
        );
    }

    // ══════════════════════════════════════════════
    //  GetAllEmployees  (CC = 2)
    // ══════════════════════════════════════════════

    [Fact]
    public async Task GetAllEmployees_WhenNoEmployeesExist_ReturnsEmptyList()
    {
        // Arrange
        _repository.ListAll<Employee>(nameof(Role))
            .Returns(new List<Employee>());

        // Act
        var result = await _employeeService.GetAllEmployees();

        // Assert
        Assert.Empty(result);
        await _repository.Received(1).ListAll<Employee>(nameof(Role));
    }

    [Fact]
    public async Task GetAllEmployees_WhenEmployeesExist_ReturnsMappedDtos()
    {
        // Arrange
        _repository.ListAll<Employee>(nameof(Role))
            .Returns(new List<Employee> { _fakeEmployee });

        // Act
        var result = await _employeeService.GetAllEmployees();

        // Assert
        Assert.Single(result);
        var dto = result.First();
        Assert.Equal(_fakeEmployee.FirstName, dto.fisrtName);
        Assert.Equal(_fakeEmployee.LastName, dto.lastName);
        Assert.Equal(_fakeEmployee.Email.Value, dto.email);
        Assert.Equal(_fakeEmployee.Role.Name, dto.role);
        await _repository.Received(1).ListAll<Employee>(nameof(Role));
    }

    // ══════════════════════════════════════════════
    //  GetEmployeeById  (CC = 3)
    // ══════════════════════════════════════════════

    [Fact]
    public async Task GetEmployeeById_WhenIdIsInvalid_ThrowsFormatInvalidException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<FormatInvalidException>(
            () => _employeeService.GetEmployeeById("not-a-guid"));
    }

    [Fact]
    public async Task GetEmployeeById_WhenEmployeeDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        _repository.GetForId<Employee>(_validId, nameof(Role))
            .ReturnsNull();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _employeeService.GetEmployeeById(_validId.ToString()));

        await _repository.Received(1).GetForId<Employee>(Arg.Any<Guid>(), nameof(Role));
        Assert.Contains(_validId.ToString(), exception.Message);
    }

    [Fact]
    public async Task GetEmployeeById_WhenEmployeeExists_ReturnsMappedDto()
    {
        // Arrange
        _repository.GetForId<Employee>(_validId, nameof(Role))
            .Returns(_fakeEmployee);

        // Act
        var result = await _employeeService.GetEmployeeById(_validId.ToString());

        // Assert
        Assert.NotNull(result);
        Assert.Equal(_fakeEmployee.FirstName, result!.fisrtName);
        Assert.Equal(_fakeEmployee.Email.Value, result.email);
        Assert.Equal(_fakeEmployee.Dni.Value, result.dni);
        await _repository.Received(1).GetForId<Employee>(Arg.Any<Guid>(), nameof(Role));
    }

    // ══════════════════════════════════════════════
    //  Create  (CC = 4)
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Create_WhenEmailOrDniAlreadyExists_ThrowsBusinessConflictException()
    {
        // Arrange
        var request = new EmployeeDto.Request(
            "Juan", "Pérez", "juan@mail.com", "12345678",
            "Tucumán", "Mitre", 100, "4000",
            _fakeRole.Id.ToString(), "admin1", "admin123");

        _repository
            .GetTheFirstOne<Employee>(Arg.Any<Expression<Func<Employee, bool>>>())
            .Returns(_fakeEmployee);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessConflictException>(
            () => _employeeService.Create(request));

        Assert.Equal("The email or dni already exists", exception.Message);
        await _repository.Received(1).GetTheFirstOne<Employee>(Arg.Any<Expression<Func<Employee, bool>>>());
    }

    [Fact]
    public async Task Create_WhenRoleIdIsInvalid_ThrowsFormatInvalidException()
    {
        // Arrange
        var request = new EmployeeDto.Request(
            "Juan", "Pérez", "juan@mail.com", "12345678",
            "Tucumán", "Mitre", 100, "4000",
            "not-a-guid", "admin1", "admin123");

        _repository
            .GetTheFirstOne<Employee>(Arg.Any<Expression<Func<Employee, bool>>>())
            .ReturnsNull();

        // Act & Assert
        await _repository.Received(0).GetForId<Role>(Arg.Any<Guid>());
        await Assert.ThrowsAsync<FormatInvalidException>(
            () => _employeeService.Create(request));
    }

    [Fact]
    public async Task Create_WhenRoleDoesNotExist_ThrowsBusinessConflictException()
    {
        // Arrange
        var request = new EmployeeDto.Request(
            "Juan", "Pérez", "juan@mail.com", "12345678",
            "Tucumán", "Mitre", 100, "4000",
            _fakeRole.Id.ToString(), "admin1", "admin123");

        _repository
            .GetTheFirstOne<Employee>(Arg.Any<Expression<Func<Employee, bool>>>())
            .ReturnsNull();

        _repository.GetForId<Role>(_fakeRole.Id)
            .ReturnsNull();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessConflictException>(
            () => _employeeService.Create(request));

        Assert.Equal("The role is not exists", exception.Message);
        await _repository.Received(1).GetForId<Role>(Arg.Any<Guid>());
    }

    [Fact]
    public async Task Create_WhenDataIsValid_ReturnsNewEmployeeDto()
    {
        // Arrange
        var request = new EmployeeDto.Request(
            "Juan", "Pérez", "juan@mail.com", "12345678",
            "Tucumán", "Mitre", 100, "4000",
            _fakeRole.Id.ToString(), "admin1", "admin123");

        _repository
            .GetTheFirstOne<Employee>(Arg.Any<Expression<Func<Employee, bool>>>())
            .ReturnsNull();

        _repository.GetForId<Role>(_fakeRole.Id)
            .Returns(_fakeRole);

        // Act
        var result = await _employeeService.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.firstName, result!.fisrtName);
        Assert.Equal(request.email, result.email);
        Assert.Equal(request.dni, result.dni);
        await _repository.Received(1).Add(Arg.Any<Employee>());
    }

    // ══════════════════════════════════════════════
    //  Update  (CC = 5)
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Update_WhenEmployeeIdIsInvalid_ThrowsFormatInvalidException()
    {
        // Arrange
        var request = new EmployeeDto.RequestUpdate("not-a-guid", _fakeRole.Id.ToString());

        // Act & Assert
        await _repository.Received(0).GetForId<Employee>(Arg.Any<Guid>());
        await Assert.ThrowsAsync<FormatInvalidException>(
            () => _employeeService.Update(request));
    }

    [Fact]
    public async Task Update_WhenEmployeeDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        var request = new EmployeeDto.RequestUpdate(_validId.ToString(), _fakeRole.Id.ToString());

        _repository.GetForId<Employee>(_validId, nameof(Role))
            .ReturnsNull();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _employeeService.Update(request));

        Assert.Contains(_validId.ToString(), exception.Message);
        await _repository.Received(1).GetForId<Employee>(Arg.Any<Guid>(), nameof(Role));
    }

    [Fact]
    public async Task Update_WhenRoleIdIsInvalid_ThrowsFormatInvalidException()
    {
        // Arrange
        var request = new EmployeeDto.RequestUpdate(_validId.ToString(), "not-a-guid");

        _repository.GetForId<Employee>(_validId, nameof(Role))
            .Returns(_fakeEmployee);

        // Act & Assert
        await _repository.Received(0).GetForId<Role>(Arg.Any<Guid>());
        await Assert.ThrowsAsync<FormatInvalidException>(
            () => _employeeService.Update(request));
    }

    [Fact]
    public async Task Update_WhenRoleDoesNotExist_ThrowsBusinessConflictException()
    {
        // Arrange
        var request = new EmployeeDto.RequestUpdate(_validId.ToString(), _fakeRole.Id.ToString());

        _repository.GetForId<Employee>(_validId, nameof(Role))
            .Returns(_fakeEmployee);

        _repository.GetForId<Role>(_fakeRole.Id)
            .ReturnsNull();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessConflictException>(
            () => _employeeService.Update(request));
        await _repository.Received(1).GetForId<Role>(Arg.Any<Guid>());
        Assert.Equal("The role is not exists", exception.Message);
    }

    [Fact]
    public async Task Update_WhenDataIsValid_ReturnsUpdatedEmployeeDto()
    {
        // Arrange
        var newRole = new Role("supervisor", "Supervisor de ventas");
        var request = new EmployeeDto.RequestUpdate(_validId.ToString(), newRole.Id.ToString());

        _repository.GetForId<Employee>(_validId, nameof(Role))
            .Returns(_fakeEmployee);

        _repository.GetForId<Role>(newRole.Id)
            .Returns(newRole);

        // Act
        var result = await _employeeService.Update(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(newRole.Name, result!.role);
        await _repository.Received(1).Update(Arg.Any<Employee>());
    }

    // ══════════════════════════════════════════════
    //  Delete  (CC = 3)
    // ══════════════════════════════════════════════

    [Fact]
    public async Task Delete_WhenIdIsInvalid_ThrowsFormatInvalidException()
    {
        // Act & Assert
        await Assert.ThrowsAsync<FormatInvalidException>(
            () => _employeeService.Delete("not-a-guid"));
        await _repository.Received(0).GetForId<Employee>(Arg.Any<Guid>());
    }

    [Fact]
    public async Task Delete_WhenEmployeeDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        _repository.GetForId<Employee>(_validId)
            .ReturnsNull();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _employeeService.Delete(_validId.ToString()));

        Assert.Contains(_validId.ToString(), exception.Message);
        await _repository.Received(1).GetForId<Employee>(Arg.Any<Guid>());
    }

    [Fact]
    public async Task Delete_WhenEmployeeExists_CallsRepositoryDelete()
    {
        // Arrange
        _repository.GetForId<Employee>(_validId)
            .Returns(_fakeEmployee);

        // Act
        await _employeeService.Delete(_validId.ToString());

        // Assert
        await _repository.Received(1).Delete(_fakeEmployee);
    }
}