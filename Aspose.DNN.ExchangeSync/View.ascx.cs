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
using DotNetNuke.Security.Profile;
using DotNetNuke.Security.Membership;
using DotNetNuke.Security.Roles;
using DotNetNuke.Entities.Users;
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data.Entity;
using System.Linq;

using System.Collections;
using System.Web.UI.WebControls;
using Aspose.Email.Exchange;
using Aspose.Email.Mail;
using System.IO;
using System.Net;
using System.Net.Security;
using Aspose.Email.Outlook.Pst;
using Aspose.Email.Outlook;
using Aspose.DNN.ExchangeSync.Components;
using DotNetNuke.Entities.Profile;

namespace Aspose.DNN.ExchangeSync
{
    public partial class View : ExchangeSyncModuleBase, IActionable
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (UserId > 0)
                {
                    LoggedInErrorDiv.Visible = false;
                    moduleMainDiv.Visible = true;
                }
                else
                {
                    LoggedInErrorDiv.Visible = true;
                    moduleMainDiv.Visible = false;
                }
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

        private bool ExchangeSettingsExist
        {
            get
            {
                Aspose_ExchangeSync_ServerDetails exchangeDetailsList = DatabaseHelper.CheckExchangeDetails(UserId);
                if (exchangeDetailsList != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void ResetControls()
        {
            ExchangeToDnnSync.Visible = false;
            DnnToExchangeSync.Visible = false;
            ExchangeSettings.Visible = false;
        }

        protected void ExchangeToDnnHyperLink_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (ExchangeSettingsExist)
            {
                ExchangeToDnnSync.ResetControls();
                ExchangeToDnnSync.Visible = true;
            }
            else
            {
                ExchangeSettings.Visible = true;
                ExchangeToDnnClickedHiddenField.Value = "true";
            }
        }

        protected void DnnToExchangeHyperLink_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (ExchangeSettingsExist)
            {
                DnnToExchangeSync.ResetControls();
                DnnToExchangeSync.Visible = true;
            }
            else
            {
                ExchangeSettings.Visible = true;
                DnnToExchangeClickedHiddenField.Value = "true";
            }
        }

        protected void ExchangeSettingsHyperLink_Click(object sender, EventArgs e)
        {
            Aspose_ExchangeSync_ServerDetails exchangeDetailsList = DatabaseHelper.CheckExchangeDetails(UserId);
            if (exchangeDetailsList != null)
            {
                ServerURLTextBox.Text = exchangeDetailsList.ServerURL;
                UsernameTextBox.Text = exchangeDetailsList.Username;
                PasswordTextBox.Text = exchangeDetailsList.Password;
                DomainTextBox.Text = exchangeDetailsList.Domain;
            }

            ResetControls();
            ExchangeSettings.Visible = true;
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            ExchangeCredsErrorDiv.Visible = false;

            Aspose_ExchangeSync_ServerDetails serverDetails = new Aspose_ExchangeSync_ServerDetails();

            serverDetails.ServerURL = ServerURLTextBox.Text.Trim();
            serverDetails.Username = UsernameTextBox.Text.Trim();
            serverDetails.Password = PasswordTextBox.Text.Trim();
            serverDetails.Domain = DomainTextBox.Text.Trim();
            serverDetails.UserID = UserId;

            try
            {
                NetworkCredential credentials = new NetworkCredential(serverDetails.Username, serverDetails.Password, serverDetails.Domain);
                IEWSClient client = EWSClient.GetEWSClient(serverDetails.ServerURL, credentials);
            }
            catch (Exception)
            {
                ExchangeCredsErrorDiv.Visible = true;
                return;
            }

            serverDetails.Password = Crypto.Encrypt(serverDetails.Password);

            DatabaseHelper.AddUpdateServerDetails(serverDetails);

            ResetControls();

            if (ExchangeToDnnClickedHiddenField.Value.Equals("true"))
            {
                ExchangeToDnnSync.Visible = true;
                ExchangeToDnnClickedHiddenField.Value = "false";
            }
            else if (DnnToExchangeClickedHiddenField.Value.Equals("true"))
            {
                DnnToExchangeSync.Visible = true;
                DnnToExchangeClickedHiddenField.Value = "false";
            }            
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            ResetControls();
        }

    }
}