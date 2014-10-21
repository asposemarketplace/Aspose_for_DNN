<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="DnnToGmailSync.ascx.cs" Inherits="Aspose.DNN.GmailSync.DnnToGmailSync" %>

<hr />

<h2>DNN to Gmail Sync</h2>
<br />
<div id="Into_Div" runat="server" class="into-text">
    DNN to Gmail Sync allows you to Sync contacts between DNN and Gmail Server. Please click on 'Fetch DNN Users' button below to get started
    <br />
    <br />
    <asp:Button ID="GetDNNUsersButton" CssClass="dnnSecondaryAction" runat="server" ValidationGroup="DoNotCheck2" Text="Fetch DNN Users" OnClick="GetDNNUsersButton_Click" />
    <br />
</div>

<div class="dnnFormMessage dnnFormInfo" runat="server" visible="false" id="NoRowSelectedErrorDiv">
    Please select one or more users to continue
</div>

<div id="DnnToGmail_MainDiv" runat="server" visible="false">
    <div class="stepHeading">
        <img alt="Site Wizard" src="/Icons/Sigma/BreadcrumbArrows_16x16_Gray.png" style="border-width: 0px;">
        Step 1: Select one or more users to Import them to Gmail server
    </div>
    <table style="width: 100%;">
        <tr>
            <td style="width: 60%; vertical-align: top;">
                <div style="max-height: 300px; overflow: auto;">
                    <asp:GridView ID="DNNUsersGridView" EmptyDataText="There are no contacts." Width="100%" EmptyDataRowStyle-CssClass="emptyClass"
                        GridLines="None" BorderWidth="0" AutoGenerateColumns="false" HeaderStyle-CssClass="rgHeader"
                        CssClass="rgMasterTable" DataKeyNames="UserId" ClientIDMode="Static" runat="server">
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="rgHeader" HeaderStyle-Width="20px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="SelectAllCheckBox" CssClass="selectAllCheckBox" runat="server" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="SelectedCheckBox" CssClass="selectableCheckBox" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DisplayName" HeaderStyle-CssClass="rgHeader" HeaderText="Display Name" />
                            <asp:BoundField DataField="Email" HeaderStyle-CssClass="rgHeader" HeaderText="Email" />
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
            <td style="width: 30px;"></td>
            <td><b>DNN to Gmail field mappings</b>
                <table class="rgMasterTable">
                    <tr>
                        <th class="rgHeader">DNN</th>
                        <th class="rgHeader">Gmail</th>
                    </tr>
                    <tr>
                        <td>Email</td>
                        <td>Work E-mail</td>
                    </tr>
                    <tr>
                        <td>DisplayName</td>
                        <td>DisplayName</td>
                    </tr>
                    <tr>
                        <td>First name</td>
                        <td>Given Name</td>
                    </tr>
                    <tr>
                        <td>Middle name</td>
                        <td>Middle Name</td>
                    </tr>
                    <tr>
                        <td>Last name</td>
                        <td>Surname</td>
                    </tr>
                    <tr>
                        <td>Telephone</td>
                        <td>PhoneNumbers.Work</td>
                    </tr>
                    <tr>
                        <td>Cell/Mobile</td>
                        <td>PhoneNumbers.Mobile</td>
                    </tr>
                    <tr>
                        <td>Website</td>
                        <td>BusinessHomePage</td>
                    </tr>
                    <tr>
                        <td>IM</td>
                        <td>Skype/Google Talk</td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top;">
                <br />
                <table class="rgMasterTable">
                    <tr>
                        <th class="rgHeader">DNN</th>
                        <th class="rgHeader">Gmail</th>
                    </tr>

                    <tr>
                        <td>Street</td>
                        <td>Street</td>
                    </tr>
                    <tr>
                        <td>City</td>
                        <td>City</td>
                    </tr>
                    <tr>
                        <td>Region</td>
                        <td>State/Province</td>
                    </tr>
                    <tr>
                        <td>Postal code</td>
                        <td>Postal Code</td>
                    </tr>
                    <tr>
                        <td>Country</td>
                        <td>Country/Region</td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>

    <br />
    <br />

    <div class="stepHeading">
        <img alt="Site Wizard" src="/Icons/Sigma/BreadcrumbArrows_16x16_Gray.png" style="border-width: 0px;">
        Step 2: Click 'DNN To Gmail Sync' button below
    </div>
    <table>
        <tr>
            <td></td>
            <td style="width: 40px;"></td>
            <td>
                <asp:Button ID="DNNToGmailSyncButton" CssClass="dnnPrimaryAction" ValidationGroup="DoNotCheck3" OnClick="DNNToGmailSyncButton_Click" runat="server" Text="DNN To Gmail Sync"></asp:Button>
            </td>
        </tr>
    </table>

</div>


<div id="ProcessSummaryDiv" runat="server" visible="false">
    <asp:Literal ID="ImportedLiteral" runat="server"></asp:Literal>
    <br />
    <br />
    <asp:Literal ID="AlreadyExistingLiteral" runat="server"></asp:Literal>
</div>

