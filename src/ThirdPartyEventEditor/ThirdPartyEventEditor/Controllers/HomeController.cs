using System;
using System.Configuration;
using System.IO;
using System.Web.Mvc;
using ThirdPartyEventEditor.Filters;
using ThirdPartyEventEditor.Models;
using ThirdPartyEventEditor.Repositories;

namespace ThirdPartyEventEditor.Controllers
{
    [ActionTimeMeasurementFilter]
    public class HomeController : Controller
    {
        private readonly IEventRepository _eventRepository;

        public HomeController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        [HttpGet]
        public ActionResult Index()
        {
            var events = _eventRepository.GetEvents();
            return View(events);
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var model = _eventRepository.GetEventById(id.Value);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ThirdPartyEvent model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_eventRepository.AddEvent(model))
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Can't create event. Try again.");

            return View(model);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }

            var @event = _eventRepository.GetEventById(id.Value);

            if (@event == null)
            {
                return HttpNotFound();
            }

            var model = new ThirdPartyEventForUpdate()
            {
                Id = @event.Id,
                Description = @event.Description,
                OriginalDescription = @event.Description,
                Name = @event.Name,
                OriginalName = @event.Name,
                LayoutId = @event.LayoutId,
                OriginalLayoutId = @event.LayoutId,
                StartDate = @event.StartDate,
                OriginalStartDate = @event.StartDate,
                EndDate = @event.EndDate,
                OriginalEndDate = @event.EndDate,
                PosterImage = @event.PosterImage,
                OriginalPosterImage = @event.PosterImage,
            };


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ThirdPartyEventForUpdate model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (_eventRepository.UpdateEvent(model))
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Unable to save changes. Try again.");

            return View(model);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var model = _eventRepository.GetEventById(id);

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(ThirdPartyEvent model)
        {
            if (_eventRepository.DeleteEvent(model))
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, "Can't delete event. Try again.");

            return View();

        }

        [HttpPost]
        public ActionResult GetDb()
        {
            var path = Path.Combine(Server.MapPath("~/App_Data/"), ConfigurationManager.AppSettings["DbJsonFile"]);

            var bytes = System.IO.File.ReadAllBytes(path);


            return File(bytes, "text/json");
        }

        [HttpGet]
        public void TryError()
        {
            throw new Exception("This exception shows error filter work.");
        }
    }
}