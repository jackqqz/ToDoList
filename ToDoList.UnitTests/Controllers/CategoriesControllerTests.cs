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
    public class CategoriesControllerTests
    {
        private readonly Mock<ISender> _sender = new();
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _controller = new CategoriesController(_sender.Object);
        }

        [Fact]
        public async Task GetAll_returns_Ok()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetAllCategoriesQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new List<CategoryEntity>());

            var result = await _controller.GetAll();
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetById_returns_NotFound_when_missing()
        {
            _sender.Setup(s => s.Send(It.IsAny<GetCategoryByIdQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync((CategoryEntity?)null);

            var result = await _controller.GetById(Guid.NewGuid());
            result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Create_returns_CreatedAtAction()
        {
            var created = new CategoryEntity { Id = Guid.NewGuid(), Name = "Work" };
            _sender.Setup(s => s.Send(It.IsAny<CreateCategoryCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(created);

            var result = await _controller.Create("Work");
            result.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task Update_returns_Ok()
        {
            var updated = new CategoryEntity { Id = Guid.NewGuid(), Name = "Home" };
            _sender.Setup(s => s.Send(It.IsAny<UpdateCategoryCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(updated);

            var result = await _controller.Update(updated.Id, "Home");
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Delete_returns_NoContent()
        {
            _sender.Setup(s => s.Send(It.IsAny<DeleteCategoryCommand>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(true);

            var result = await _controller.Delete(Guid.NewGuid());
            result.Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task Suggest_returns_Ok()
        {
            _sender.Setup(s => s.Send(It.IsAny<SuggestCategoriesQuery>(), It.IsAny<CancellationToken>()))
                   .ReturnsAsync(new List<CategoryEntity>());

            var result = await _controller.Suggest("Wo", 5);
            result.Should().BeOfType<OkObjectResult>();
        }
    }
}
