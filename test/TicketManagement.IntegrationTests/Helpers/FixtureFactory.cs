using AutoFixture;
using AutoFixture.AutoMoq;

namespace TicketManagement.IntegrationTests.Helpers;

internal static class FixtureFactory
{
    public static IFixture GetFixture()
    {
        return new Fixture().Customize(new AutoMoqCustomization());
    }
}