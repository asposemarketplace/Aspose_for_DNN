/*
' Copyright (c) 2014  Christoc.com
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
using Aspose.Pdf;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using DotNetNuke.Common;

namespace Aspose.Modules.AsposeDNNPdfImport
{
    public partial class View : AsposeDNNPdfImportModuleBase, IActionable
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

                // Initialize the stream to read the uploaded file.
                Stream myStream = ImportFileUpload.FileContent;
                //open document
                Document pdfDocument = new Document(myStream);
                string path = Server.MapPath(".") + "//" + ImportFileUpload.FileName.Replace(".pdf", ".html");
                pdfDocument.Save(path, SaveFormat.Html);
                string extractedText = File.ReadAllText(path);

                if (destinationControl != null)
                    destinationControl.Controls.Add(new LiteralControl(extractedText));
                else
                    OutputLiteral.Text = extractedText;
            }
            else
            {
                OutputLiteral.Text = "Please Upload File";
            }
        }
    }
}