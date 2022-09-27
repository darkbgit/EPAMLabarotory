using System.Collections.Generic;
using System.Linq;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Repositories.Base;

namespace ThirdPartyEventEditor.Repositories
{
    internal class EventRepository : Repository<ThirdPartyEvent>, IEventRepository
    {
        public EventRepository(string path)
            : base(path)
        {

        }

        public IEnumerable<ThirdPartyEvent> GetEvents()
        {
            return GetAll()
                .OrderBy(e => e.Name)
                .ToList();
        }

        public ThirdPartyEvent GetEventById(int id)
        {
            return GetById(id);
        }

        public bool AddEvent(ThirdPartyEvent thirdPartyEvent)
        {
            return Insert(thirdPartyEvent);
        }

        public bool UpdateEvent(ThirdPartyEventForUpdate thirdPartyEvent)
        {
            var originalEvent = new ThirdPartyEvent
            {
                Id = thirdPartyEvent.Id,
                Name = thirdPartyEvent.OriginalName,
                Description = thirdPartyEvent.OriginalDescription,
                LayoutId = thirdPartyEvent.OriginalLayoutId,
                StartDate = thirdPartyEvent.OriginalStartDate,
                EndDate = thirdPartyEvent.OriginalEndDate,
                PosterImage = thirdPartyEvent.OriginalPosterImage,
            };

            var newEvent = new ThirdPartyEvent()
            {
                Id = thirdPartyEvent.Id,
                Name = thirdPartyEvent.Name,
                Description = thirdPartyEvent.Description,
                LayoutId = thirdPartyEvent.LayoutId,
                StartDate = thirdPartyEvent.StartDate,
                EndDate = thirdPartyEvent.EndDate,
                PosterImage = thirdPartyEvent.PosterImage,
            };

            return Update(originalEvent, newEvent);
        }

        public bool DeleteEvent(ThirdPartyEvent thirdPartyEvent)
        {
            return Delete(thirdPartyEvent);
        }

        protected override bool IsExist(ThirdPartyEvent item, IEnumerable<ThirdPartyEvent> items)
        {
            var dbItem = items.FirstOrDefault(i => i.Name == item.Name && i.LayoutId == item.LayoutId &&
                                                i.Description == item.Description && i.StartDate == item.StartDate &&
                                                i.EndDate == item.EndDate && i.PosterImage == item.PosterImage);

            return dbItem != null;
        }

        protected override bool IsExistWithId(ThirdPartyEvent item, IEnumerable<ThirdPartyEvent> items)
        {
            var dbItem = items.FirstOrDefault(i =>i.Id == item.Id && i.Name == item.Name && i.LayoutId == item.LayoutId &&
                                                i.Description == item.Description && i.StartDate == item.StartDate &&
                                                i.EndDate == item.EndDate && i.PosterImage == item.PosterImage);

            return dbItem != null;
        }
    }
}