using System;
using DotNetNuke.Security.Membership;
using DotNetNuke.Security.Roles;
using DotNetNuke.Entities.Users;
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
using System.Linq;
using System.Collections;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Net.Security;
using Aspose.DNN.GmailSync.Components;
using Aspose.DNN.GmailSync.Data;
using DotNetNuke.Entities.Profile;
using Aspose.Email.Mail;
using Aspose.Email.Google;
using Aspose.Email.Services.Google;

namespace Aspose.DNN.GmailSync
{
    public partial class DnnToGmailSync : GmailSyncModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetControls();
            }
            CheckGmailDetails();
        }

        private void CheckGmailDetails()
        {
            Aspose_GmailSync_ServerDetails gmailDetailsList = DatabaseHelper.CheckGmailDetails(UserId);
            if (gmailDetailsList != null)
            {
                ViewState["GmailDetails"] = gmailDetailsList;
            }
        }

        public void ResetControls()
        {
            ProcessSummaryDiv.Visible = DnnToGmail_MainDiv.Visible = NoRowSelectedErrorDiv.Visible = false;
            Into_Div.Visible = true;
        }

        protected void DNNToGmailSyncButton_Click(object sender, EventArgs e)
        {
            NoRowSelectedErrorDiv.Visible = false;

            if (ViewState["GmailDetails"] != null)
            {
                List<string> alreadyExistingList = new System.Collections.Generic.List<string>();
                List<UserInfo> usersList = new List<UserInfo>();

                foreach (GridViewRow row in DNNUsersGridView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("SelectedCheckBox") as CheckBox);
                        if (chkRow.Checked)
                        {
                            int userId = Convert.ToInt32(DNNUsersGridView.DataKeys[row.RowIndex].Value.ToString());
                            usersList.Add(UserController.GetUserById(PortalId, userId));
                        }
                    }
                }

                if (usersList.Count() > 0)
                {
                    Aspose_GmailSync_ServerDetails serverDetails = (Aspose_GmailSync_ServerDetails)ViewState["GmailDetails"];

                    using (IGmailClient client = Aspose.Email.Google.GmailClient.GetInstance(serverDetails.ClientID, serverDetails.ClientSecret, serverDetails.RefreshToken))
                    {
                        Contact[] contacts = client.GetAllContacts();
                        Contact[] validContacts = (from contactsList in contacts where contactsList.EmailAddresses.Count > 0 select contactsList).ToArray<Contact>();

                        foreach (UserInfo user in usersList)
                        {
                            if (validContacts.FirstOrDefault(x => x.EmailAddresses[0].Address.Equals(user.Email)) == null)
                            {
                                Contact contact = BuildNewGmailContact(user);
                                client.CreateContact(contact, serverDetails.Email);
                            }
                            else
                            {
                                alreadyExistingList.Add(user.Email);
                            }
                        }
                    }
                }
                else
                {
                    NoRowSelectedErrorDiv.Visible = true;
                    return;
                }

                ImportedLiteral.Text = string.Format("{0} contact(s) have been imported to Gmail Server successfully.", (usersList.Count() - alreadyExistingList.Count));

                if (alreadyExistingList.Count > 0)
                {
                    AlreadyExistingLiteral.Text = "The following contacts already exists in Gmail Server and therefore not imported";
                    foreach (string email in alreadyExistingList)
                        AlreadyExistingLiteral.Text += "<br>" + email;
                }

                ProcessSummaryDiv.Visible = true;
                Into_Div.Visible = false;
                DnnToGmail_MainDiv.Visible = false;
            }
        }

        private Contact BuildNewGmailContact(UserInfo user)
        {
            Contact contact = new Contact();            
            EmailAddress ea = new EmailAddress();
            ea.Address = user.Email;
            contact.EmailAddresses.Work = ea;

            contact.GivenName = user.FirstName;
            contact.Surname = user.LastName;
            contact.DisplayName = user.DisplayName;

            ProfileController.GetUserProfile(ref user);

            contact.MiddleName = user.Profile.GetPropertyValue("MiddleName");
            contact.InstantMessengers.GoogleTalk = user.Profile.IM;

            contact.PhysicalAddresses.HomeAddress = new PostalAddress();
            contact.PhysicalAddresses.HomeAddress.Street = user.Profile.Street;
            contact.PhysicalAddresses.HomeAddress.City = user.Profile.City;
            contact.PhysicalAddresses.HomeAddress.StateOrProvince = user.Profile.Region;
            contact.PhysicalAddresses.HomeAddress.PostalCode = user.Profile.PostalCode;
            contact.PhysicalAddresses.HomeAddress.Country = user.Profile.Country;

            if (!string.IsNullOrEmpty(user.Profile.Telephone)) contact.PhoneNumbers.Work = user.Profile.Telephone;
            if (!string.IsNullOrEmpty(user.Profile.Cell)) contact.PhoneNumbers.Mobile = user.Profile.Cell;
            if (!string.IsNullOrEmpty(user.Profile.Website)) contact.Urls.BusinessHomePage = user.Profile.Website;

            return contact;
        }

        protected void GetDNNUsersButton_Click(object sender, EventArgs e)
        {
            Into_Div.Visible = false;
            DnnToGmail_MainDiv.Visible = true;

            if (ViewState["GmailDetails"] != null)
            {
                ArrayList dnnUsersArrayList = UserController.GetUsers(PortalId);
                DNNUsersGridView.DataSource = dnnUsersArrayList;
                DNNUsersGridView.DataBind();
            }
        }
    }
}