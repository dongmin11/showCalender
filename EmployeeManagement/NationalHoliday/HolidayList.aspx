<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HolidayList.aspx.cs" Inherits="EmployeeManagement.HolidayList.HolidayList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1/i18n/jquery.ui.datepicker-ja.min.js"></script>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <link rel="stylesheet" href="~/Content/Site.css">
    <script>
        $(function () {

            $("[id*=txtDateStart]").datepicker
                ({
                    dateFormat: "yy/mm/dd"
                });
            $("[id*=txtDateEnd]").datepicker
                ({
                    dateFormat: "yy/mm/dd"
                });
        });
    </script>
    <%--CSS--%>
    <style>
         #div1,#div2{
            display:inline-block;
            width:auto
        }
        .margin-TopLeft {
            margin-top: 3%;
            margin-left: 3%;
        }
        .margin-Left {
            margin-left: 3%;
        }
        .search {
            color: white;
            background-color: #0066FF;
            border-color: #0066FF;
            width: 90px;
            height: 30px;
            font-size: 15px;
            margin-right: 15px;
        }
        html body{
            padding-bottom: 5%;
        }
    </style>
   
    <%--タイトル--%>
    <h2>祝日一覧設定</h2>
    <div id="div1" style="vertical-align:top;">
         <%--検索画面--%>
    <div
        class="box-card box-cardshadow">
        <%--検索ラベル--%>
        <asp:Label
            runat="server"
            ID="lblSearchHoliday"
            class="title-label"
            Text="検索">
        </asp:Label>
    </div>
   
       
    </div>
    <%--祝日グリッドビュー--%>
    <div id="div2" style="margin-left:100px;">
         <asp:GridView
        ID="gridViewHoliday"
        runat="server"
        AutoGenerateColumns="false">
        <AlternatingRowStyle BackColor="#f2f2f2" VerticalAlign="Top" />
        <Columns>
            <asp:BoundField DataField="date"/>
            <asp:BoundField DataField="name"/>
        </Columns>
    </asp:GridView>
    </div>
   
    
</asp:Content>
