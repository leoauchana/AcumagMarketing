using Application.DTOs;
using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using NSubstitute;
using System.Linq.Expressions;
using Transversal.Interfaces;
namespace Tests.ApplicationTests;

public class EmployeeServiceTest
{
    private readonly IRepository _repository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly EmployeeService _employeeService;
    public EmployeeServiceTest()
    {
        _repository = Substitute.For<IRepository>();
        _passwordHasher = Substitute.For<IPasswordHasher>();

        _employeeService = new EmployeeService(_repository, _passwordHasher);
    }
    [Fact]
    public async Task GetAllEmployees_WhenGetAllEmployees_AndExistingACount_ThenReturnAListWithTheEmployees()
    {
        //Arrange   
        var responseRepo = new List<Employee>()
        {
            new Employee("Juan", "Perez", Email.Create("juan@gmail.com"), Dni.Create("45478412"), new Domicilie("Av Sarmiento", "San Miguel",
            4000, "4000"), new Role("admin","Administrador del sistema"), new User("admin1", "admin123"))
        };
        var responseDto = new List<EmployeeDto.Response>()
        {
            new EmployeeDto.Response("3496214C-E2D3-412A-977D-86E53F6BA7F1","Juan", "Perez", "juan@gmail.com", "45478412", "San Miguel", "Av Sarmiento",
            4000, "admin")
        };
        _repository.ListAll<Employee>(nameof(Role))
            .Returns(Task.FromResult(responseRepo));
        //Act
        List<EmployeeDto.Response> responseList = await _employeeService.GetAllEmployees();
        //Asert
        await _repository.Received(1).ListAll<Employee>(nameof(Role));
        Assert.NotNull(responseList);
        Assert.Equal(responseDto.Count, responseList.Count);
        Assert.Equal(responseDto, responseList,
            (expected, actual) => expected.fisrtName.Equals(actual.fisrtName));
    }
    [Fact]
    public async Task GetById_WhenGetEmployeeById_AndExisting_ThenReturnTheEmployees()
    {
        //Arrange   
        string id = "3496214C-E2D3-412A-977D-86E53F6BA7F1";
        var responseRepo = new Employee("Juan", "Perez", Email.Create("juan@gmail.com"), Dni.Create("45478412"), new Domicilie("Av Sarmiento", "San Miguel",
            4000, "4000"), new Role("admin", "Administrador del sistema"), new User("admin1", "admin123"));
        var responseDto = new EmployeeDto.Response("3496214C-E2D3-412A-977D-86E53F6BA7F1", "Juan", "Perez", "juan@gmail.com", "45478412", "San Miguel", "Av Sarmiento",
            4000, "admin");

        _repository.GetForId<Employee>(Guid.Parse(id), nameof(Role))!
            .Returns(Task.FromResult(responseRepo));
        //Act
        var responseEmployee = await _employeeService.GetEmployeeById(id);
        //Asert
        await _repository.Received(1).GetForId<Employee>(Arg.Any<Guid>(), nameof(Role));
        Assert.NotNull(responseEmployee);
        Assert.Equal(responseDto, responseEmployee,
            (expected, actual) => expected.dni == actual.dni);
    }
    [Fact]
    public async Task GetById_WhenGetEmployeeById_AndNotExisting_ThenThrowEntityNotFoundException()
    {
        //Arrange   
        string id = "3496214C-E2D3-412A-977D-86E53F6BA7F1";

        _repository.GetForId<Employee>(Guid.Parse(id), nameof(Role))!
            .Returns(Task.FromResult<Employee>(null!));
        //Act & Assert
        var exception = await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _employeeService.GetEmployeeById(id));
        await _repository.Received(1).GetForId<Employee>(Arg.Any<Guid>(), nameof(Role));
        Assert.Equal("The customer with id 3496214c-e2d3-412a-977d-86e53f6ba7f1 was not found", exception.Message);
    }
    [Fact]
    public async Task Create_WhenCreateNewEmployee_AndNotExisting_ThenReturnTheNewEmployee()
    {
        //Arrange   
        var roleFound = new Role("admin", "Administrador del sistema");
        var requestDto = new EmployeeDto.Request("Juan", "Perez", "juan@gmail.com", "45478412", "San Miguel", "Av Sarmiento",
            58, "4000", roleFound.Id.ToString(), "admin1", "admin123");
        var newEmployee = new Employee("Juan", "Perez", Email.Create("juan@gmail.com"), Dni.Create("45478412"), new Domicilie("Av Sarmiento", "San Miguel",
            58, "4000"), new Role("admin", "Administrador del sistema"), new User("admin1", "admin123"));
        var responseDto = new EmployeeDto.Response(newEmployee.Id.ToString(), "Juan", "Perez", "juan@gmail.com", "45478412", "San Miguel", "Av Sarmiento",
            58, "admin");
        _repository.Add(newEmployee)
            .Returns(Task.CompletedTask);
        _repository.GetForId<Role>(roleFound.Id)
            .Returns(roleFound);
        //Act
        var serviceResponse = await _employeeService.Create(requestDto);
        //Asert
        await _repository.Received(1).Add(Arg.Any<Employee>());
        await _repository.Received(1).GetForId<Role>(Arg.Any<Guid>());
        Assert.NotNull(serviceResponse);
        Assert.Equal(responseDto, serviceResponse,
            (expected, actual) => expected!.dni.Equals(actual!.dni));
    }
    [Fact]
    public async Task Create_WhenCreateNewEmployee_AndExisting_ThenThrowBusinessConflictException()
    {
        //Arrange   
        var requestDto = new EmployeeDto.Request("Juan", "Perez", "juan@gmail.com", "45478412", "San Miguel", "Av Sarmiento",
            58, "4000", "30833CB1-6542-48C2-BB96-8B0742437616", "admin1", "admin123");
        var employeeFound = new Employee("Juan", "Perez", Email.Create("juan@gmail.com"), Dni.Create("45478412"), new Domicilie("Av Sarmiento", "San Miguel",
            58, "4000"), new Role("admin", "Administrador del sistema"), new User("admin1", "admin123"));
        var email = Email.Create(requestDto.email);
        var dni = Dni.Create(requestDto.dni);
        _repository.GetTheFirstOne<Employee>(Arg.Any<Expression<Func<Employee, bool>>>(), Arg.Any<string[]>())!
            .Returns(Task.FromResult(employeeFound));
        //Act && Assert
        var exception = await Assert.ThrowsAsync<BusinessConflictException>(
            () => _employeeService.Create(requestDto));
        await _repository.Received(1).GetTheFirstOne<Employee>(Arg.Any<Expression<Func<Employee, bool>>>(), Arg.Any<string[]>());
        Assert.Equal("The email or dni already exists", exception.Message);
    }
    [Fact]
    public async Task Create_WhenCreateNewEmployeeWithARole_AndNotExistingTheRole_ThenThrowBusinessConfilctException()
    {
        //Arrange   
        var roleFound = new Role("admin", "Administrador del sistema");
        var requestDto = new EmployeeDto.Request("Juan", "Perez", "juan@gmail.com", "45478412", "San Miguel", "Av Sarmiento",
            58, "4000", "63F2D270-07BC-41C8-BF76-B1461037D248", "admin1", "admin123");
        _repository.GetTheFirstOne<Employee>(Arg.Any<Expression<Func<Employee, bool>>>(), Arg.Any<string[]>())!
            .Returns(Task.FromResult<Employee>(null!));
        _repository.GetForId<Role>(Guid.Parse("63F2D270-07BC-41C8-BF76-B1461037D248"))!
            .Returns(Task.FromResult<Role>(null!));
        //Act && Assert
        var exception = await Assert.ThrowsAsync<BusinessConflictException>(
            () => _employeeService.Create(requestDto));
        await _repository.Received(1).GetTheFirstOne<Employee>(Arg.Any<Expression<Func<Employee, bool>>>(), Arg.Any<string[]>());
        await _repository.Received(1).GetForId<Role>(Arg.Any<Guid>());
        Assert.Equal("The role is not exists", exception.Message);
    }
}