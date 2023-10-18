<%@ Page Title="設定" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Setting.aspx.cs" Inherits="EmployeeManagement.Setting.Setting" %>
<asp:Content ID="Content1"  ContentPlaceHolderID="MainContent" runat="server">
    
    <div style="height:600px">
        <h2>設定</h2>
        <p>
            <a style="font-size:x-large" href="UserList.aspx"> ・社員一覧</a>
        </p>
        <p>
            <a style="font-size:x-large" href="DepartmentList.aspx" > ・部署一覧</a>
        </p>
        <p>
            <a style="font-size:x-large" href="PapersList.aspx"> ・申請書マスタ</a>
        </p>
        <p>
            <a style="font-size:x-large" href="HolidayList.aspx"> ・祝日一覧</a>
        </p>
    </div>   

    
</asp:Content>
