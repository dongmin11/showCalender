<%@ Page Title="社員一覧" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="EmployeeManagement.Setting.UserList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" href="~/Content/Site.css">
    <style>
        .checkbox {
            float: right;
            margin-right: 5px !important;
            margin-top: 3px !important;
            padding: 0px 0px !important;
        }

            .checkbox input {
                margin-left: 0px !important;
            }

        .PagerStyle td {
            padding: 0.1em 0.4em;
        }

        .PagerStyle a {
            background-color: #449ee0;
            color: #555555;
            font-size: 20px;
            padding: 5px 10px;
        }

            .PagerStyle a:hover {
                color: black;
            }

        .PagerStyle span {
            background-color: #449ee0;
            color: black;
            font-size: 20px;
            padding: 5px 10px;
        }
    </style>
    <h1>社員一覧
        <asp:Button ID="BtnUserList"
            runat="server"
            OnClick="BtnUserList_Click"
            CssClass="btn-confirm "
            Text="新規追加" />
    </h1>
    <div style="height: 35px">
        <asp:DropDownList ID="KennsuList"
            Width="40px"
            OnSelectedIndexChanged="KennsuList_SelectedIndexChanged"
            AutoPostBack="true"
            runat="server">
            <asp:ListItem>5</asp:ListItem>
            <asp:ListItem>10</asp:ListItem>
            <asp:ListItem>15</asp:ListItem>
            <asp:ListItem>20</asp:ListItem>
            <asp:ListItem>30</asp:ListItem>
        </asp:DropDownList>
        <asp:Label ID="LblKensuu"
            runat="server"
            Text="件表示"
            AssociatedControlID="KennsuList"></asp:Label>

        <asp:Label ID="LblSearch"
            runat="server"
            Style="float: right;"
            Text="検索:">
            <asp:TextBox ID="TxtSearch"
                runat="server"
                Width="150px"></asp:TextBox>
            <asp:Button ID="BtnSearch"
                runat="server"
                Text="検索"
                Width="50px"
                OnClick="BtnSearch_Click" />
        </asp:Label>
        <asp:CheckBox ID="CheckBoxDelete"
            Text="削除も表示"
            CssClass="checkbox"
            runat="server" />
    </div>
    <div>
        <asp:GridView ID="GvUseList"
            runat="server"
            CssClass="table"
            GridLines="Horizontal"
            BorderStyle="None"
            BorderColor="Black"
            AllowSorting="True"
            AllowPaging="True"
            AutoGenerateColumns="False"
            PagerStyle-HorizontalAlign="Right"
            DataKeyNames="社員番号"
            Width="100%"
            PageSize="10"
            BorderWidth="1px"
            OnRowCommand="GvUseList_RowCommand"
            OnPageIndexChanging="GvUseList_PageIndexChanging"
            OnSorting="GvUseList_Sorting">
            <AlternatingRowStyle BackColor="#eeeeee" />
            <HeaderStyle
                CssClass="gridHeader"
                ForeColor="Black"
                Font-Size="12pt" />
            <Columns>
                <%--[ユーザーID]列--%>
                <asp:BoundField
                    DataField="ユーザー"
                    SortExpression="ユーザー"
                    HeaderText="ユーザーID"
                    HeaderStyle-Width="250px"></asp:BoundField>
                <%--[社員名称]列--%>
                <asp:BoundField
                    DataField="社員名称"
                    HeaderText="社員名称"
                    SortExpression="社員名称"
                    HeaderStyle-Width="250px" />
                <%--[フリガナ]列--%>
                <asp:BoundField
                    DataField="フリガナ"
                    HeaderText="フリガナ"
                    SortExpression="フリガナ"
                    HeaderStyle-Width="250px"></asp:BoundField>
                <%--[メール]列--%>
                <asp:BoundField
                    DataField="メール"
                    HeaderText="メール"
                    SortExpression="メール"
                    HeaderStyle-Width="250px"></asp:BoundField>
                <%--[部署名]列--%>
                <asp:BoundField
                    DataField="部署名"
                    HeaderText="部署名"
                    SortExpression="部署名"
                    HeaderStyle-Width="250px"></asp:BoundField>
                <%--[権限]列--%>
                <asp:BoundField
                    DataField="権限名"
                    HeaderText="権限"
                    SortExpression="権限名"
                    HeaderStyle-Width="300px"></asp:BoundField>
                <%--[詳細ボタン]列--%>
                <asp:ButtonField
                    HeaderStyle-CssClass="text-center"
                    ButtonType="Button"
                    HeaderText="詳細"
                    Text="詳細"
                    ControlStyle-Width="75"
                    CommandName="btnDetail" />
            </Columns>
            <PagerStyle CssClass="PagerStyle" />
            <PagerSettings Mode="NumericFirstLast" />
        </asp:GridView>
    </div>
</asp:Content>
