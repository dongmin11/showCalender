<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PapersEdit.aspx.cs" Inherits="EmployeeManagement.Setting.PapersEdit" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2><%:title%></h2>
    <p>
        書類番号
        <asp:Label runat="server" ID="lbPaperId" ForeColor="Red"></asp:Label>
        <br />
        <asp:TextBox runat="server" ID="txtPaperID"  Width="40%" ToolTip="半角数字を入力してください"></asp:TextBox>
    </p>
    <p>
        書類名称
        <asp:Label runat="server" ID="lbPaperName" ForeColor="Red"></asp:Label>
        <br />
        <asp:TextBox runat="server" ID="txtPaperName" Width="40%"></asp:TextBox>
    </p>
    <asp:Button ID="btnEdit"
        runat="server"
        CssClass="btn-confirm"
        OnClientClick="return confirm('この内容でよろしいですか？')"
        OnClick="btnEdit_Click"
        Text="追加" />
    <asp:Button ID="btnCancel"
        runat="server"
        CssClass="btn-cancel"
        OnClientClick="return confirm('キャンセルしてもよろしいですか？')"
        OnClick="btnCancel_Click"
        Text="キャンセル" />
    <asp:Button ID="btnDelete"
        runat="server"
        CssClass=" btn-delete"
        OnClientClick="return confirm('本当に削除してもよろしいですか？')"
        Text="削除" 
        OnClick="btnDelete_Click"
        Visible="false" />
    
</asp:Content>
