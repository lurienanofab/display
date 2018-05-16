using Display.Models;
using MongoDB.Bson;
using System.IO;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Linq;

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

        [HttpGet, Route("admin/{displayId?}")]
        public async Task<ActionResult> Admin(int displayId = 0)
        {
            var model = await DisplayRepository.Current.GetDisplay(displayId);
            return View(model);
        }

        [HttpPost, Route("admin")]
        public ActionResult AdminRedirect(int displayId = 0)
        {
            if (displayId == 0)
                return RedirectToAction("Admin");
            else
                return RedirectToAction("Admin", new { displayId });
        }

        [HttpPost, Route("admin/{displayId}/image/upload")]
        public async Task<ActionResult> AdminImageUpload(int displayId)
        {
            int count = 0;

            var display = await DisplayRepository.Current.GetDisplay(displayId);

            if (Request.Files.Count > 0)
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    var postedFile = Request.Files[i];
                    if (postedFile.ContentLength > 0)
                    {
                        string fileName = Path.GetFileName(postedFile.FileName);
                        Stream source = postedFile.InputStream;
                        var file = await FileModel.Create(fileName, source);
                        await DisplayRepository.Current.AddFile(displayId, file);
                        count++;
                    }
                }
            }

            return RedirectToAction("Admin", new { displayId });
        }

        [HttpGet, Route("admin/{displayId}/image/{contentId}/delete")]
        public async Task<ActionResult> AdminImageDelete(int displayId, string contentId)
        {
            var display = await DisplayRepository.Current.GetDisplay(displayId);
            var objectId = ObjectId.Parse(contentId);

            await FileRepository.Current.DeleteFile(objectId);
            await DisplayRepository.Current.RemoveFile(displayId, objectId);
            
            return RedirectToAction("Admin", new { displayId });
        }

        [HttpGet, Route("admin/{displayId}/image/{contentId}/toggle-active")]
        public async Task<ActionResult> AdminImageToggleActive(int displayId, string contentId)
        {
            var display = await DisplayRepository.Current.GetDisplay(displayId);
            var objectId = ObjectId.Parse(contentId);
            var file = display.Files.First(x => x.ContentId == objectId);
            await DisplayRepository.Current.SetFileActive(displayId, objectId, !file.Active);

            return RedirectToAction("Admin", new { displayId });
        }

        [HttpGet, Route("image/{contentId}")]
        public async Task<ActionResult> Image(string contentId)
        {
            var objectId = ObjectId.Parse(contentId);
            var file = await FileRepository.Current.GetFileInfo(objectId);
            var mime = MimeMapping.GetMimeMapping(file.Filename);
            var data = await FileRepository.Current.DownloadFile(objectId);
            return File(data, mime);
        }
    }
}