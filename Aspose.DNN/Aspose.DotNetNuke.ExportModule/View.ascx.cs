using System;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Text;
using Aspose.Words;
using System.Collections;

namespace Aspose.Modules.AsposeDotNetNukeContentExport
{
    public enum ExportType
    {
        None = 0,
        Pdf = 1,
        Word = 2
    }


    public partial class View : AsposeDotNetNukeContentExportModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                {
                    SetLocalizationText();
                    LoadPanes();
                }
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void SetLocalizationText()
        {
            WordsExportButton.CssClass = Settings["WordsExportButtonCssClass"] != null ? Settings["WordsExportButtonCssClass"].ToString() : string.Empty;
            PdfExportButton.CssClass = Settings["PdfExportButtonCssClass"] != null ? Settings["PdfExportButtonCssClass"].ToString() : string.Empty;

            if (Settings["PaneSelectionDropDownCssClass"] != null)
            {
                if (!string.IsNullOrEmpty(Settings["PaneSelectionDropDownCssClass"].ToString()))
                    PanesDropDownList.CssClass = Settings["PaneSelectionDropDownCssClass"].ToString();
            }
        }

        private void LoadPanes()
        {
            PanesDropDownList.Items.Add(new ListItem(LocalizeString("FullPage"), "dnn_full_page"));

            foreach (string pane in PortalSettings.ActiveTab.Panes)
            {
                Control obj = (Control)DotNetNuke.Common.Globals.FindControlRecursiveDown(Page, pane);

                PanesDropDownList.Items.Add(new ListItem(pane, obj.ClientID));
            }

            if (Settings["DefaultPane"] != null)
            {
                PanesDropDownList.SelectedValue = Settings["DefaultPane"].ToString();
            }

            Session["PanesDropDown_" + TabId.ToString()] = PanesDropDownList.Items;

            PanesDropDownList.Attributes.Remove("style");

            if (Settings["HideDefaultPane"] != null)
            {
                if (Convert.ToBoolean(Settings["HideDefaultPane"].ToString()))
                    PanesDropDownList.Attributes.Add("style", "display: none;");
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
            string name = System.Guid.NewGuid().ToString() + extension;
            return name;
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

        private void ExportContent(ExportType exportType)
        {
            string pageSource = PageSourceHiddenField.Value;
            pageSource = "<html>" + pageSource.Replace("#g#", ">").Replace("#l#", "<") + "</html>";

            pageSource = pageSource.Replace("<div class=" + "\"exportButton\"" + ">", "<div class=" + "\"exportButton\"" + "style=" + "\"display: none\"" + ">");

            // To make the relative image paths work, base URL must be included in head section
            pageSource = pageSource.Replace("</head>", string.Format("<base href='{0}'></base></head>", BaseURL));

            // Check for license and apply if exists
            string licenseFile = Server.MapPath("~/App_Data/Aspose.Words.lic");
            if (File.Exists(licenseFile))
            {
                License license = new License();
                license.SetLicense(licenseFile);
            }

            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(pageSource));
            Document doc = new Document(stream);
            string fileName = GetOutputFileName(exportType == ExportType.Word ? ".doc" : ".pdf");

            doc.Save(GetPortalRootSavePath() + "\\" + fileName);
            doc.Save(Response, fileName, ContentDisposition.Inline, null);
            Response.End();
        }

        private string GetPortalRootSavePath()
        {
            string rootPath = Server.MapPath(PortalSettings.HomeDirectory) + "\\" + "AsposeExport";
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);
            return rootPath;
        }

        protected void WordsExportButton_Click(object sender, EventArgs e)
        {
            ExportContent(ExportType.Word);
        }

        protected void PdfExportButton_Click(object sender, EventArgs e)
        {
            ExportContent(ExportType.Pdf);
        }

    }
}