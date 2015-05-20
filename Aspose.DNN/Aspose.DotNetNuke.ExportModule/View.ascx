<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="View.ascx.cs" Inherits="Aspose.Modules.AsposeDotNetNukeContentExport.View" %>

<script type="text/javascript">

    String.prototype.replaceAll = function (find, replace) {
        var str = this;
        return str.replace(new RegExp(find, 'g'), replace);
    };

    function ButtonClicked() {
        var PanesDropDownList = document.getElementById("<%=PanesDropDownList.ClientID%>");
        var selectedDropDownValue = PanesDropDownList.options[PanesDropDownList.selectedIndex].value;

        if (selectedDropDownValue == "dnn_full_page") {
            document.getElementById("PageSourceHiddenField").value = document.body.innerHTML.replaceAll("<", "#l#").replaceAll(">", "#g#");
        }
        else {
            document.getElementById("PageSourceHiddenField").value = document.getElementById(selectedDropDownValue).innerHTML.replaceAll("<", "#l#").replaceAll(">", "#g#");
        }
        return true;
    }
</script>


<div class="exportButton">
    <asp:HiddenField ID="PageSourceHiddenField" ClientIDMode="Static" runat="server" />
    <asp:DropDownList ID="PanesDropDownList" CssClass="panesDropDown" runat="server"></asp:DropDownList>
    &nbsp;&nbsp;&nbsp;
    <asp:Button ID="WordsExportButton" OnClientClick="return ButtonClicked();" ResourceKey="WordsExportButton" runat="server" Text="Export to Word" OnClick="WordsExportButton_Click" />
    &nbsp;&nbsp;&nbsp;
    <asp:Button ID="PdfExportButton" OnClientClick="return ButtonClicked();" runat="server" ResourceKey="PdfExportButton" Text="Export to PDF" OnClick="PdfExportButton_Click" />
</div>

<div style="clear: both"></div>
