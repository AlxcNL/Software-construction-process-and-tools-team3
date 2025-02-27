using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using Moq;
using Moq.Protected;
using Xunit;
using System.Threading;

public class Items_Unit_Tests
{
    private readonly HttpClient _client;
    private readonly Mock<HttpMessageHandler> _handlerMock;

    public Items_Unit_Tests()
    {
        _handlerMock = new Mock<HttpMessageHandler>();
        _client = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://localhost:5000/api/v2/") };
        var apiKey = "a1b2c3d4e5";  
        _client.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
    }

    [Fact]
    public async Task GetAllItems_ShouldReturnSuccess()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("[{\"id\":1, \"code\":\"ITEM123\", \"description\":\"Item 1\"}, {\"id\":2, \"code\":\"ITEM124\", \"description\":\"Item 2\"}]")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.GetAsync("items");

        // Assert
        response.EnsureSuccessStatusCode();
        var items = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(items));
    }

    [Fact]
    public async Task GetItemById_ShouldReturnSuccess()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":1, \"code\":\"ITEM123\", \"description\":\"Item 1\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.GetAsync("items/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var item = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(item));
    }

    [Fact]
    public async Task AddNewItem_ShouldReturnSuccess()
    {
        // Arrange
        var newItem = new
        {
            Code = "NEW123",
            Description = "New high-quality item",
            ShortDescription = "New Item",
            UpcCode = "987654321012",
            ModelNumber = "NI-2024",
            CommodityCode = "NEW-COMMODITY",
            ItemLineId = 1,
            ItemGroupId = 1,
            ItemTypeId = 1,
            UnitPurchaseQuantity = 1,
            UnitOrderQuantity = 1,
            PackOrderQuantity = 1,
            SupplierId = 1,
            SupplierCode = "SUPNEW",
            SupplierPartNumber = "NI-2024-PART",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.Created)
        {
            Content = new StringContent("{\"id\":1, \"code\":\"NEW123\", \"description\":\"New high-quality item\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.PostAsJsonAsync("items", newItem);

        // Assert
        response.EnsureSuccessStatusCode();
        var createdItem = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(createdItem));
    }

    [Fact]
    public async Task UpdateItem_ShouldReturnSuccess()
    {
        // Arrange
        var updatedItem = new
        {
            Id = 1,
            Code = "UPDATED123",
            Description = "Updated description for item",
            ShortDescription = "Updated Item",
            UpcCode = "987654321013",
            ModelNumber = "UI-2024",
            CommodityCode = "UPDATED-COMMODITY",
            ItemLineId = 1,
            ItemGroupId = 2,
            ItemTypeId = 1,
            UnitPurchaseQuantity = 2,
            UnitOrderQuantity = 2,
            PackOrderQuantity = 2,
            SupplierId = 2,
            SupplierCode = "SUPUPDATED",
            SupplierPartNumber = "UI-2024-PART",
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow
        };

        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent("{\"id\":1, \"code\":\"UPDATED123\", \"description\":\"Updated description for item\"}")
        };

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.PutAsJsonAsync("items/1", updatedItem);

        // Assert
        response.EnsureSuccessStatusCode();
        var updatedItemResponse = await response.Content.ReadAsStringAsync();
        Assert.False(string.IsNullOrEmpty(updatedItemResponse));
    }

    [Fact]
    public async Task DeleteItem_ShouldReturnSuccess()
    {
        // Arrange
        var responseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.NoContent);

        _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(responseMessage);

        // Act
        var response = await _client.DeleteAsync("items/1");

        // Assert
        response.EnsureSuccessStatusCode();
    }
}
