<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HolidayList.aspx.cs" Inherits="EmployeeManagement.HolidayList.HolidayList" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1/i18n/jquery.ui.datepicker-ja.min.js"></script>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <link rel="stylesheet" href="~/Content/Site.css">
    <%--カレンダー--%>
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
        function button() {
            $("#MainContent_fileCsv").click();
        }

        function file() {
            var text = document.getElementById('MainContent_txtCsv');
            var file = document.getElementById('MainContent_fileCsv').files[0].name;
            var value = file;
            text.value = value;
        }

    </script>
    <%--CSS--%>
    <style>
        #divEdit, #divGridView, #inputText, #inputButton {
            display: inline-block;
            max-width: 1140px;
        }

        #divNumber, #divSearch, #divUpdate {
            margin-left: 3%;
            font-size: 15px;
        }

        #divSearch, #divUpdate {
            margin-top: 4%;
        }

        .margin-sides {
            margin-left: 2%;
            margin-right: 2%;
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

        html body {
            padding-bottom: 5%;
        }

        .btnPagging {
            border-top: solid;
            border-width: thin;
        }

            .btnPagging a {
                margin: 10px 5px;
                padding: 3px 7px;
                background-color: #9bc2e6;
                color: black;
            }

            .btnPagging td {
                padding-top: 5px;
                padding-bottom: 5px;
            }

            .btnPagging span {
                margin: 10px 5px;
                padding: 3px 7px;
                background-color: #9bc2e6;
                color: white;
            }

        .header {
            border-bottom-style: solid;
            border-width: thin;
            border-color: black;
            height: 25px;
            color: black;
            font-size: 17px;
        }

            .header a {
                color: black
            }

        .none {
            display: none;
        }
    </style>

    <%--タイトル--%>
    <h2>祝日一覧設定</h2>
    <div id="divEdit" style="vertical-align: top; width: 32%;">
        <%--検索画面--%>
        <div
            class="box-card box-cardshadow" style="height: 580px;">
            <%--検索ラベル--%>
            <asp:Label
                runat="server"
                ID="lblSearchHoliday"
                class="title-label"
                Text="検索">
            </asp:Label>
            <%--表示件数ドロップダウンリスト--%>
            <div id="divNumber" style="margin-top: 3%;">
                <asp:DropDownList
                    ID="dropNumber"
                    runat="server"
                    AutoPostBack="true"
                    OnSelectedIndexChanged="DropNumber_SelectedIndexChanged">
                    <asp:ListItem Text="5" Value="5"></asp:ListItem>
                    <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    <asp:ListItem Text="20" Value="20"></asp:ListItem>
                    <asp:ListItem Text="30" Value="30"></asp:ListItem>
                </asp:DropDownList>
                <%--表示ラベル--%>
                <asp:Label
                    ID="lblDisplay"
                    runat="server"
                    Text="件表示">
                </asp:Label>
            </div>
            <%--検索期間ラベル--%>
            <div id="divSearch">
                検索期間
                <asp:Label
                    ID="lblSearchPeriod"
                    runat="server"
                    ForeColor="Red"
                    Text="">
                </asp:Label>
                <p>
                    <%--開始日テキストボックス--%>
                    <asp:TextBox
                        ID="txtDateStart"
                        runat="server"
                        Width="30%">
                    </asp:TextBox>
                    <%--～ラベル--%>
                    <asp:Label
                        CssClass="margin-sides"
                        ID="lblTilde"
                        runat="server"
                        Text="～">
                    </asp:Label>
                    <%--終了日テキストボックス--%>
                    <asp:TextBox
                        ID="txtDateEnd"
                        runat="server"
                        Width="30%">
                    </asp:TextBox>
                </p>
                <%--検索ボタン--%>
                <asp:Button
                    ID="btnSearch"
                    runat="server"
                    Text="検索"
                    CssClass="search"
                    OnClick="BtnSearch_Click" />
            </div>
            <%--アップデートラベル--%>
            <div id="divUpdate">
                アップデート
                <asp:Label
                    ID="lblUpdate"
                    runat="server"
                    Text=""
                    ForeColor="Red">
                </asp:Label>
                <p>
                    <%--更新テキストボックス--%>
                    <input
                        id="txtCsv"
                        type="text"
                        style="width: 80%"
                        onclick="text()"
                        runat="server"
                        readonly="readonly" />
                    <input
                        id="btnFile"
                        type="button"
                        onclick="button()"
                        runat="server"
                        value="参照" />
                    <input
                        id="fileCsv"
                        type="file"
                        onchange="file()"
                        runat="server"
                        style="display: none"
                        accept=".csv" />
                </p>
                <p>
                    <%--更新ボタン--%>
                    <asp:Button
                        ID="btnUpdate"
                        runat="server"
                        Text="アップデート"
                        CssClass="btn-confirm"
                        Width="40%"
                        OnClick="BtnUpdate_Click" />
                </p>
            </div>
        </div>
    </div>
    <%--祝日グリッドビュー--%>
    <div id="divGridView" style="margin-left: 2%; width: 65%;">
        <asp:GridView
            ID="gridViewHoliday"
            runat="server"
            AutoGenerateColumns="false"
            GridLines="None"
            PageSize="20"
            AllowPaging="true"
            OnRowDataBound="GridViewHoliday_RowDataBound"
            OnPageIndexChanging="GridViewHoliday_PageIndexChanging"
            HeaderStyle-CssClass="header"
            DataKeyNames="date"
            OnRowDeleting="GridViewHoliday_RowDeleting"
            Font-Size="15px"
            ShowHeaderWhenEmpty="true"
            Width="100%">
            <RowStyle
                BackColor="#ffffff"
                VerticalAlign="Middle"
                Height="32px" />
            <AlternatingRowStyle
                BackColor="#f2f2f2"
                VerticalAlign="Top" />
            <Columns>
                <asp:BoundField
                    DataField="date"
                    HeaderStyle-CssClass="text-center"
                    HeaderText="日付"
                    HeaderStyle-Width="22%"
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    ItemStyle-VerticalAlign="Middle" />
                <asp:BoundField
                    DataField="name"
                    HeaderText="祝日名称"
                    HeaderStyle-Width="65%"
                    HeaderStyle-HorizontalAlign="Right"
                    ItemStyle-VerticalAlign="Middle" />
                <asp:ButtonField
                    HeaderStyle-HorizontalAlign="Center"
                    ItemStyle-HorizontalAlign="Center"
                    HeaderStyle-CssClass="text-center"
                    ItemStyle-CssClass="text-center"
                    ItemStyle-VerticalAlign="Middle"
                    Text="削除"
                    ButtonType="Button"
                    HeaderText="削除"
                    ItemStyle-Width="75px"
                    ItemStyle-Height="26px"
                    ControlStyle-Height="26px"
                    ControlStyle-Width="75px"
                    HeaderStyle-Width="80px"
                    CommandName="DELETE" />
            </Columns>
            <%--改ページボタン--%>
            <PagerSettings
                Mode="NumericFirstLast"
                Visible="true" />
            <PagerStyle
                HorizontalAlign="Right"
                Font-Size="15px"
                CssClass="btnPagging" />
        </asp:GridView>
    </div>

</asp:Content>
