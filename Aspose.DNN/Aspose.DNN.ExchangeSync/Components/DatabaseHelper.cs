using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Data.EntityClient;
using System.Data.Entity;

namespace Aspose.DNN.ExchangeSync.Components
{
    public class DatabaseHelper
    {
        static ExchangeEntities exchEntities;

        private static ExchangeEntities CurrentDBEntities
        {
            get
            {
                if (exchEntities == null)
                {
                    string providerName = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ProviderName;
                    SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
                    sqlBuilder.ConnectionString = ConfigurationManager.ConnectionStrings["SiteSqlServer"].ConnectionString;
                    EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
                    entityBuilder.Provider = providerName;
                    entityBuilder.ProviderConnectionString = sqlBuilder.ToString();

                    entityBuilder.Metadata = "res://*/Data.ExchangeModel.csdl|res://*/Data.ExchangeModel.ssdl|res://*/Data.ExchangeModel.msl";
                    EntityConnection entityConnection = new EntityConnection(entityBuilder.ToString());

                    exchEntities = new ExchangeEntities(entityBuilder.ToString());
                }
                return exchEntities;
            }
        }

        private static string ExchangeDetailsSessionName(int userId)
        {
            return "ExchangeDetailsSession-" + userId.ToString();
        }

        public static Aspose_ExchangeSync_ServerDetails CheckExchangeDetails(int userID)
        {
            Aspose_ExchangeSync_ServerDetails detailsToReturn = null;
            try
            {
                Aspose_ExchangeSync_ServerDetails serverDetails = CurrentDBEntities.Aspose_ExchangeSync_ServerDetails.FirstOrDefault(x => x.UserID == userID);
                if (serverDetails != null)
                {
                    detailsToReturn = new Aspose_ExchangeSync_ServerDetails();
                    detailsToReturn.Username = serverDetails.Username;
                    detailsToReturn.Password = Crypto.Decrypt(serverDetails.Password);
                    detailsToReturn.ServerURL = serverDetails.ServerURL;
                    detailsToReturn.Domain = serverDetails.Domain;
                }                
            }
            catch (Exception)
            {
                // Fall back to session approach if database fails
                if (HttpContext.Current.Session[ExchangeDetailsSessionName(userID)] != null)
                {
                    detailsToReturn = (Aspose_ExchangeSync_ServerDetails)HttpContext.Current.Session[ExchangeDetailsSessionName(userID)];
                }
            }
            return detailsToReturn;   
        }

        public static void AddUpdateServerDetails(Aspose_ExchangeSync_ServerDetails details)
        {
            try
            {
                Aspose_ExchangeSync_ServerDetails serverDetails = CurrentDBEntities.Aspose_ExchangeSync_ServerDetails.FirstOrDefault(x => x.UserID == details.UserID);

                if (serverDetails != null)
                {
                    serverDetails.Username = details.Username;
                    serverDetails.Password = details.Password;
                    serverDetails.ServerURL = details.ServerURL;
                    serverDetails.Domain = details.Domain;
                    CurrentDBEntities.SaveChanges();
                }
                else
                {
                    CurrentDBEntities.Aspose_ExchangeSync_ServerDetails.Add(details);
                    CurrentDBEntities.SaveChanges();
                }
            }
            catch (Exception)
            {
                // Fall back to session approach if database fails
                details.Password = Crypto.Decrypt(details.Password);                    
                HttpContext.Current.Session[ExchangeDetailsSessionName((int)details.UserID)] = details;
            }
        }
    }
}