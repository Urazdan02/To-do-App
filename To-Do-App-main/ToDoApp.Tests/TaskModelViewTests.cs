using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using ToDoApp.Controllers;
using ToDoApp.Data;
using ToDoApp.Models;

namespace ToDoApp.Tests
{
    public class Tests
    {
        private TaskModelsController taskController;

        [SetUp]
        public void Setup()
        {
            taskController = new TaskModelsController(null);
        }

        [Test]
        public void ShouldReturnNotFoundViewTrue()
        {
            var result = taskController.Details(null).Result as ViewResult;
            Assert.AreEqual("NotFound", result.ViewName);
        }

        [Test]
        public void ShouldReturnCreateViewModelTrue()
        {
            var result = taskController.Create() as ViewResult;
            Assert.NotNull(result.Model);
        }

        [Test]
        public void ShouldReturnTaskObjectTrue()
        {
            TaskModel task = new TaskModel();
            var result = taskController.Create() as ViewResult;
            Assert.AreEqual(typeof(TaskModel), result.Model.GetType());
        }

    }
}