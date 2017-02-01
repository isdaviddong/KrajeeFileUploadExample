using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication8
{
    public class customResponse
    {
        public string URL { get; set; }
    }
    public partial class ReceieveFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "application/json";

            if (Request.Files.Count > 0)
            {
                try
                {
                    string path = Server.MapPath("temp\\");
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var guid = Guid.NewGuid();
                        var file = Request.Files[i];
                        path += guid + "-" + file.FileName;
                        file.SaveAs(path);
                        //done
                        var JSON = Newtonsoft.Json.JsonConvert.SerializeObject(
                            new customResponse { URL = "temp\\" + guid + "-" + file.FileName });
                        Response.Write(JSON);
                    }
                }
                catch (Exception ex)
                {
                    string jsonError = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
                    Response.Write(jsonError);
                }
                Response.End();
            }
            else
            {
                string jsonError = Newtonsoft.Json.JsonConvert.SerializeObject(new Exception("No Any Files."));
                Response.Write(jsonError);
                Response.End();
            }
        }
    }
}