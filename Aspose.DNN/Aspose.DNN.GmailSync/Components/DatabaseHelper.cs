using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data.Entity;
using Aspose.DNN.GmailSync.Data;

namespace Aspose.DNN.GmailSync.Components
{
    public class DatabaseHelper
    {
        static GmailSyncEntities gmailSyncEntities;

        private static GmailSyncEntities CurrentDBEntities
        {
            get
            {
                if (gmailSyncEntities == null)
                {
                    string providerName = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ProviderName;
                    SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
                    sqlBuilder.ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
                    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                    entityBuilder.Provider = providerName;
                    entityBuilder.ProviderConnectionString = sqlBuilder.ToString();

                    entityBuilder.Metadata = "res://*/Data.GmailSyncModel.csdl|res://*/Data.GmailSyncModel.ssdl|res://*/Data.GmailSyncModel.msl";
                    EntityConnection entityConnection = new EntityConnection(entityBuilder.ToString());

                    gmailSyncEntities = new GmailSyncEntities(entityBuilder.ToString());
                }
                return gmailSyncEntities;
            }
        }

        private static string GmailDetailsSessionName(int userId)
        {
            return "GmailDetailsSession-" + userId.ToString();
        }

        public static Aspose_GmailSync_ServerDetails CheckGmailDetails(int userID)
        {
            Aspose_GmailSync_ServerDetails detailsToReturn = null;
            try
            {
                Aspose_GmailSync_ServerDetails serverDetails = CurrentDBEntities.Aspose_GmailSync_ServerDetails.FirstOrDefault(x => x.DNNUserID == userID);
                if (serverDetails != null)
                {
                    detailsToReturn = new Aspose_GmailSync_ServerDetails();
                    detailsToReturn.DNNUserID = serverDetails.DNNUserID;
                    detailsToReturn.Username = serverDetails.Username;
                    detailsToReturn.Email = serverDetails.Email;
                    detailsToReturn.Password = Crypto.Decrypt(serverDetails.Password);
                    detailsToReturn.ClientID = Crypto.Decrypt(serverDetails.ClientID);
                    detailsToReturn.ClientSecret = Crypto.Decrypt(serverDetails.ClientSecret);
                    detailsToReturn.RefreshToken = Crypto.Decrypt(serverDetails.RefreshToken);
                }
            }
            catch (Exception)
            {
                // Fall back to session approach if database fails
                if (HttpContext.Current.Session[GmailDetailsSessionName(userID)] != null)
                {
                    detailsToReturn = (Aspose_GmailSync_ServerDetails)HttpContext.Current.Session[GmailDetailsSessionName(userID)];
                }
            }
            return detailsToReturn;
        }

        public static void AddUpdateServerDetails(Aspose_GmailSync_ServerDetails details)
        {
            try
            {
                Aspose_GmailSync_ServerDetails serverDetails = CurrentDBEntities.Aspose_GmailSync_ServerDetails.FirstOrDefault(x => x.DNNUserID == details.DNNUserID);

                if (serverDetails != null)
                {
                    serverDetails.DNNUserID = details.DNNUserID;
                    serverDetails.Username = details.Username;
                    serverDetails.Email = details.Email;
                    serverDetails.Password = details.Password;
                    serverDetails.ClientID = details.ClientID;
                    serverDetails.ClientSecret = details.ClientSecret;
                    serverDetails.RefreshToken = details.RefreshToken;
                    CurrentDBEntities.SaveChanges();
                }
                else
                {
                    CurrentDBEntities.Aspose_GmailSync_ServerDetails.Add(details);
                    CurrentDBEntities.SaveChanges();
                }
            }
            catch (Exception)
            {
                // Fall back to session approach if database fails
                details.Password = Crypto.Decrypt(details.Password);
                HttpContext.Current.Session[GmailDetailsSessionName((int)details.DNNUserID)] = details;
            }
        }
    }
}