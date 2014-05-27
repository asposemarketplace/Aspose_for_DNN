<%--Copyright (c) Aspose 2002-2014. All Rights Reserved.--%>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Modules.Aspose.DNN.WordImport.View" %>

<div id="div_main">
    <br />
    <asp:Literal ID="OutputLiteral" runat="server"></asp:Literal>
    <br />    
    <asp:FileUpload ID="ImportFileUpload" runat="server" />
    <br />
    <br />
    <asp:Button ID="ImportButton" runat="server" Text="Import from Word" OnClick="ImportButton_Click" />
    <br />
</div>
