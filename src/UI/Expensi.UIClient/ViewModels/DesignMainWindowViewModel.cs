using System;
using System.Collections.Generic;
using System.Threading;
using Expensi.UIClient.Models;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Serialization;
using NSubstitute;

namespace Expensi.UIClient.ViewModels;

public class DesignMainWindowViewModel() : MainWindowViewModel(GetAdapter())
{
    private static readonly List<CategoryDto> FakeCategories =
    [
        new()
        {
            Id = Guid.NewGuid(),
            Name = "Category 1",
            Description = "Description 1"
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Category 2",
            Description = "Description 2"
        },

        new()
        {
            Id = Guid.NewGuid(),
            Name = "Category 3",
            Description = "Description 3"
        }
    ];

    private static readonly List<ExpenseDto> FakeExpenses =
    [
        new()
        {
            Id = Guid.NewGuid(),
            Title = "Expense 1",
            Amount = 100,
            Date = DateTime.Now,
            Description = "Expense 1 description",
            CategoryId = FakeCategories[0].Id,
            CategoryName = FakeCategories[0].Name,
            FamilyMemberName = "Member 1"
        },
        new()
        {
            Id = Guid.NewGuid(),
            Title = "Expense 2",
            Amount = 200,
            Date = DateTime.Now,
            Description = "Expense 2 description",
            CategoryId = FakeCategories[1].Id,
            CategoryName = FakeCategories[1].Name,
            FamilyMemberName = "Member 2"
        },
        new()
        {
            Id = Guid.NewGuid(),
            Title = "Expense 3",
            Amount = 300,
            Date = DateTime.Now,
            Description = "Expense 3 description",
            CategoryId = FakeCategories[2].Id,
            CategoryName = FakeCategories[2].Name,
            FamilyMemberName = "Member 1"
        }
    ];

    private static IRequestAdapter GetAdapter()
    {
        // https://learn.microsoft.com/en-us/openapi/kiota/testing
        var adapter = Substitute.For<IRequestAdapter>();
        adapter.SendCollectionAsync(
                Arg.Any<RequestInformation>(),
                Arg.Any<ParsableFactory<CategoryDto>>(),
                Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
                Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(FakeCategories);
        adapter.SendCollectionAsync(
                Arg.Any<RequestInformation>(),
                Arg.Any<ParsableFactory<ExpenseDto>>(),
                Arg.Any<Dictionary<string, ParsableFactory<IParsable>>>(),
                Arg.Any<CancellationToken>())
            .ReturnsForAnyArgs(FakeExpenses);
        return adapter;
    }
}
