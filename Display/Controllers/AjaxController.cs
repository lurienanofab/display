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
                        return Json(new { lastUpdate = display.LastUpdateUTC.ToLocalTime().ToString("yyyy-MM-dd HH:mm:ss.fffzzz"), images = files.Select(x => Url.Action("Image", "Home", new { id = x.ID })) }, JsonRequestBehavior.AllowGet);
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

        private async Task<DisplayModel> GetDisplay(int displayId)
        {
            var result = await DisplayRepository.Current.GetDisplay(displayId);
            return result;
        }

        private async Task<IEnumerable<FileModel>> GetFiles(int displayId)
        {
            var result = await FileRepository.Current.GetFiles(displayId);
            return result;
        }
    }
}