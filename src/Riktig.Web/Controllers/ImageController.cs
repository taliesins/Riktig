namespace Riktig.Web.Controllers
{
    using System;
    using System.Web.Mvc;
    using Contracts.Api;
    using MassTransit;
    using Models;


    public class ImageController : 
        Controller
    {
        //
        // GET: /Image/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Image/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Image/Create

        public ActionResult Request()
        {
            return View();
        }

        //
        // POST: /Image/Create

        [HttpPost]
        public ActionResult Request(ImageViewModel model)
        {
            try
            {
                Bus.Instance.GetEndpoint(ServiceBusConfig.ImageServiceAddress)
                   .Send(new RequestImageCommand(new Uri(model.SourceAddress)));

                return RedirectToAction("Index");
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