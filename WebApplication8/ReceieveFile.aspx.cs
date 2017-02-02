using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication8
{
    /// <summary>
    /// 回傳物件
    /// </summary>
    public class customResponse
    {
        /// <summary>
        /// 我們只回傳一個URL
        /// </summary>
        public string URL { get; set; }
    }
    public partial class ReceieveFile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //配合前端需要接收pure JSON response
            Response.Clear();
            Response.ContentType = "application/json";
            //理論上，只會有1
            if (Request.Files.Count > 0)
            {
                try
                {
                    //我們把上傳的檔案放到temp資料夾底下
                    string path = Server.MapPath("temp\\");
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        //為了上傳檔案不會重複，因此檔名一率加上guid
                        var guid = Guid.NewGuid();
                        var file = Request.Files[i];
                        path += guid + "-" + file.FileName;
                        //存檔
                        file.SaveAs(path);
                        //done
                        var JSON = Newtonsoft.Json.JsonConvert.SerializeObject(
                            new customResponse { URL = "temp\\" + guid + "-" + file.FileName });
                        //回應JSON
                        Response.Write(JSON);
                    }
                }
                catch (Exception ex)
                {
                    //如果有錯誤，回應exception JSON
                    string jsonError = Newtonsoft.Json.JsonConvert.SerializeObject(ex);
                    Response.Write(jsonError);
                }
                //stop response
                Response.End();
            }
            else
            {
                //如果別人亂入
                string jsonError = Newtonsoft.Json.JsonConvert.SerializeObject(new Exception("No Any Files."));
                Response.Write(jsonError);
                Response.End();
            }
        }
    }
}