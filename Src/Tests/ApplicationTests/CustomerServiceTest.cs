using Application.DTOs;
using Application.Exceptions;
using Application.Services;
using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;

namespace Tests.ApplicationTests;

public class CustomerServiceTest
{
    // ──────────────────────────────────────────────
    //  Infraestructura compartida
    // ──────────────────────────────────────────────
    private readonly IRepository _repository;
    private readonly CustomersService _customerService;

    // Datos de prueba reutilizables
    private readonly Guid _validId = Guid.NewGuid();
    private readonly Customer _fakeCustomer;

    public CustomerServiceTest()
    {
        _repository = Substitute.For<IRepository>();
        _customerService = new CustomersService(_repository);

        _fakeCustomer = new Customer(
            "Juan",
            "Pérez",
            Email.Create("juan@mail.com"),
            Dni.Create("12345678"),
            new Domicilie("Mitre", "Tucumán", 100, "4000"),
            "3814000000"
        );
    }

    // ══════════════════════════════════════════════
    //  GetAllCustomers  (CC = 2)
    // ══════════════════════════════════════════════

    /// <summary>
    /// Camino 1: el repositorio devuelve una lista vacía → el servicio retorna colección vacía.
    /// </summary>
    [Fact]
    public async Task GetAllCustomers_WhenNoCustomersExist_ReturnsEmptyCollection()
    {
        // Arrange
        _repository.GetAll<Customer>().Returns(new List<Customer>());

        // Act
        var result = await _customerService.GetAllCustomers();

        // Assert
        await _repository.Received(1).GetAll<Customer>();
        Assert.Empty(result);
    }

    /// <summary>
    /// Camino 2: el repositorio devuelve clientes → el servicio los mapea a DTOs correctamente.
    /// </summary>
    [Fact]
    public async Task GetAllCustomers_WhenCustomersExist_ReturnsMappedDtos()
    {
        // Arrange
        _repository.GetAll<Customer>().Returns(new List<Customer> { _fakeCustomer });

        // Act
        var result = await _customerService.GetAllCustomers();

        // Assert
        await _repository.Received(1).GetAll<Customer>();
        Assert.Single(result);
        var dto = result.First();
        Assert.Equal(_fakeCustomer.FirstName, dto.firstName);
        Assert.Equal(_fakeCustomer.LastName, dto.lastName);
        Assert.Equal(_fakeCustomer.Email.Value, dto.email);
    }

    // ══════════════════════════════════════════════
    //  GetCustomerById  (CC = 3)
    // ══════════════════════════════════════════════

    /// <summary>
    /// Camino 1: el id recibido no es un Guid válido → lanza FormatInvalidException.
    /// </summary>
    [Fact]
    public async Task GetCustomerById_WhenIdIsInvalid_ThrowsFormatInvalidException()
    {
        // Arrange
        var invalidId = "not-a-guid";

        // Act & Assert
        await Assert.ThrowsAsync<FormatInvalidException>(
            () => _customerService.GetCustomerById(invalidId));
        await _repository.Received(0).GetForId<Customer>(Arg.Any<Guid>());
    }

