using System;
using DotNetNuke.Security.Membership;
using DotNetNuke.Security.Roles;
using DotNetNuke.Entities.Users;
using System.Collections.Generic;
using DotNetNuke.Common.Utilities;
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
    public partial class DnnToExchangeSync : ExchangeSyncModuleBase
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetControls();
            }
            CheckExchangeDetails();
        }

        private void CheckExchangeDetails()
        {
            Aspose_ExchangeSync_ServerDetails exchangeDetailsList = DatabaseHelper.CheckExchangeDetails(UserId);
            if (exchangeDetailsList != null)
            {
                ViewState["ExchangeDetails"] = exchangeDetailsList;
            }
        }

        public void ResetControls()
        {
            ProcessSummaryDiv.Visible = DnnToExchange_MainDiv.Visible = NoRowSelectedErrorDiv.Visible = false;
            Into_Div.Visible = true;
        }

        protected void DNNToExchangeSyncButton_Click(object sender, EventArgs e)
        {
            NoRowSelectedErrorDiv.Visible = false;

            if (ViewState["ExchangeDetails"] != null)
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
                    Aspose_ExchangeSync_ServerDetails serverDetails = (Aspose_ExchangeSync_ServerDetails)ViewState["ExchangeDetails"];

                    NetworkCredential credentials = new NetworkCredential(serverDetails.Username, serverDetails.Password, serverDetails.Domain);
                    IEWSClient client = EWSClient.GetEWSClient(serverDetails.ServerURL, credentials);

                    MapiContact[] contacts = client.ListContacts(client.MailboxInfo.ContactsUri);

                    foreach (UserInfo user in usersList)
                    {
                        if (contacts.FirstOrDefault(x => x.ElectronicAddresses.Email1.EmailAddress.Equals(user.Email)) == null)
                        {
                            MapiContact contact = BuildNewExchangeContact(user);
                            client.CreateContact(contact);
                        }
                        else
                        {
                            alreadyExistingList.Add(user.Email);
                        }
                    }
                }
                else
                {
                    NoRowSelectedErrorDiv.Visible = true;
                    return;
                }

                ImportedLiteral.Text = string.Format("{0} contact(s) have been imported to Exchange Server successfully.", (usersList.Count() - alreadyExistingList.Count));

                if (alreadyExistingList.Count > 0)
                {
                    AlreadyExistingLiteral.Text = "The following contacts already exists in Microsoft Exchange Server and therefore not imported";
                    foreach (string email in alreadyExistingList)
                        AlreadyExistingLiteral.Text += "<br>" + email;
                }

                ProcessSummaryDiv.Visible = true;
                Into_Div.Visible = false;
                DnnToExchange_MainDiv.Visible = false;
            }
        }

        private string GetUserEmailAddress(MapiContact contact)
        {
            string emailAddress = string.Empty;

            emailAddress = contact.ElectronicAddresses.Email1.EmailAddress;
            if (!string.IsNullOrEmpty(emailAddress)) return emailAddress;

            emailAddress = contact.ElectronicAddresses.Email2.EmailAddress;
            if (!string.IsNullOrEmpty(emailAddress)) return emailAddress;

            emailAddress = contact.ElectronicAddresses.Email3.EmailAddress;
            if (!string.IsNullOrEmpty(emailAddress)) return emailAddress;

            return emailAddress;
        }

        private MapiContact BuildNewExchangeContact(UserInfo user)
        {
            MapiContact contact = new MapiContact();
            contact.ElectronicAddresses.Email1.EmailAddress = user.Email;
            contact.NameInfo.GivenName = user.FirstName;
            contact.NameInfo.Surname = user.LastName;
            contact.NameInfo.DisplayName = user.DisplayName;

            ProfileController.GetUserProfile(ref user);

            contact.Telephones.HomeTelephoneNumber = user.Profile.Telephone;
            contact.NameInfo.MiddleName = user.Profile.GetProperty("MiddleName").PropertyValue;
            contact.Telephones.BusinessTelephoneNumber = user.Profile.Telephone;
            contact.Telephones.MobileTelephoneNumber = user.Profile.Cell;
            contact.PersonalInfo.BusinessHomePage = user.Profile.Website;
            contact.PersonalInfo.InstantMessagingAddress = user.Profile.IM;

            contact.PhysicalAddresses.HomeAddress.Street = user.Profile.Street;
            contact.PhysicalAddresses.HomeAddress.City = user.Profile.City;
            contact.PhysicalAddresses.HomeAddress.StateOrProvince = user.Profile.Region;
            contact.PhysicalAddresses.HomeAddress.PostalCode = user.Profile.PostalCode;
            contact.PhysicalAddresses.HomeAddress.Country = user.Profile.Country;

            return contact;
        }

        protected void GetDNNUsersButton_Click(object sender, EventArgs e)
        {
            Into_Div.Visible = false;
            DnnToExchange_MainDiv.Visible = true;

            if (ViewState["ExchangeDetails"] != null)
            {
                ArrayList dnnUsersArrayList = UserController.GetUsers(PortalId);
                DNNUsersGridView.DataSource = dnnUsersArrayList;
                DNNUsersGridView.DataBind();
            }
        }
    }
}