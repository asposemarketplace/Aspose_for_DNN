using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aspose.DNN.GmailSync.Components
{
    public class GmailContact
    {
        public GmailContact() { }
        public GmailContact(string displayName, string email) { DisplayName = displayName; Email = email; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
    }
}