<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleList.aspx.cs" Inherits="EmployeeManagement.ScheduleList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <style>
        .green{
            background-color: #CBFFD3;
            height: 20px!important;
        }
        .span {
            display: block !important;
            color: black !important;
        }

        .holiday {
            display: block !important;
            background-color: tomato;
            color: black !important;
        }

        .blue {
            background-color: #A2D4FF;
        }

        .orange {
            background-color: #edb078;
        }

        .margin-Left {
            margin-left: 10px;
            table-layout: fixed;
        }

        .float-right {
            float: right;
            width: 50px;
            border-spacing: 10px;
            border-collapse: separate;
        }

        .center {
            padding-left: 25%;
        }

        .margin-TopLeft {
            margin:0 auto;
            table-layout: fixed!important;
        }
        html body{
            padding-bottom: 5%;
        }
    </style>
    <%--見出し--%>
    <div>
        <h3 style="margin-left:3px">スケジュール</h3>
    </div>
    <%--部門ドロップダウンリスト--%>
    <asp:DropDownList
        ID="dropDept"
        runat="server"
        Width="150px"
        Height="21px"
        CssClass="margin-Left"
        DataValueField="部署ＩＤ"
        DataTextField="部署名"
        OnSelectedIndexChanged="DropDept_SelectedIndexChanged"
        AutoPostBack="true">
    </asp:DropDownList>
    <%--本日の日付("yyyy年MM月dd日(ddd)")--%>
    <asp:Label
        ID="lblToday"
        runat="server"
        Text=""
        Font-Size="X-Large"
        CssClass="center">
    </asp:Label>
    <%--相対日付ボタン--%>
    <table class="float-right">
        <tr>
            <td>
                <asp:Button
                    ID="btnLastMonth"
                    runat="server"
                    Text="先月"
                    OnClick="btnLastMonth_Click"/>
            </td>
            <td>
                <asp:Button
                    ID="btnLastWeek"
                    runat="server"
                    Text="先週"
                    OnClick="btnLastWeek_Click"/>
            </td>
            <td>
                <asp:Button
                    ID="btnToday"
                    runat="server"
                    Text="本日"
                    OnClick="btnToday_Click"/>
            </td>
            <td>
                <asp:Button
                    ID="btnNextweek"
                    runat="server"
                    Text="翌週"
                    OnClick="btnNextweek_Click"/>
            </td>
            <td>
                <asp:Button
                    ID="btnNextMonth"
                    runat="server"
                    Text="翌月"
                    OnClick="btnNextMonth_Click"/>
            </td>
        </tr>
    </table>
        <%=this.ScheduleTable %>
</asp:Content>
