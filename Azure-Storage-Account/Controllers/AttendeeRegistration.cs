using Azure_Storage_Account.Data;
using Azure_Storage_Account.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Azure_Storage_Account.Controllers
{
    public class AttendeeRegistration : Controller
    {
        private readonly ITableStorageService _tableStorageService;

        public AttendeeRegistration(ITableStorageService tableStorageService)
        {
            _tableStorageService = tableStorageService;
        }

        // GET: AttendeeRegistration
        public async Task<ActionResult> Index()
        {
            var data = await _tableStorageService.GetAttendees();
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
        public async Task<ActionResult> Create(AttendeeEntity attendee)
        {
            try
            {
                attendee.PartitionKey = attendee.Industry;
                attendee.RowKey = Guid.NewGuid().ToString();
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
