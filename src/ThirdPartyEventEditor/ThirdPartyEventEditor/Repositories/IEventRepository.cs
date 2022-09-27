using System.Collections.Generic;
using System.Threading.Tasks;
using ThirdPartyEventEditor.Models;

namespace ThirdPartyEventEditor.Repositories
{
    public interface IEventRepository
    {
        IEnumerable<ThirdPartyEvent> GetEvents();
        ThirdPartyEvent GetEventById(int id);

        bool AddEvent(ThirdPartyEvent thirdPartyEvent);
        bool UpdateEvent(ThirdPartyEventForUpdate thirdPartyEvent);
        bool DeleteEvent(ThirdPartyEvent thirdPartyEvent);
    }
}