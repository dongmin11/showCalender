<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepartmentEdit.aspx.cs" Inherits="EmployeeManagement.Setting.DepartmentEdit" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2 ><%:title%></h2>
    <p>
        部署名称
        <asp:Label runat="server" ID="lbDepartmentName" ForeColor="Red"></asp:Label>
        <br />
        <asp:TextBox runat="server" ID="txtDepartmentName" Width="40%"></asp:TextBox>
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
