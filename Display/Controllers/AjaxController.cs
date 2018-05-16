using MongoDB.Bson;
using Display.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Display.Controllers
{
    public class AjaxController : Controller
    {
        [Route("ajax")]
        public async Task<ActionResult> Index(string command, int id)
        {
            try
            {
                if (command == "get-display")
                {
                    if (id > 0)
                    {
                        var display = await GetDisplay(id);
                        var files = await GetFiles(id);
                        return Json(new { lastUpdate = display.LastUpdateUTC.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fffzzz"), images = files.OrderBy(x => x.FileName).Select(x => Url.Action("Image", "Home", new { id = 0 })) }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        throw new HttpException(400, "Invalid value for parameter: id");
                }
                else
                    throw new HttpException(400, "Invalid value for parameter: command");
            }
            catch (HttpException httpEx)
            {
                var statusCode = httpEx.GetHttpCode();
                Response.StatusCode = statusCode;
                return Json(new { error = httpEx.Message, statusCode = statusCode }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Json(new { error = ex.Message, statusCode = 500 }, JsonRequestBehavior.AllowGet);
            }
        }

        [Route("ajax/displays")]
        public async Task<ActionResult> GetDisplays()
        {
            var displays = await DisplayRepository.Current.GetDisplays();
            return Json(displays.Select(x => new
            {
                id = x.Id.ToString(),
                displayId = x.DisplayID,
                lastUpdateUtc = x.LastUpdateUTC.ToString("o"),
                files = x.Files.Select(f => new
                {
                    contentId = f.ContentId.ToString(),
                    filename = f.FileName,
                    width = f.Width,
                    height = f.Height,
                    active = f.Active
                })
            }), JsonRequestBehavior.AllowGet);
        }

        [Route("ajax/displays/{id}")]
        public async Task<ActionResult> GetDisplay(int id, bool? active = null)
        {
            var display = await DisplayRepository.Current.GetDisplay(id);
            return Json(new
            {
                id = display.Id.ToString(),
                displayId = display.DisplayID,
                lastUpdate = display.LastUpdateUTC.ToString("o"),
                images = display.Files.Where(x => active.GetValueOrDefault(x.Active) == x.Active).Select(f => new
                {
                    contentId = f.ContentId.ToString(),
                    filename = f.FileName,
                    width = f.Width,
                    height = f.Height,
                    active = f.Active,
                    url = Url.Action("Image", "Home", new { contentId = f.ContentId })
                })
            }, JsonRequestBehavior.AllowGet);
        }

        [Route("ajax/displays/{id}/files")]
        public async Task<ActionResult> GetDisplayFiles(int id)
        {
            var display = await DisplayRepository.Current.GetDisplay(id);
            return Json(display.Files.Select(f => new
            {
                contentId = f.ContentId.ToString(),
                filename = f.FileName,
                width = f.Width,
                height = f.Height,
                active = f.Active
            }), JsonRequestBehavior.AllowGet);
        }

        [Route("ajax/displays/{id}/files/active")]
        public async Task<ActionResult> GetDisplayActiveFiles(int id)
        {
            var display = await DisplayRepository.Current.GetDisplay(id);
            return Json(display.Files.Where(x => x.Active).Select(f => new
            {
                contentId = f.ContentId.ToString(),
                filename = f.FileName,
                width = f.Width,
                height = f.Height,
                active = f.Active
            }), JsonRequestBehavior.AllowGet);
        }

        [Route("ajax/files")]
        public async Task<ActionResult> GetAllFiles()
        {
            var files = await FileRepository.Current.GetFiles();
            var result = files.Select(x => new { id = x.Id.ToString(), filename = x.Filename });
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [Route("ajax/files/{id}/delete")]
        public async Task<ActionResult> DeleteFile(string id)
        {
            bool result = false;

            if (id == "all")
            {
                result = await FileRepository.Current.DeleteAllFiles();
            }
            else if (ObjectId.TryParse(id, out ObjectId objectId))
            {
                result = await FileRepository.Current.DeleteFile(objectId);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        private async Task<DisplayModel> GetDisplay(int displayId)
        {
            var result = await DisplayRepository.Current.GetDisplay(displayId);
            return result;
        }

        private async Task<IEnumerable<FileModel>> GetFiles(int displayId)
        {
            var display = await DisplayRepository.Current.GetDisplay(displayId);
            var result = display.Files;
            return result;
        }
    }
}