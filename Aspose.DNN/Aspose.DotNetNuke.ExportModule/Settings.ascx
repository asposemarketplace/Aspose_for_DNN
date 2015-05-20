﻿<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Settings.ascx.cs" Inherits="Aspose.Modules.DotNetNukeContentExport.Settings" %>
<%@ Register TagName="label" TagPrefix="dnn" Src="~/controls/labelcontrol.ascx" %>
<h2 id="dnnSitePanel-BasicSettings" class="dnnFormSectionHead">
    <a href="" class="dnnSectionExpanded">
        <%=LocalizeString("BasicSettings")%></a></h2>
<fieldset>
    <div class="dnnFormItem">
        <dnn:label ID="WordsExportButtonCssClassLabel" runat="server" />
        <asp:TextBox ID="WordsExportButtonCssClassTextBox" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="PdfExportButtonCssClassLabel" runat="server" />
        <asp:TextBox ID="PdfExportButtonCssClassTextBox" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="PaneSelectionDropDownCssClassLabel" runat="server" />
        <asp:TextBox ID="PaneSelectionDropDownCssClassTextBox" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="DefaultPaneLabel" runat="server" />
        <asp:DropDownList ID="PanesDropDownList" CssClass="panesDropDown" runat="server">
        </asp:DropDownList>
        <asp:TextBox ID="DefaultPaneTextBox" Visible="false" runat="server" />
    </div>
    <div class="dnnFormItem">
        <dnn:label ID="HideDefaultPaneLabel" runat="server" />
        <asp:CheckBox ID="HideDefaultPaneCheckBox" runat="server" />
    </div>
</fieldset>
