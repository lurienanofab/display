using Display.Models;
using MongoDB.Bson;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Display.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        public ActionResult Index(int id = 0)
        {
            ViewBag.ID = id;
            return View();
        }

        [HttpGet, Route("admin")]
        public async Task<ActionResult> Admin(AdminModel model)
        {
            model.Files = await FileRepository.Current.GetFiles(model.ID);
            return View(model);
        }

        [HttpGet, Route("image/{id}")]
        public async Task<ActionResult> Image(string id)
        {
            var objectId = ObjectId.Parse(id);
            var file = await FileRepository.Current.GetFile(objectId);
            var mime = MimeMapping.GetMimeMapping(file.FileName);
            var data = file.Data;
            return File(data, mime);
        }

        [HttpPost, Route("image/{displayId}")]
        public async Task<ActionResult> ImageUpload(int displayId)
        {
            int count = 0;

            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var file = Request.Files[i];
                    if (file.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(file.FileName);
                        Stream source = file.InputStream;
                        await FileRepository.Current.SaveFile(displayId, fileName, source);
                        count++;
                    }
                }
            }

            if (count > 0)
                await DisplayRepository.Current.Update(displayId);

            return RedirectToAction("Admin", new { id = displayId });
        }

        [HttpGet, Route("image/{id}/delete")]
        public async Task<ActionResult> ImageDelete(string id)
        {
            var objectId = ObjectId.Parse(id);
            var file = await FileRepository.Current.GetFile(objectId);
            var deleted = await file.Delete();

            if (deleted)
                await DisplayRepository.Current.Update(file.DisplayID);

            return RedirectToAction("Admin", new { id = file.DisplayID });
        }
    }
}