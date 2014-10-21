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
    public partial class GmailToDnnSync : GmailSyncModuleBase
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
            ProcessSummaryDiv.Visible = GmailToDnn_MainDiv.Visible = NoRowSelectedErrorDiv.Visible = false;
            Into_Div.Visible = true;
        }

        protected void GmailToDNNSyncButton_Click(object sender, EventArgs e)
        {
            NoRowSelectedErrorDiv.Visible = false;

            if (ViewState["GmailDetails"] != null)
            {
                List<string> alreadyExistingList = new System.Collections.Generic.List<string>();

                List<string> contactsList = new List<string>();

                foreach (GridViewRow row in GmailContactsGridView.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("SelectedCheckBox") as CheckBox);
                        if (chkRow.Checked)
                        {
                            string email = GmailContactsGridView.DataKeys[row.RowIndex].Value.ToString();
                            contactsList.Add(email);
                        }
                    }
                }

                if (contactsList.Count() > 0)
                {
                    Aspose_GmailSync_ServerDetails serverDetails = (Aspose_GmailSync_ServerDetails)ViewState["GmailDetails"];
                    List<Components.GmailContact> gmailContactsList = new List<Components.GmailContact>();

                    using (IGmailClient client = Aspose.Email.Google.GmailClient.GetInstance(serverDetails.ClientID, serverDetails.ClientSecret, serverDetails.RefreshToken))
                    {
                        Contact[] contacts = client.GetAllContacts();

                        foreach (Contact contact in contacts)
                        {
                            if (contactsList.Contains(contact.EmailAddresses[0].Address))
                            {
                                // check user existence
                                int totalUsers = 0;
                                ArrayList users = UserController.GetUsersByEmail(PortalId, contact.EmailAddresses[0].Address, 1, 50, ref totalUsers);

                                if (users.Count <= 0)
                                {
                                    UserInfo userInfo = UserController.GetUserByName(contact.EmailAddresses[0].Address);

                                    if (userInfo == null)
                                    {
                                        CreateDNNUser(contact);
                                    }
                                    else
                                    {
                                        alreadyExistingList.Add(contact.EmailAddresses[0].Address);
                                    }
                                }
                                else
                                {
                                    alreadyExistingList.Add(contact.EmailAddresses[0].Address);
                                }
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
                GmailToDnn_MainDiv.Visible = false;
            }
        }

        private UserCreateStatus CreateDNNUser(Contact contact)
        {
            UserInfo user = new UserInfo();
            user.Email = contact.EmailAddresses[0].Address;
            user.Username = user.Email;
            user.FirstName = contact.GivenName;
            user.LastName = contact.Surname;
            user.DisplayName = contact.DisplayName;

            user.Membership.Password = UserController.GeneratePassword(12).ToString();
            user.PortalID = PortalId;
            user.IsSuperUser = false;

            UserCreateStatus createStatus = UserCreateStatus.AddUser;

            //Create the User
            createStatus = UserController.CreateUser(ref user);

            if (createStatus == UserCreateStatus.Success)
            {
                ProfileController.GetUserProfile(ref user);

                user.Profile.FirstName = contact.GivenName;
                user.Profile.SetProfileProperty("MiddleName", contact.MiddleName);
                user.Profile.LastName = contact.Surname;

                user.Profile.Telephone = string.IsNullOrEmpty(contact.PhoneNumbers.Work) ? contact.PhoneNumbers.Home : contact.PhoneNumbers.Mobile;
                user.Profile.Cell = contact.PhoneNumbers.Mobile;
                user.Profile.Website = string.IsNullOrEmpty(contact.Urls.BusinessHomePage) ? contact.Urls.HomePage : contact.Urls.BusinessHomePage;
                user.Profile.IM = contact.InstantMessengers.Skype;

                if (string.IsNullOrEmpty(user.Profile.IM)) user.Profile.IM = contact.InstantMessengers.Yahoo;
                if (string.IsNullOrEmpty(user.Profile.IM)) user.Profile.IM = contact.InstantMessengers.QQ;
                if (string.IsNullOrEmpty(user.Profile.IM)) user.Profile.IM = contact.InstantMessengers.MSN;
                if (string.IsNullOrEmpty(user.Profile.IM)) user.Profile.IM = contact.InstantMessengers.GoogleTalk;

                if (contact.PhysicalAddresses.HomeAddress != null)
                {
                    user.Profile.Street = contact.PhysicalAddresses.HomeAddress.Street;
                    user.Profile.City = contact.PhysicalAddresses.HomeAddress.City;
                    user.Profile.Region = contact.PhysicalAddresses.HomeAddress.StateOrProvince;
                    user.Profile.PostalCode = contact.PhysicalAddresses.HomeAddress.PostalCode;
                    user.Profile.Country = contact.PhysicalAddresses.HomeAddress.Country;
                }

                if (contact.PhysicalAddresses.WorkAddress != null)
                {
                    user.Profile.Street = string.IsNullOrEmpty(user.Profile.Street) ? contact.PhysicalAddresses.WorkAddress.Street : user.Profile.Street;
                    user.Profile.City = string.IsNullOrEmpty(user.Profile.City) ? contact.PhysicalAddresses.WorkAddress.City : user.Profile.City;
                    user.Profile.Region = string.IsNullOrEmpty(user.Profile.Region) ? contact.PhysicalAddresses.WorkAddress.StateOrProvince : user.Profile.Region;
                    user.Profile.PostalCode = string.IsNullOrEmpty(user.Profile.PostalCode) ? contact.PhysicalAddresses.WorkAddress.PostalCode : user.Profile.PostalCode;
                    user.Profile.Country = string.IsNullOrEmpty(user.Profile.Country) ? contact.PhysicalAddresses.WorkAddress.Country : user.Profile.Country;
                }

                if (contact.PhysicalAddresses.OtherAddress != null)
                {
                    user.Profile.Street = string.IsNullOrEmpty(user.Profile.Street) ? contact.PhysicalAddresses.OtherAddress.Street : user.Profile.Street;
                    user.Profile.City = string.IsNullOrEmpty(user.Profile.City) ? contact.PhysicalAddresses.OtherAddress.City : user.Profile.City;
                    user.Profile.Region = string.IsNullOrEmpty(user.Profile.Region) ? contact.PhysicalAddresses.OtherAddress.StateOrProvince : user.Profile.Region;
                    user.Profile.PostalCode = string.IsNullOrEmpty(user.Profile.PostalCode) ? contact.PhysicalAddresses.OtherAddress.PostalCode : user.Profile.PostalCode;
                    user.Profile.Country = string.IsNullOrEmpty(user.Profile.Country) ? contact.PhysicalAddresses.OtherAddress.Country : user.Profile.Country;
                }

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

        protected void GetGmailContactsButton_Click(object sender, EventArgs e)
        {
            Into_Div.Visible = false;
            GmailToDnn_MainDiv.Visible = true;

            RenderRoles();

            if (ViewState["GmailDetails"] != null)
            {
                Aspose_GmailSync_ServerDetails serverDetails = (Aspose_GmailSync_ServerDetails)ViewState["GmailDetails"];
                List<Components.GmailContact> gmailContactsList = new List<Components.GmailContact>();

                using (IGmailClient client = Aspose.Email.Google.GmailClient.GetInstance(serverDetails.ClientID, serverDetails.ClientSecret, serverDetails.RefreshToken))
                {
                    Contact[] contacts = client.GetAllContacts();

                    foreach (Contact c in contacts)
                    {
                        if (c.EmailAddresses.Count > 0)
                            gmailContactsList.Add(new Components.GmailContact(c.DisplayName, c.EmailAddresses[0].Address));
                    }

                    GmailContactsGridView.DataSource = gmailContactsList;
                    GmailContactsGridView.DataBind();
                }
            }
        }
    }
}