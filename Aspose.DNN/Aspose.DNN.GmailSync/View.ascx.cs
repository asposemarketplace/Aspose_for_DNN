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
using Aspose.Email.Google;
using Aspose.Email.Mail;
using System.IO;
using System.Net;
using System.Net.Security;
using Aspose.Email.Services.Google;
using Aspose.DNN.GmailSync.Components;
using Aspose.DNN.GmailSync.Data;
using DotNetNuke.Entities.Profile;

namespace Aspose.DNN.GmailSync
{
    public partial class View : GmailSyncModuleBase, IActionable
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

        private bool GmailSettingsExist
        {
            get
            {
                Aspose_GmailSync_ServerDetails gmailDetailsList = DatabaseHelper.CheckGmailDetails(UserId);
                if (gmailDetailsList != null)
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
            GmailToDnnSync.Visible = false;
            DnnToGmailSync.Visible = false;
            GmailSettings.Visible = false;
        }

        protected void GmailToDnnHyperLink_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (GmailSettingsExist)
            {
                GmailToDnnSync.ResetControls();
                GmailToDnnSync.Visible = true;
            }
            else
            {
                GmailSettings.Visible = true;
                GmailToDnnClickedHiddenField.Value = "true";
            }
        }

        protected void DnnToGmailHyperLink_Click(object sender, EventArgs e)
        {
            ResetControls();

            if (GmailSettingsExist)
            {
                DnnToGmailSync.ResetControls();
                DnnToGmailSync.Visible = true;
            }
            else
            {
                GmailSettings.Visible = true;
                DnnToGmailClickedHiddenField.Value = "true";
            }
        }

        protected void GmailSettingsHyperLink_Click(object sender, EventArgs e)
        {
            Aspose_GmailSync_ServerDetails gmailDetailsList = DatabaseHelper.CheckGmailDetails(UserId);
            if (gmailDetailsList != null)
            {
                EmailAddressTextBox.Text = gmailDetailsList.Email;
                ClientIDTextBox.Text = gmailDetailsList.ClientID;
                PasswordTextBox.Text = gmailDetailsList.Password;
                ClientSecretTextBox.Text = gmailDetailsList.ClientSecret.ToString();
            }

            ResetControls();
            GmailSettings.Visible = true;
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            GmailCredsErrorDiv.Visible = false;

            Aspose_GmailSync_ServerDetails serverDetails = new Aspose_GmailSync_ServerDetails();

            serverDetails.Email = EmailAddressTextBox.Text.Trim();

            if (serverDetails.Email.Contains("@"))
            {
                serverDetails.Username = serverDetails.Email.Split('@')[0];
            }
            
            serverDetails.Password = PasswordTextBox.Text.Trim();
            serverDetails.ClientID = ClientIDTextBox.Text.Trim();
            serverDetails.ClientSecret = ClientSecretTextBox.Text.Trim();
            serverDetails.DNNUserID = UserId;

            try
            {
                string refresh_token = string.Empty;

                //Code segment - START
                //This segment of code is used to get the refresh_token. In general, you do not have to refresh refresh_token every time, you need to do it once, and then use it to retrieve access-token.
                //Thus, use it once to retrieve the refresh_token and then use the refresh_token value each time.
                string access_token; string token_type; int expires_in;
                GoogleTestUser user = new GoogleTestUser(serverDetails.Username, serverDetails.Email, serverDetails.Password, serverDetails.ClientID, serverDetails.ClientSecret);
                GoogleOAuthHelper.GetAccessToken(user, out access_token, out refresh_token, out token_type, out expires_in);
                serverDetails.RefreshToken = refresh_token;
                //Code segment - END

                using (IGmailClient client = Aspose.Email.Google.GmailClient.GetInstance(serverDetails.ClientID, serverDetails.ClientSecret, serverDetails.RefreshToken))
                {
                    FeedEntryCollection groups = client.FetchAllGroups();
                }
                
            }
            catch (Exception)
            {
                GmailCredsErrorDiv.Visible = true;
                return;
            }

            serverDetails.Password = Crypto.Encrypt(serverDetails.Password);
            serverDetails.ClientID = Crypto.Encrypt(serverDetails.ClientID);
            serverDetails.ClientSecret = Crypto.Encrypt(serverDetails.ClientSecret);
            serverDetails.RefreshToken = Crypto.Encrypt(serverDetails.RefreshToken);

            DatabaseHelper.AddUpdateServerDetails(serverDetails);

            ResetControls();

            if (GmailToDnnClickedHiddenField.Value.Equals("true"))
            {
                GmailToDnnSync.Visible = true;
                GmailToDnnClickedHiddenField.Value = "false";
            }
            else if (DnnToGmailClickedHiddenField.Value.Equals("true"))
            {
                DnnToGmailSync.Visible = true;
                DnnToGmailClickedHiddenField.Value = "false";
            }
        }

        protected void CancelButton_Click(object sender, EventArgs e)
        {
            ResetControls();
        }
    }
}