    /// <summary>
    /// Camino 2: el id es válido pero no existe ningún cliente con ese id → lanza EntityNotFoundException.
    /// </summary>
    [Fact]
    public async Task GetCustomerById_WhenCustomerDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        _repository.GetForId<Customer>(_validId).ReturnsNull();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _customerService.GetCustomerById(_validId.ToString()));
        await _repository.Received(1).GetForId<Customer>(Arg.Any<Guid>());
    }

    /// <summary>
    /// Camino 3: el id es válido y el cliente existe → retorna el DTO con los datos correctos.
    /// </summary>
    [Fact]
    public async Task GetCustomerById_WhenCustomerExists_ReturnsMappedDto()
    {
        // Arrange
        _repository.GetForId<Customer>(_validId).Returns(_fakeCustomer);

        // Act
        var result = await _customerService.GetCustomerById(_validId.ToString());

        // Assert
        await _repository.Received(1).GetForId<Customer>(Arg.Any<Guid>());
        Assert.NotNull(result);
        Assert.Equal(_fakeCustomer.FirstName, result!.firstName);
        Assert.Equal(_fakeCustomer.Email.Value, result.email);
        Assert.Equal(_fakeCustomer.Dni.Value, result.dni);
    }

    // ══════════════════════════════════════════════
    //  Create  (CC = 2)
    // ══════════════════════════════════════════════

    /// <summary>
    /// Camino 1: ya existe un cliente con el mismo email o DNI → lanza BusinessConflictException.
    /// </summary>
    [Fact]
    public async Task Create_WhenEmailOrDniAlreadyExists_ThrowsBusinessConflictException()
    {
        // Arrange
        var request = new CustomerDto.Request(
            "Ana", "López", "juan@mail.com", "12345678",
            "San Martín", "Tucumán", 200, "3815000000", "4000");

        _repository
            .GetTheFirstOne<Customer>(Arg.Any<System.Linq.Expressions.Expression<Func<Customer, bool>>>())
            .Returns(_fakeCustomer);

        // Act & Assert
        await Assert.ThrowsAsync<BusinessConflictException>(
            () => _customerService.Create(request));
        await _repository.Received(1).GetTheFirstOne<Customer>(Arg.Any<Expression<Func<Customer, bool>>>(), Arg.Any<string[]>());
    }

    /// <summary>
    /// Camino 2: los datos son únicos → crea el cliente y retorna el DTO con los datos ingresados.
    /// </summary>
    [Fact]
    public async Task Create_WhenDataIsUnique_ReturnsNewCustomerDto()
    {
        // Arrange
        var request = new CustomerDto.Request(
            "Ana", "López", "ana@mail.com", "87654321",
            "San Martín", "Tucumán", 200, "3815000000", "4000");

        _repository
            .GetTheFirstOne<Customer>(Arg.Any<System.Linq.Expressions.Expression<Func<Customer, bool>>>())
            .ReturnsNull();

        // Act
        var result = await _customerService.Create(request);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.firstName, result!.firstName);
        Assert.Equal(request.email, result.email);
        Assert.Equal(request.dni, result.dni);

        await _repository.Received(1).Add(Arg.Any<Customer>());
    }

    // ══════════════════════════════════════════════
    //  Update  (CC = 3)
    // ══════════════════════════════════════════════

    /// <summary>
    /// Camino 1: el id recibido no es un Guid válido → lanza FormatInvalidException.
    /// </summary>
    [Fact]
    public async Task Update_WhenIdIsInvalid_ThrowsFormatInvalidException()
    {
        // Arrange
        var request = new CustomerDto.RequestUpdate(
            "Carlos", null, null, null, null, null, null, null, null, "not-a-guid");

        // Act & Assert
        await Assert.ThrowsAsync<FormatInvalidException>(
            () => _customerService.Update(request));
        await _repository.Received(0).Update(Arg.Any<Customer>());
    }

    /// <summary>
    /// Camino 2: el id es válido pero no existe el cliente → lanza EntityNotFoundException.
    /// </summary>
    [Fact]
    public async Task Update_WhenCustomerDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        var request = new CustomerDto.RequestUpdate(
            "Carlos", null, null, null, null, null, null, null, null, _validId.ToString());

        _repository.GetForId<Customer>(_validId).ReturnsNull();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _customerService.Update(request));
        await _repository.Received(1).GetForId<Customer>(Arg.Any<Guid>());
    }

    /// <summary>
    /// Camino 3: el id es válido y el cliente existe → actualiza y retorna el DTO actualizado.
    /// </summary>
    [Fact]
    public async Task Update_WhenCustomerExists_ReturnsUpdatedDto()
    {
        // Arrange
        var request = new CustomerDto.RequestUpdate(
            "Carlos", "Gómez", null, null, null, null, null, "3816000000", null, _validId.ToString());

        _repository.GetForId<Customer>(_validId).Returns(_fakeCustomer);

        // Act
        var result = await _customerService.Update(request);

        // Assert
        Assert.NotNull(result);
        await _repository.Received(1).Update(Arg.Any<Customer>());
    }

    // ══════════════════════════════════════════════
    //  Delete  (CC = 3)
    // ══════════════════════════════════════════════

    /// <summary>
    /// Camino 1: el id recibido no es un Guid válido → lanza FormatInvalidException.
    /// </summary>
    [Fact]
    public async Task Delete_WhenIdIsInvalid_ThrowsFormatInvalidException()
    {
        // Arrange
        var invalidId = "not-a-guid";

        // Act & Assert
        await Assert.ThrowsAsync<FormatInvalidException>(
            () => _customerService.Delete(invalidId));
        await _repository.Received(0).Delete(Arg.Any<Customer>());
    }

    /// <summary>
    /// Camino 2: el id es válido pero no existe el cliente → lanza EntityNotFoundException.
    /// </summary>
    [Fact]
    public async Task Delete_WhenCustomerDoesNotExist_ThrowsEntityNotFoundException()
    {
        // Arrange
        _repository.GetForId<Customer>(_validId).ReturnsNull();

        // Act & Assert
        await Assert.ThrowsAsync<EntityNotFoundException>(
            () => _customerService.Delete(_validId.ToString()));
        await _repository.Received(1).GetForId<Customer>(Arg.Any<Guid>());
    }

    /// <summary>
    /// Camino 3: el id es válido y el cliente existe → llama al repositorio para eliminarlo.
    /// </summary>
    [Fact]
    public async Task Delete_WhenCustomerExists_CallsRepositoryDelete()
    {
        // Arrange
        _repository.GetForId<Customer>(_validId).Returns(_fakeCustomer);

        // Act
        await _customerService.Delete(_validId.ToString());

        // Assert
        await _repository.Received(1).GetForId<Customer>(Arg.Any<Guid>());
        await _repository.Received(1).Delete(_fakeCustomer);
    }
}
