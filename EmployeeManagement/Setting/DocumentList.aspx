<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocumentList.aspx.cs" Inherits="EmployeeManagement.Setting.DocumentList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #h2 input[type=submit]{
            vertical-align:top;
            margin-left:50px
        }
        /*.position{
            float:right;
        }*/
    </style>
   <h2 id="h2" >書類申請一覧<asp:Button runat="server" ID="btnAdd"　CssClass="btn-confirm"  Text="新規追加" /></h2> 
    <p>
        <asp:DropDownList runat="server" ID="dropNumber"></asp:DropDownList>件表示
        <list style="float:right;" >ステータス：<asp:DropDownList runat="server" ID="dropStatus" ></asp:DropDownList></list>
    </p>
    <asp:GridView runat="server" ID="gvDcList"
     AutoGenerateColumns="false">
        ＜
    </asp:GridView>
    
</asp:Content>
