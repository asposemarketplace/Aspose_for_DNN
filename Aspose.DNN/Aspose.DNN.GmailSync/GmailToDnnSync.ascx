<%@ Control Language="C#" AutoEventWireup="True" CodeBehind="GmailToDnnSync.ascx.cs" Inherits="Aspose.DNN.GmailSync.GmailToDnnSync" %>
<hr />

<h2>Gmail to DNN Sync</h2>
<br />
<div id="Into_Div" runat="server" class="into-text">
    Gmail to DNN Sync allows you to Sync contacts between Gmail Server and DNN. Please click on 'Fetch Gmail Contacts' button below to get started
    <br />
    <br />
    <asp:Button ID="GetGmailContactsButton" CssClass="dnnSecondaryAction" runat="server" ValidationGroup="DoNotCheck2" Text="Fetch Gmail Contacts" OnClick="GetGmailContactsButton_Click" />
    <br />
</div>
<div class="dnnFormMessage dnnFormInfo" runat="server" visible="false" id="NoRowSelectedErrorDiv">
    Please select one or more contacts to continue
</div>
<div id="GmailToDnn_MainDiv" runat="server" visible="false">
    <div class="stepHeading">
        <img alt="Site Wizard" src="/Icons/Sigma/BreadcrumbArrows_16x16_Gray.png" style="border-width: 0px;">
        Step 1: Select one or more contacts to Import them to DNN
    </div>
    <table style="width: 100%;">
        <tr>
            <td style="width: 60%; vertical-align: top;">
                <div style="max-height: 300px; overflow: auto;">
                    <asp:GridView ID="GmailContactsGridView" EmptyDataText="There are no contacts." Width="100%" EmptyDataRowStyle-CssClass="emptyClass"
                        GridLines="None" BorderWidth="0" AutoGenerateColumns="false" HeaderStyle-CssClass="rgHeader"
                        CssClass="rgMasterTable" DataKeyNames="Email" ClientIDMode="Static" runat="server">
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
            <td><b>Gmail to DNN field mappings</b>
                <table class="rgMasterTable">
                    <tr>
                        <th class="rgHeader">Gmail</th>
                        <th class="rgHeader">DNN</th>
                    </tr>
                    <tr>
                        <td>First Email</td>
                        <td>Email Address</td>
                    </tr>
                    <tr>
                        <td>DisplayName</td>
                        <td>DisplayName</td>
                    </tr>
                    <tr>
                        <td>Given Name</td>
                        <td>First Name</td>
                    </tr>
                    <tr>
                        <td>Middle name</td>
                        <td>Middle Name</td>
                    </tr>
                    <tr>
                        <td>Surname</td>
                        <td>Last Name</td>
                    </tr>
                    <tr>
                        <td>Business phone</td>
                        <td>Telephone</td>
                    </tr>
                    <tr>
                        <td>Mobile phone</td>
                        <td>Cell/Mobile</td>
                    </tr>
                    <tr>
                        <td>BusinessHomePage</td>
                        <td>Website</td>
                    </tr>
                    <tr>
                        <td>Skype</td>
                        <td>IM</td>
                    </tr>
                </table>
            </td>
            <td style="vertical-align: top;">
                <br />
                <table class="rgMasterTable">
                    <tr>
                        <th class="rgHeader">Gmail</th>
                        <th class="rgHeader">DNN</th>
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
                        <td>State/Province</td>
                        <td>Region</td>
                    </tr>
                    <tr>
                        <td>Postal code</td>
                        <td>Postal Code</td>
                    </tr>
                    <tr>
                        <td>Country/Region</td>
                        <td>Country</td>
                    </tr>
                </table>

            </td>

        </tr>

    </table>

    <br />
    <br />

    <div class="stepHeading">
        <img alt="Site Wizard" src="/Icons/Sigma/BreadcrumbArrows_16x16_Gray.png" style="border-width: 0px;">
        Step 2: Select Role(s) and click 'Gmail To DNN Sync' button below
    </div>
    <table>
        <tr>
            <td>
                <asp:GridView ID="RolesGridView" EmptyDataText="There are no roles." EmptyDataRowStyle-CssClass="emptyClass"
                    GridLines="None" BorderWidth="0" AutoGenerateColumns="false" ShowHeader="false" CssClass="rgMasterTable"
                    DataKeyNames="RoleID" ClientIDMode="Static" runat="server">
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="SelectAllCheckBox" runat="server" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="SelectedCheckBox" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="RoleName" HeaderText="RoleName" />
                    </Columns>
                </asp:GridView>
            </td>
            <td style="width: 40px;"></td>
            <td>
                <asp:Button ID="GmailToDNNSyncButton" CssClass="dnnPrimaryAction" ValidationGroup="DoNotCheck3" OnClick="GmailToDNNSyncButton_Click" runat="server" Text="Gmail To DNN Sync"></asp:Button>
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

