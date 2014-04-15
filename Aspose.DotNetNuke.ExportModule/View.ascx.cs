/*
' Copyright (c) 2014  Aspose.com
'  All rights reserved.
' 
' THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED
' TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
' THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF
' CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
' DEALINGS IN THE SOFTWARE.
' 
*/

using System;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;

using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Text;
using Aspose.Words;

namespace Aspose.Modules.AsposeDotNetNukeContentExport
{
    /// -----------------------------------------------------------------------------
    /// <summary>
    /// The View class displays the content
    /// 
    /// Typically your view control would be used to display content or functionality in your module.
    /// 
    /// View may be the only control you have in your project depending on the complexity of your module
    /// 
    /// Because the control inherits from AsposeDotNetNukeContentExportModuleBase you have access to any custom properties
    /// defined there, as well as properties from DNN such as PortalId, ModuleId, TabId, UserId and many more.
    /// 
    /// </summary>
    /// -----------------------------------------------------------------------------
    public partial class View : AsposeDotNetNukeContentExportModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        public ModuleActionCollection ModuleActions
        {
            get
            {
                var actions = new ModuleActionCollection
                    {
                        {
                            GetNextActionID(), Localization.GetString("EditModule", LocalResourceFile), "", "", "",
                            EditUrl(), false, SecurityAccessLevel.Edit, true, false
                        }
                    };
                return actions;
            }
        }

        private string GetOutputFileName(string extension)
        {
            string name = HttpContext.Current.Request.RawUrl.Substring(HttpContext.Current.Request.RawUrl.LastIndexOf("/"));
            name = name.Replace("/", string.Empty).Replace(".aspx", extension);

            if (string.IsNullOrEmpty(name))
            {
                name = System.Guid.NewGuid().ToString();
            }

            if (!name.EndsWith(extension)) name = name + extension;

            return name;
        }

        private string CurrentPageURL
        {
            get
            {
                string url = Request.Url.Authority + HttpContext.Current.Request.RawUrl.ToString();

                if (Request.ServerVariables["HTTPS"] == "on")
                {
                    url = "https://" + url;
                }
                else
                {
                    url = "http://" + url;
                }

                return url;
            }
        }

        private string BaseURL
        {
            get
            {
                string url = Request.Url.Authority;

                if (Request.ServerVariables["HTTPS"] == "on")
                {
                    url = "https://" + url;
                }
                else
                {
                    url = "http://" + url;
                }

                return url;
            }
        }

        protected void WordsExportButton_Click(object sender, EventArgs e)
        {
            string html = new WebClient().DownloadString(CurrentPageURL);

            // To make the relative image paths work, base URL must be included in head section
            html = html.Replace("</head>", string.Format("<base href='{0}'></base></head>", BaseURL));

            // Check for license and apply if exists
            string licenseFile = Server.MapPath("~/App_Data/Aspose.Words.lic");
            if (File.Exists(licenseFile))
            {
                License license = new License();
                license.SetLicense(licenseFile);
            }

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
            Document doc = new Document(stream);
            doc.Save(Response, GetOutputFileName(".doc"), ContentDisposition.Inline, null);
            Response.End();
        }

        protected void PdfExportButton_Click(object sender, EventArgs e)
        {
            string html = new WebClient().DownloadString(CurrentPageURL);

            // To make the relative image paths work, base URL must be included in head section
            html = html.Replace("</head>", string.Format("<base href='{0}'></base></head>", BaseURL));

            // Check for license and apply if exists
            string licenseFile = Server.MapPath("~/App_Data/Aspose.Words.lic");
            if (File.Exists(licenseFile))
            {
                License license = new License();
                license.SetLicense(licenseFile);
            }

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(html));
            Document doc = new Document(stream);
            doc.Save(Response, GetOutputFileName(".pdf"), ContentDisposition.Inline, null);
            Response.End();
        }
    }
}