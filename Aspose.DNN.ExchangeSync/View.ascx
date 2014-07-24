<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="View.ascx.cs" Inherits="Aspose.DNN.ExchangeSync.View" %>
<%@ Register Src="~/desktopmodules/Aspose.DNN.ExchangeSync/ExchangeToDnnSync.ascx" TagPrefix="uc1" TagName="ExchangeToDnnSync" %>
<%@ Register Src="~/desktopmodules/Aspose.DNN.ExchangeSync/DnnToExchangeSync.ascx" TagPrefix="uc1" TagName="DnnToExchangeSync" %>

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

<div class="dnnFormMessage dnnFormInfo" runat="server" id="LoggedInErrorDiv">You must be logged-in to use this module</div>

<div id="moduleMainDiv" runat="server">
    <div class="DNNContainer_Title_h2 SpacingBottom">
        <div id="dnn_ctr352_ContentPane">
            <div class="DNNModuleContent ModConsoleC" id="dnn_ctr352_ModuleContent">
                <div class="console normal" id="dnn_ctr352_ViewConsole_Console">
                    <div class="console-large">

                        <asp:LinkButton ID="ExchangeToDnnHyperLink" runat="server" OnClick="ExchangeToDnnHyperLink_Click" ValidationGroup="DoNotCheck4">
                            <div class="console-large" title="Sync contacts from Microsoft Exchange Server to DNN">
                                <img width="32" height="32" alt="Configuration Manager" src="/Icons/Sigma/SynchronizeEnabled_32x32_Standard.png" style="border-width: 0px;">
                                <h3>Exchange to DNN Sync</h3>
                                <div class="console-large">Sync contacts from Microsoft Exchange Server to DNN</div>
                            </div>
                        </asp:LinkButton>

                        <asp:LinkButton ID="DnnToExchangeHyperLink" runat="server" OnClick="DnnToExchangeHyperLink_Click" ValidationGroup="DoNotCheck5">                            
                            <div class="console-large" title="Sync contacts from DNN to Microsoft Exchange Server">                        
                                <img width="32" height="32" alt="Dashboard" src="/Icons/Sigma/SynchronizeEnabled_32x32_Standard.png" style="border-width: 0px;">
                                <h3>DNN to Exchange Sync</h3>
                                <div class="console-large">Sync contacts from DNN to Microsoft Exchange Server</div>
                            </div>                        
                        </asp:LinkButton>
                        <asp:LinkButton ID="ExchangeSettingsHyperLink" runat="server" OnClick="ExchangeSettingsHyperLink_Click" ValidationGroup="DoNotCheck6">
                            <div class="console-large" title="Microsoft Exchange Server Settings">
                                <img width="32" height="32" alt="Host Settings" src="/Icons/Sigma/Hostsettings_32X32_Standard.png" style="border-width: 0px;">
                                <h3>Exchange Settings</h3>
                                <div class="console-large">Microsoft Exchange Server Settings</div>
                            </div>
                        </asp:LinkButton>
                    </div>
                    <br style="clear: both">
                </div>
            </div>
        </div>
        <div class="clear"></div>
    </div>

    <uc1:ExchangeToDnnSync runat="server" Visible="false" id="ExchangeToDnnSync" />

    <uc1:DnnToExchangeSync runat="server" Visible="false" id="DnnToExchangeSync" />

    <div id="ExchangeSettings" runat="server" visible="false">
        <div class="dnnForm dnnManageUsers dnnClear ui-tabs ui-widget ui-widget-content ui-corner-all">
            <div class="dnnUserDetails dnnClear" id="dnnUserDetails">
                <div class="udContent dnnClear">
                    <fieldset>
                        <div class="dnnFormItem">
                            <h2 class="dnnFormSectionHead"><span>Microsoft Exchange Server details</span></h2>
                        </div>
                        <div class="dnnFormMessage dnnFormInfo" runat="server" visible="false" id="ExchangeCredsErrorDiv">
                            <strong>Oops!</strong> We are unable to connect to mail server using the information you have provided. Please check the information below and try again.
                        </div>
                        <div class="dnnFormItem">
                            <div class="dnnUser register">
                                <div class="dnnForm">
                                    <div class="dnnFormItem dnnFormShort">
                                        <div class="dnnLabel" style="position: relative;">
                                            <label><span class=" dnnFormRequired">Server URL:</span></label>
                                            <a href="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(&quot;dnn$ctr393$ManageUsers$User$userForm$userName$Link&quot;, &quot;&quot;, true, &quot;&quot;, &quot;&quot;, false, true))" class="dnnFormHelp" tabindex="-1" id="dnn_ctr393_ManageUsers_User_userForm_userName_Link"></a>
                                            <div class="dnnTooltip" style="position: absolute; right: -29%;">
                                                <div class="dnnFormHelpContent dnnClear">
                                                    <span class="dnnHelpText">Enter a Server URL.  It should be similar to https://exchange.domain.com/Exchange.asmx</span><a href="#" class="pinHelp"></a>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:TextBox ID="ServerURLTextBox" CssClass="form-control" runat="server"></asp:TextBox>
                                        <asp:RequiredFieldValidator ControlToValidate="ServerURLTextBox" CssClass="dnnFormMessage dnnFormError" ID="RequiredFieldValidator1" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator>
                                    </div>
                                    <div class="dnnFormItem dnnFormShort">
                                        <div class="dnnLabel" style="position: relative;">
                                            <label>
                                                <span class=" dnnFormRequired">Username:</span></label>
                                            <a href="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(&quot;dnn$ctr393$ManageUsers$User$userForm$displayName$Link&quot;, &quot;&quot;, true, &quot;&quot;, &quot;&quot;, false, true))" class="dnnFormHelp" tabindex="-1" id="dnn_ctr393_ManageUsers_User_userForm_displayName_Link"></a>
                                            <div class="dnnTooltip" style="position: absolute; right: -29%;">
                                                <div class="dnnFormHelpContent dnnClear">
                                                    <span class="dnnHelpText">Provide a username, it may be your email address as well</span><a href="#" class="pinHelp"></a>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:TextBox ID="UsernameTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="dnnFormMessage dnnFormError" ControlToValidate="UsernameTextBox" ID="RequiredFieldValidator2" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator>
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
                                                <span class=" dnnFormRequired" id="Span1">Domain:</span></label>
                                            <a href="javascript:WebForm_DoPostBackWithOptions(new WebForm_PostBackOptions(&quot;dnn$ctr393$ManageUsers$User$userForm$email$Link&quot;, &quot;&quot;, true, &quot;&quot;, &quot;&quot;, false, true))" class="dnnFormHelp" tabindex="-1" id="A1"></a>
                                            <div class="dnnTooltip" style="position: absolute; right: -29%;">
                                                <div class="dnnFormHelpContent dnnClear" id="Div2">
                                                    <span class="dnnHelpText" id="Span2">Enter domain, try with empty value if you are not sure</span><a href="#" class="pinHelp"></a>
                                                </div>
                                            </div>
                                        </div>
                                        <asp:TextBox ID="DomainTextBox" runat="server" CssClass="form-control"></asp:TextBox>
                                        <asp:RequiredFieldValidator CssClass="dnnFormMessage dnnFormError" ValidationGroup="DoNotCheck" ControlToValidate="DomainTextBox" ID="RequiredFieldValidator5" Display="Dynamic" SetFocusOnError="true" runat="server" ErrorMessage="* Required"></asp:RequiredFieldValidator>
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

<asp:HiddenField ID="ExchangeToDnnClickedHiddenField" Value="false" runat="server" />
<asp:HiddenField ID="DnnToExchangeClickedHiddenField" Value="false" runat="server" />
