namespace Riktig.Web.Controllers
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Contracts.Api;
    using Magnum.Extensions;
    using MassTransit;
    using Models;
    using RapidTransit.Core;


    public class ImageController :
        Controller
    {
        readonly IServiceBus _bus;
        readonly ImageServiceSettings _settings;

        public ImageController(IHostServiceBus bus, ImageServiceSettings settings)
        {
            _bus = bus;
            _settings = settings;
        }

        //
        // GET: /Image/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Image/Create

        public ActionResult RequestImage()
        {
            return View();
        }

        //
        // POST: /Image/Create

        [HttpPost]
        public ActionResult RequestImage(ImageViewModel model)
        {
            try
            {
                _bus.GetEndpoint(_settings.ImageTrackingServiceAddress)
                    .Send(new RequestImageCommand(new Uri(model.SourceAddress)));

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult GetImages()
        {
            return View();
        }

        //
        // POST: /Image/Create

        [HttpPost]
        public async Task<ActionResult> GetImages(GetImagesViewModel model)
        {
            try
            {
                IEndpoint endpoint = _bus.GetEndpoint(_settings.ImageTrackingServiceAddress);

                var results = new ConcurrentBag<Uri>();

                IEnumerable<Task> requests = model.SourceAddress
                                                  .Where(
                                                      x =>
                                                      !string.IsNullOrEmpty(x)
                                                      && Uri.IsWellFormedUriString(x, UriKind.RelativeOrAbsolute))
                                                  .Select(address =>
                                                      {
                                                          return
                                                              endpoint.SendRequestAsync(_bus,
                                                                  new RequestImageCommand(new Uri(address)), x =>
                                                                      {
                                                                          x.Handle<ImageRequestCompleted>(
                                                                              msg => { results.Add(msg.LocalAddress); });
                                                                          x.Handle<ImageRequestFaulted>(msg => { });
                                                                          x.HandleTimeout(30.Seconds(), () => { });
                                                                      }).Task;
                                                      });

                return await Task.WhenAll(requests)
                                 .ContinueWith(tasks => { return Json(results); });
            }
            catch
            {
                return View();
            }
        }


        class RequestImageCommand :
            RequestImage
        {
            public RequestImageCommand(Uri sourceAddress)
            {
                RequestId = NewId.NextGuid();
                Timestamp = DateTime.UtcNow;

                SourceAddress = sourceAddress;
            }

            public Guid RequestId { get; private set; }
            public DateTime Timestamp { get; private set; }
            public Uri SourceAddress { get; private set; }
        }
    }
}