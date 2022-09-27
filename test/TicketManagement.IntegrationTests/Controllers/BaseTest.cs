using System;
using Microsoft.EntityFrameworkCore;
using TicketManagement.DataAccess.EF.Core;
using TicketManagement.DataAccess.EF.Implementation;
using TicketManagement.DataAccess.Public;
using TicketManagement.IntegrationTests.Helpers;

namespace TicketManagement.IntegrationTests.Controllers;

internal class BaseTest
{
    protected IUnitOfWork GetUnitOfWork()
    {
        var context = GetContextWithData();

        IUnitOfWork unitOfWork = new UnitOfWork(context);

        return unitOfWork;
    }

    private TicketManagementContext GetContextWithData()
    {
        var options = new DbContextOptionsBuilder<TicketManagementContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var context = new TicketManagementContext(options);

        context.Venue.AddRange(TestData.Venues);
        context.Layout.AddRange(TestData.Layouts);
        context.Area.AddRange(TestData.Areas);
        context.Seat.AddRange(TestData.Seats);
        context.Event.AddRange(TestData.Events);
        context.EventArea.AddRange(TestData.EventAreas);
        context.EventSeat.AddRange(TestData.EventSeats);

        context.SaveChanges();

        return context;
    }
}