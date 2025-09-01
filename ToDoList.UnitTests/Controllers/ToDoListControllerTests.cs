using System;
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
    public class ToDoListsControllerTests
    {
        private readonly Mock<ISender> _sender = new();
        private readonly ToDoListsController _controller;

        public ToDoListsControllerTests()
        {
            _controller = new ToDoListsController(_sender.Object);
        }

        [Fact]
        public async Task CreateAsync_returns_Ok_with_list()
        {
            var created = new ToDoListEntity { Id = Guid.NewGuid(), Title = "Work" };
            _sender.Setup(s => s.Send(It.IsAny<CreateToDoListCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(created);

            var result = await _controller.CreateAsync("Work");

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            (ok!.Value as ToDoListEntity)!.Title.Should().Be("Work");
        }

        [Fact]
        public async Task CreateAsync_returns_BadRequest_when_title_missing()
        {
            var result = await _controller.CreateAsync("   ");
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task GetByIdAsync_returns_Ok_when_found()
        {
            var id = Guid.NewGuid();
            _sender.Setup(s => s.Send(new GetToDoListWithItemsQuery(id), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new ToDoListEntity { Id = id, Title = "Work" });

            var result = await _controller.GetByIdAsync(id);

            var ok = result as OkObjectResult;
            ok.Should().NotBeNull();
            (ok!.Value as ToDoListEntity)!.Id.Should().Be(id);
        }

        [Fact]
        public async Task GetByIdAsync_returns_NotFound_when_missing()
        {
            var id = Guid.NewGuid();
            _sender.Setup(s => s.Send(new GetToDoListWithItemsQuery(id), It.IsAny<CancellationToken>()))
                   .ReturnsAsync((ToDoListEntity?)null);

            var result = await _controller.GetByIdAsync(id);
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task AddItemAsync_returns_Ok_with_id()
        {
            _sender.Setup(s => s.Send(It.IsAny<AddItemToListCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(Guid.NewGuid());

            var result = await _controller.AddItemAsync(Guid.NewGuid(), new ToDoItem { Title = "Milk" });

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task AddItemAsync_returns_BadRequest_when_item_missing()
        {
            var result = await _controller.AddItemAsync(Guid.NewGuid(), null);
            result.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}
