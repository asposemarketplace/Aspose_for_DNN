<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="View.ascx.cs" Inherits="Aspose.DNN.GmailSync.View" %>
<%@ Register Src="~/desktopmodules/Aspose.DNN.GmailSync/GmailToDnnSync.ascx" TagPrefix="uc1" TagName="GmailToDnnSync" %>
<%@ Register Src="~/desktopmodules/Aspose.DNN.GmailSync/DnnToGmailSync.ascx" TagPrefix="uc1" TagName="DnnToGmailSync" %>

<script type="text/javascript" language="javascript">

    $(document).ready(function () {
        $('.selectAllCheckBox input[type="checkbox"]').click(function (event) {  //on click
            if (this.checked) { // check select status
                $('.selectableCheckBox input[type="checkbox"]').each(function () { //loop through each checkbox
                    this.checked = true;  //select all checkboxes with class "checkbox1"              
                });
            } else {
                $('.selectableCheckBox input[type="checkbox"]').each(function () { //loop through each checkbox
                    this.checked = false; //deselect all checkboxes with class "checkbox1"                      
                });
            }
        });
    });
</script>
<br />
<h2>Aspose .NET Gmail Sync for DNN</h2>
<br />
<div class="dnnFormMessage dnnFormInfo" runat="server" id="LoggedInErrorDiv">You must be logged-in to use this module</div>

