using System;
using Xunit;
using Catalog.Repositories;
using Catalog.Entities;
using Catalog.Controllers;
using Catalog.Dtos;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FluentAssertions;

namespace Catalog.UnitTests
{
    public class ItemsControllerTests
    {
        private readonly Mock<IItemRepository> repositoryStub = new();
        private readonly Mock<ILogger<ItemsController>> loggerStub = new();
        private readonly Random random = new();
        [Fact]
        public async Task GetItemAsync_WithNonExistingItem_ReturnsNotFound()
        {
            //Arrange
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync((Item)null);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            //Act
            var result = await controller.GetItemAsync(Guid.NewGuid());
            //Assert
            result.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task GetItemAsync_WithExistingItem_ReturnExpectedItem()
        {
            var expectedItem = CreateRandomItem();
            
            repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
                .ReturnsAsync(expectedItem);
            
            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            var result = await controller.GetItemAsync(Guid.NewGuid());
            
            result.Value.Should().BeEquivalentTo(
                expectedItem,
                options => options.ComparingByMembers<Item>());
        }

        [Fact]
        public async Task GetItemAsync_WithExistingItems_ReturnExpectedItem()
        {
            var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem()};
            repositoryStub.Setup(repo => repo.GetItemsAsync())
                .ReturnsAsync(expectedItems);

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);
            
            var result = await controller.GetItemsAsync();

            result.Should().BeEquivalentTo(
                expectedItems,
                options => options.ComparingByMembers<Item>()
            );
        }

        [Fact]
        public async Task CreateItem_WithItemToCreate_RetrunsCreatedItem()
        {
            var itemToCreate = new CreateItemDto()
            {
                Name = Guid.NewGuid().ToString(),
                Price = random.Next(1000)
            };

            var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

            var result = await controller.CreateItemAsync(itemToCreate);

            var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;

            itemToCreate.Should().BeEquivalentTo(
                createdItem,
                optiions => optiions.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
            );
            createdItem.Id.Should().NotBeEmpty();
            createdItem.CreateDate.Should().BeCloseTo(DateTimeOffset.UtcNow, 1000);
        }
        private Item CreateRandomItem()
        {
            return new()
            {
                Id = Guid.NewGuid(),
                Name = Guid.NewGuid().ToString(),
                Price = random.Next(1000),
                CreateDate = DateTimeOffset.UtcNow
            };
        }

    }
}
