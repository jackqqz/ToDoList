using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ToDoList.API.Controllers;
using ToDoList.Application.Commands;
using ToDoList.Application.Queries;
using ToDoList.Core.Models;
using Xunit;

namespace ToDoList.UnitTests.Controllers
{
    public class ToDoItemControllerTests
    {
        private readonly Mock<ISender> _sender = new();
        private readonly ToDoItemController _controller;

        public ToDoItemControllerTests()
        {
            _controller = new ToDoItemController(_sender.Object);
        }

        [Fact]
        public async Task AddToDoItemAsync_returns_Ok()
        {
            _sender.Setup(s => s.Send(It.IsAny<AddToDoItemCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ToDoItem { Id = Guid.NewGuid(), Title = "Task1" });

            var result = await _controller.AddToDoItemAsync(new ToDoItem { Title = "Task1" });
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetAllToDoItemsAsync_returns_list()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetAllToDoItemsQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new List<ToDoItem> { new() { Title = "Task1" } });

            var result = await _controller.GetAllToDoItemsAsync();

            var ok = result as OkObjectResult;
            (ok!.Value as IEnumerable<ToDoItem>).Should().NotBeEmpty();
        }

        [Fact]
        public async Task GetToDoItemByIdAsync_returns_NotFound_when_missing()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetToDoItemByIdQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync((ToDoItem?)null);

            var result = await _controller.GetToDoItemByIdAsync(Guid.NewGuid());
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task UpdateToDoItemAsync_returns_Ok()
        {
            _sender.Setup(s => s.Send(It.IsAny<UpdateToDoItemCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ToDoItem { Id = Guid.NewGuid(), Title = "Updated" });

            var result = await _controller.UpdateToDoItemAsync(Guid.NewGuid(), new ToDoItem { Title = "Updated" });
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DeleteToDoItemAsync_returns_Ok()
        {
            _sender.Setup(s => s.Send(It.IsAny<DeleteToDoItemCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(true);

            var result = await _controller.DeleteToDoItemAsync(Guid.NewGuid());
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
