using Microsoft.Data.SqlClient;

namespace TicketManagement.DataAccess.ADO.Data
{
    internal interface IConnectionFactory
    {
        SqlConnection Create();
    }
}