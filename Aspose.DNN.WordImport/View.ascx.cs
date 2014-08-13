// Copyright (c) Aspose 2002-2014. All Rights Reserved.

using System;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Localization;
using System.IO;
using Aspose.Words;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;


namespace Modules.Aspose.DNN.WordImport
{
    public partial class View : WordImportModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!Page.IsPostBack)
                    LoadPanes();
            }
            catch (Exception exc) //Module failed to load
            {
                Exceptions.ProcessModuleLoadException(this, exc);
            }
        }

        private void LoadPanes()
        {
            foreach (string pane in PortalSettings.ActiveTab.Panes)
            {
                Control obj = (Control)DotNetNuke.Common.Globals.FindControlRecursiveDown(Page, pane);

                PanesDropDownList.Items.Add(new ListItem(pane, obj.ClientID));
            }
        }

        protected void ImportButton_Click(object sender, EventArgs e)
        {
            Control destinationControl = Globals.FindControlRecursiveDown(Page, PanesDropDownList.SelectedValue.Replace("dnn_", string.Empty));
            
            if (ImportFileUpload.HasFile)
            {
                // Check for license and apply if exists
                string licenseFile = Server.MapPath("~/App_Data/Aspose.Total.lic");
                if (File.Exists(licenseFile))
                {
                    License license = new License();
                    license.SetLicense(licenseFile);
                }

                Stream stream = ImportFileUpload.FileContent;
                Document doc = new Document(stream);

                string filePath = Server.MapPath("~/temp/");
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);

                filePath += "\\" + System.Guid.NewGuid().ToString();

                doc.Save(filePath, SaveFormat.Html);
                string outputText = File.ReadAllText(filePath);

                if (destinationControl != null)
                    destinationControl.Controls.Add(new LiteralControl(outputText));
                else
                    OutputLiteral.Text = outputText;
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
    }
}