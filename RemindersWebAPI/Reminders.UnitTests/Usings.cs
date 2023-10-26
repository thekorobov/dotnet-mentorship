global using Xunit;
global using AutoMapper;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.EntityFrameworkCore;
global using Moq;
global using Reminders.BLL.DTO.Exceptions;
global using Reminders.DAL.Entities.Filtres;
global using Reminders.DAL.Interfaces;
global using User = Reminders.DAL.Entities.User;
global using Reminders.BLL.Utils.Constants;
global using Reminders.BLL.DTO;
global using FluentValidation.TestHelper;
global using Reminders.DAL.Entities;
global using FluentValidation;
global using FluentAssertions;
global using Reminders.BLL.Interfaces;
global using Reminders.BLL.CQS;
global using Reminders.UnitTests.Common;
global using Reminders.DAL.Data;
global using Reminders.DAL.Repositories;
global using FluentValidation.Results;
global using Reminders.BLL.CQS.ValidationDecorators;
global using Reminders.BLL.Services;
global using MockQueryable.Moq;