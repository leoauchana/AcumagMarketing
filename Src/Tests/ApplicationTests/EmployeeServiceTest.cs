using Domain.Interfaces;
using Transversal.Interfaces;
using NSubstitute;
using Application.Services;
using Application.DTOs;
using Domain.Entities;
using Domain.ValueObjects;
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
}


