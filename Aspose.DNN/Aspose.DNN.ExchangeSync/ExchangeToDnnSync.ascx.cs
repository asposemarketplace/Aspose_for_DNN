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
    public partial class ExchangeToDnnSync : ExchangeSyncModuleBase
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
            ProcessSummaryDiv.Visible = ExchangeToDnn_MainDiv.Visible = NoRowSelectedErrorDiv.Visible = false;
            Into_Div.Visible = true;
        }

        protected void ExchangeToDNNSyncButton_Click(object sender, EventArgs e)
        {
            NoRowSelectedErrorDiv.Visible = false;

            if (ViewState["ExchangeDetails"] != null)
            {
                List<string> alreadyExistingList = new System.Collections.Generic.List<string>();

                List<string> contactsList = new List<string>();

                foreach (GridViewRow row in ExchangeContactsGridView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("SelectedCheckBox") as CheckBox);
                        if (chkRow.Checked)
                        {
                            string email = ExchangeContactsGridView.DataKeys[row.RowIndex].Value.ToString();
                            contactsList.Add(email);
                        }
                    }
                }

                if (contactsList.Count() > 0)
                {
                    Aspose_ExchangeSync_ServerDetails serverDetails = (Aspose_ExchangeSync_ServerDetails)ViewState["ExchangeDetails"];

                    NetworkCredential credentials = new NetworkCredential(serverDetails.Username, serverDetails.Password, serverDetails.Domain);
                    IEWSClient client = EWSClient.GetEWSClient(serverDetails.ServerURL, credentials);

                    MapiContact[] contacts = client.ListContacts(client.MailboxInfo.ContactsUri);

                    foreach (MapiContact contact in contacts)
                    {
                        if (contactsList.Contains(contact.ElectronicAddresses.Email1.EmailAddress))
                        {
                            // check user existance
                            int totalUsers = 0;
                            ArrayList users = UserController.GetUsersByEmail(PortalId, contact.ElectronicAddresses.Email1.EmailAddress, 1, 50, ref totalUsers);

                            if (users.Count <= 0)
                            {
                                UserInfo userInfo = UserController.GetUserByName(contact.ElectronicAddresses.Email1.EmailAddress);

                                if (userInfo == null)
                                {
                                    CreateDNNUser(contact);
                                }
                                else
                                {
                                    alreadyExistingList.Add(contact.ElectronicAddresses.Email1.EmailAddress);
                                }
                            }
                            else
                            {
                                alreadyExistingList.Add(contact.ElectronicAddresses.Email1.EmailAddress);
                            }
                        }
                    }
                }
                else
                {
                    NoRowSelectedErrorDiv.Visible = true;
                    return;
                }

                ImportedLiteral.Text = string.Format("{0} contact(s) have been imported to DNN successfully.", (contactsList.Count() - alreadyExistingList.Count));

                if (alreadyExistingList.Count > 0)
                {
                    AlreadyExistingLiteral.Text = "The following contacts already exists in DNN and therefore not imported";
                    foreach (string email in alreadyExistingList)
                        AlreadyExistingLiteral.Text += "<br>" + email;
                }

                ProcessSummaryDiv.Visible = true;
                Into_Div.Visible = false;
                ExchangeToDnn_MainDiv.Visible = false;
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

        private UserCreateStatus CreateDNNUser(MapiContact contact)
        {
            UserInfo user = new UserInfo();
            user.Email = GetUserEmailAddress(contact);
            user.Username = user.Email;
            user.FirstName = contact.NameInfo.GivenName;
            user.LastName = contact.NameInfo.Surname;
            user.DisplayName = contact.NameInfo.DisplayName;

            user.Membership.Password = UserController.GeneratePassword(12).ToString();
            user.PortalID = PortalId;
            user.IsSuperUser = false;

            UserCreateStatus createStatus = UserCreateStatus.AddUser;

            //Create the User
            createStatus = UserController.CreateUser(ref user);

            if (createStatus == UserCreateStatus.Success)
            {
                ProfileController.GetUserProfile(ref user);

                user.Profile.FirstName = contact.NameInfo.GivenName;
                user.Profile.SetProfileProperty("MiddleName", contact.NameInfo.MiddleName);
                user.Profile.LastName = contact.NameInfo.Surname;

                user.Profile.Telephone = string.IsNullOrEmpty(contact.Telephones.BusinessTelephoneNumber) ? contact.Telephones.HomeTelephoneNumber : contact.Telephones.BusinessTelephoneNumber;
                user.Profile.Cell = contact.Telephones.MobileTelephoneNumber;
                user.Profile.Website = string.IsNullOrEmpty(contact.PersonalInfo.BusinessHomePage) ? contact.PersonalInfo.Html : contact.PersonalInfo.BusinessHomePage;
                user.Profile.IM = contact.PersonalInfo.InstantMessagingAddress;

                user.Profile.Street = contact.PhysicalAddresses.HomeAddress.Street;
                user.Profile.City = contact.PhysicalAddresses.HomeAddress.City;
                user.Profile.Region = contact.PhysicalAddresses.HomeAddress.StateOrProvince;
                user.Profile.PostalCode = contact.PhysicalAddresses.HomeAddress.PostalCode;
                user.Profile.Country = contact.PhysicalAddresses.HomeAddress.Country;

                user.Profile.Street = string.IsNullOrEmpty(user.Profile.Street) ? contact.PhysicalAddresses.WorkAddress.Street : user.Profile.Street;
                user.Profile.City = string.IsNullOrEmpty(user.Profile.City) ? contact.PhysicalAddresses.WorkAddress.City : user.Profile.City;
                user.Profile.Region = string.IsNullOrEmpty(user.Profile.Region) ? contact.PhysicalAddresses.WorkAddress.StateOrProvince : user.Profile.Region;
                user.Profile.PostalCode = string.IsNullOrEmpty(user.Profile.PostalCode) ? contact.PhysicalAddresses.WorkAddress.PostalCode : user.Profile.PostalCode;
                user.Profile.Country = string.IsNullOrEmpty(user.Profile.Country) ? contact.PhysicalAddresses.WorkAddress.Country : user.Profile.Country;

                user.Profile.Street = string.IsNullOrEmpty(user.Profile.Street) ? contact.PhysicalAddresses.OtherAddress.Street : user.Profile.Street;
                user.Profile.City = string.IsNullOrEmpty(user.Profile.City) ? contact.PhysicalAddresses.OtherAddress.City : user.Profile.City;
                user.Profile.Region = string.IsNullOrEmpty(user.Profile.Region) ? contact.PhysicalAddresses.OtherAddress.StateOrProvince : user.Profile.Region;
                user.Profile.PostalCode = string.IsNullOrEmpty(user.Profile.PostalCode) ? contact.PhysicalAddresses.OtherAddress.PostalCode : user.Profile.PostalCode;
                user.Profile.Country = string.IsNullOrEmpty(user.Profile.Country) ? contact.PhysicalAddresses.OtherAddress.Country : user.Profile.Country;

                ProfileController.UpdateUserProfile(user);

                DataCache.ClearPortalCache(user.PortalID, false);

                RoleController objRoles = new RoleController();

                List<int> rolesList = new List<int>();

                foreach (int roleID in rolesList)
                {
                    objRoles.AddUserRole(user.PortalID, user.UserID, roleID, Null.NullDate, Null.NullDate);
                }
            }

            return createStatus;
        }

        private List<int> GetSelectedRoles()
        {
            List<int> rolesList = new List<int>();

            foreach (GridViewRow row in RolesGridView.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("SelectedCheckBox") as CheckBox);
                    if (chkRow.Checked)
                    {
                        int roleID = Convert.ToInt32(RolesGridView.DataKeys[row.RowIndex].Value.ToString());
                        rolesList.Add(roleID);
                    }
                }
            }

            return rolesList;
        }

        private void RenderRoles()
        {
            IList<RoleInfo> arrRoles = new RoleController().GetRoles(PortalId);
            RolesGridView.DataSource = arrRoles;
            RolesGridView.DataBind();
        }

        protected void GetExchangeContactsButton_Click(object sender, EventArgs e)
        {
            Into_Div.Visible = false;
            ExchangeToDnn_MainDiv.Visible = true;

            RenderRoles();

            if (ViewState["ExchangeDetails"] != null)
            {
                Aspose_ExchangeSync_ServerDetails serverDetails = (Aspose_ExchangeSync_ServerDetails)ViewState["ExchangeDetails"];

                NetworkCredential credentials = new NetworkCredential(serverDetails.Username, serverDetails.Password, serverDetails.Domain);
                IEWSClient client = EWSClient.GetEWSClient(serverDetails.ServerURL, credentials);

                MapiContact[] contacts = client.ListContacts(client.MailboxInfo.ContactsUri);

                List<ExchangeContact> exchangeContactsList = new System.Collections.Generic.List<ExchangeContact>();

                foreach (MapiContact contact in contacts)
                {
                    exchangeContactsList.Add(new ExchangeContact(contact.NameInfo.DisplayName, contact.ElectronicAddresses.Email1.EmailAddress));
                }

                ExchangeContactsGridView.DataSource = exchangeContactsList;
                ExchangeContactsGridView.DataBind();
            }
        }
    }
}