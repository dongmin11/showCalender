<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocumentList.aspx.cs" Inherits="EmployeeManagement.Setting.DocumentList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #h2 input[type=submit]{
            vertical-align:top;
            margin-left:50px
        }
        
    </style>
   <h2 id="h2" >書類申請一覧<asp:Button runat="server" ID="btnAdd"　CssClass="btn-confirm"  Text="新規追加" /></h2> 
    <p>
        <asp:DropDownList runat="server" ID="dropNumber" AutoPostBack="true">
            <asp:ListItem Text="5件" Value="5" ></asp:ListItem>
            <asp:ListItem Text="10件" Value="10" ></asp:ListItem>
            <asp:ListItem Text="20件" Value="20" ></asp:ListItem>
            <asp:ListItem Text="30件" Value="30" ></asp:ListItem>
        </asp:DropDownList>件表示
        
        <list style="float:right;" >ステータス：<asp:DropDownList runat="server" ID="dropStatus" AutoPostBack="true"></asp:DropDownList></list>
    </p>
    <asp:GridView runat="server" ID="gvDcList"
     AutoGenerateColumns="false">
        <Columns>
            <asp:BoundField HeaderText="申請日" HeaderStyle-Width="16%" />
            <asp:BoundField HeaderText="申請者" HeaderStyle-Width="16%" />
            <asp:BoundField HeaderText="書類名称" HeaderStyle-Width="16%" />
            <asp:BoundField HeaderText="受け取る予定日" HeaderStyle-Width="16%" />
            <asp:BoundField HeaderText="承認者" HeaderStyle-Width="16%" />
            <asp:BoundField HeaderText="ステータス" HeaderStyle-Width="10%" />
            <asp:ButtonField HeaderText="詳細" HeaderStyle-Width="6%" />
        </Columns>
    </asp:GridView>
    
</asp:Content>