<div id="moduleMainDiv" runat="server">
    <div class="DNNContainer_Title_h2 SpacingBottom">
        <div id="dnn_ctr352_ContentPane">
            <div class="DNNModuleContent ModConsoleC" id="dnn_ctr352_ModuleContent">
                <div class="console normal" id="dnn_ctr352_ViewConsole_Console">
                    <div class="console-large">

                        <asp:LinkButton ID="GmailToDnnHyperLink" runat="server" OnClick="GmailToDnnHyperLink_Click" ValidationGroup="DoNotCheck4">
                            <div class="console-large" title="Sync contacts from Gmail Server to DNN">
                                <img width="32" height="32" alt="Configuration Manager" src="/Icons/Sigma/SynchronizeEnabled_32x32_Standard.png" style="border-width: 0px;">
                                <h3>Gmail to DNN Sync</h3>
                                <div class="console-large">Sync contacts from Gmail Server to DNN</div>
                            </div>
                        </asp:LinkButton>

                        <asp:LinkButton ID="DnnToGmailHyperLink" runat="server" OnClick="DnnToGmailHyperLink_Click" ValidationGroup="DoNotCheck5">                            
                            <div class="console-large" title="Sync contacts from DNN to Gmail Server">                        
                                <img width="32" height="32" alt="Dashboard" src="/Icons/Sigma/SynchronizeEnabled_32x32_Standard.png" style="border-width: 0px;">
                                <h3>DNN to Gmail Sync</h3>
                                <div class="console-large">Sync contacts from DNN to Gmail Server</div>
                            </div>                        
                        </asp:LinkButton>
                        <asp:LinkButton ID="GmailSettingsHyperLink" runat="server" OnClick="GmailSettingsHyperLink_Click" ValidationGroup="DoNotCheck6">
                            <div class="console-large" title="Gmail Server Settings">
                                <img width="32" height="32" alt="Host Settings" src="/Icons/Sigma/Hostsettings_32X32_Standard.png" style="border-width: 0px;">
                                <h3>Gmail Settings</h3>
                                <div class="console-large">Gmail Server Settings</div>
                            </div>
                        </asp:LinkButton>
                    </div>
                    <br style="clear: both">
                </div>
            </div>
        </div>
        <div class="clear"></div>
    </div>

    <uc1:GmailToDnnSync runat="server" Visible="false" id="GmailToDnnSync" />
    <uc1:DnnToGmailSync runat="server" Visible="false" id="DnnToGmailSync" />

    <div id="GmailSettings" runat="server" visible="false">
        <div class="dnnForm dnnManageUsers dnnClear ui-tabs ui-widget ui-widget-content ui-corner-all">
            <div class="dnnUserDetails dnnClear" id="dnnUserDetails">
                <div class="udContent dnnClear">
                    <fieldset>
                        <b>Note: </b> Please make sure that you have obtained Client ID and Client Secret as explained on <a href="http://www.aspose.com/docs/display/emailnet/Create+project+in+Google+Developer+Console">http://www.aspose.com/docs/display/emailnet/Create+project+in+Google+Developer+Console</a>
                        <br />
                        <div class="dnnFormItem">
                            <h2 class="dnnFormSectionHead"><span>Gmail Server details</span></h2>
                        </div>
                        <div class="dnnFormMessage dnnFormInfo" runat="server" visible="false" id="GmailCredsErrorDiv">
                            <strong>Oops!</strong> We are unable to connect to mail server using the information you have provided. Please check the information below and try again.
                        </div>
                        <div class="dnnFormItem">
                            <div class="dnnUser register">
                                <div class="dnnForm">
                                    <div class="dnnFormItem dnnFormShort">
                                        <div class="dnnLabel" style="position: relative;">
                                            <label><span class=" dnnFormRequired">Google Email Address:</span></label>
                                            <a href="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(&quot;dnn$ctr393$ManageUsers$User$userForm$userName$Link&quot;, &quot;&quot;, true, &quot;&quot;, &quot;&quot;, false, true))" class="dnnFormHelp" tabindex="-1" id="dnn_ctr393_ManageUsers_User_userForm_userName_Link"></a>
                                            <div class="dnnTooltip" style="position: absolute; right: -29%;">
                                                <div class="dnnFormHelpContent dnnClear">
                                                    <span class="dnnHelpText">Your Google email address e.g. admin@gmail.com</span><a href="#" class="pinHelp"></a>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:TextBox ID="EmailAddressTextBox" CssClass="form-control" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ControlToValidate="EmailAddressTextBox" CssClass="dnnFormMessage dnnFormError" ID="RequiredFieldValidator1" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="dnnFormItem dnnFormShort">
                                        <div class="dnnLabel" style="position: relative;">
                                            <label>
                                                <span class=" dnnFormRequired">Password:</span></label>

                                            <a href="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(&quot;dnn$ctr393$ManageUsers$User$userForm$email$Link&quot;, &quot;&quot;, true, &quot;&quot;, &quot;&quot;, false, true))" class="dnnFormHelp" tabindex="-1" id="dnn_ctr393_ManageUsers_User_userForm_email_Link"></a>
                                            <div class="dnnTooltip" style="position: absolute; right: -29%;">
                                                <div class="dnnFormHelpContent dnnClear">
                                                    <span class="dnnHelpText">Enter your password</span><a href="#" class="pinHelp"></a>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:TextBox ID="PasswordTextBox" runat="server" TextMode="Password" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="dnnFormMessage dnnFormError" ControlToValidate="PasswordTextBox" ID="RequiredFieldValidator3" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="dnnFormItem dnnFormShort">
                                        <div class="dnnLabel" style="position: relative;">
                                            <label>
                                                <span class=" dnnFormRequired">Client ID:</span></label>
                                            <a href="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(&quot;dnn$ctr393$ManageUsers$User$userForm$displayName$Link&quot;, &quot;&quot;, true, &quot;&quot;, &quot;&quot;, false, true))" class="dnnFormHelp" tabindex="-1" id="dnn_ctr393_ManageUsers_User_userForm_displayName_Link"></a>
                                            <div class="dnnTooltip" style="position: absolute; right: -29%;">
                                                <div class="dnnFormHelpContent dnnClear">
                                                    <span class="dnnHelpText">Please provide a valid Client ID Taken from Google Developer console</span><a href="#" class="pinHelp"></a>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:TextBox ID="ClientIDTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="dnnFormMessage dnnFormError" ControlToValidate="ClientIDTextBox" ID="RequiredFieldValidator2" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="dnnFormItem dnnFormShort">
                                        <div class="dnnLabel" style="position: relative;">
                                            <label>
                                                <span class=" dnnFormRequired" id="Span1">Client secret:</span></label>
                                            <a href="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(&quot;dnn$ctr393$ManageUsers$User$userForm$email$Link&quot;, &quot;&quot;, true, &quot;&quot;, &quot;&quot;, false, true))" class="dnnFormHelp" tabindex="-1" id="A1"></a>
                                            <div class="dnnTooltip" style="position: absolute; right: -29%;">
                                                <div class="dnnFormHelpContent dnnClear" id="Div2">
                                                    <span class="dnnHelpText" id="Span2">Please provide a valid Client secret Taken from Google Developer console</span><a href="#" class="pinHelp"></a>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:TextBox ID="ClientSecretTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="dnnFormMessage dnnFormError" ValidationGroup="DoNotCheck" ControlToValidate="ClientSecretTextBox" ID="RequiredFieldValidator5" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </fieldset>
                </div>
            </div>
        </div>
        <div class="dnnForm">
            <ul class="dnnActions dnnClear">
                <li>
                    <asp:Button ID="SaveButton" CssClass="dnnPrimaryAction" runat="server" Text="Save" OnClick="SaveButton_Click"></asp:Button>
                </li>
                <li>
                    <asp:Button ID="CancelButton" OnClick="CancelButton_Click" CssClass="dnnSecondaryAction" runat="server" Text="Cancel"></asp:Button>
                </li>
            </ul>
        </div>
    </div>


</div>

<asp:HiddenField ID="GmailToDnnClickedHiddenField" Value="false" runat="server" />
<asp:HiddenField ID="DnnToGmailClickedHiddenField" Value="false" runat="server" />
