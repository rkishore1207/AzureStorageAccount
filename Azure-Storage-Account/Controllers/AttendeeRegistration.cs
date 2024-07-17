using Azure_Storage_Account.Data;
using Azure_Storage_Account.Services;
using Microsoft.AspNetCore.Mvc;

namespace Azure_Storage_Account.Controllers
{
    public class AttendeeRegistration : Controller
    {
        private readonly ITableStorageService _tableStorageService;
        private readonly IBlobStorageService _blobStorageService;

        public AttendeeRegistration(ITableStorageService tableStorageService, IBlobStorageService blobStorageService)
        {
            _tableStorageService = tableStorageService;
            _blobStorageService = blobStorageService;
        }

        // GET: AttendeeRegistration
        public async Task<ActionResult> Index()
        {
            var data = await _tableStorageService.GetAttendees();
            foreach (var attendee in data)
            {
                if(attendee.ImageName != null)
                    attendee.ImageName = await _blobStorageService.GetBlobUrl(attendee.ImageName);
            }
            return View(data);
        }

        // GET: AttendeeRegistration/Details/5
        public async Task<ActionResult> Details(string rowKey, string industry)
        {
            var data = await _tableStorageService.GetAttendee(industry, rowKey);
            return View(data);
        }

        // GET: AttendeeRegistration/Create
        public ActionResult Create()
        {
            //List<string> industries = new List<string>{ "IT" , "HR", "CORE","NON-CORE"};
            return View();
        }

        // POST: AttendeeRegistration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(AttendeeEntity attendee, IFormFile formFile)
        {
            try
            {
                string id = Guid.NewGuid().ToString();
                attendee.PartitionKey = attendee.Industry;
                attendee.RowKey = id;
                if (formFile.Length > 0)
                    attendee.ImageName = await _blobStorageService.UploadBlob(formFile, id, attendee.ImageName);
                else
                    attendee.ImageName = "default.jpg";
                await _tableStorageService.UpsertAttendee(attendee);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        //public async Task<ActionResult> Edit()
        //{
        //    var data = await _tableStorageService.GetAttendees();
        //    return View(data);
        //}

        // POST: AttendeeRegistration/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(AttendeeEntity attendee)
        {
            try
            {
                attendee.PartitionKey = attendee.Industry;
                await _tableStorageService.UpsertAttendee(attendee);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }



        // POST: AttendeeRegistration/Delete/5
        //[HttpDelete]
        //public async Task<ActionResult> Delete(string id, string industry)
        //{
        //    try
        //    {
        //        await  _tableStorageService.DeleteAttendee(id, industry);
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
