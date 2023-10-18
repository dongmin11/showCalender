<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ScheduleEdit.aspx.cs" Inherits="EmployeeManagement.Schedule.ScheduleEdit" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1/i18n/jquery.ui.datepicker-ja.min.js"></script>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <link rel="stylesheet" href="~/Content/Site.css">
    <script>
        $(function () {

            $("[id*=txtDateStart]").datepicker
                ({
                    dateFormat: "yy年mm月dd日(D)"
                });
            $("[id*=txtDateEnd]").datepicker
                ({
                    dateFormat: "yy年mm月dd日(D)"
                });
        });
        function onMyCheckClicked() {
            if ($("[id*=cbSchedule]").is(":checked")) {
                $("[id*=lblDateTilde]").show();
                $("[id*=txtDateEnd]").show();
            } else {
                $("[id*=lblDateTilde]").hide();
                $("[id*=txtDateEnd]").hide();
            }
        }
    </script>
    <style>
        .red {
            color: red;
        }

        .font-Size {
            font-size: 36px;
            vertical-align: -55%;
        }

        .spacing {
            border-spacing: 15px;
        }

        .margin-Right {
            margin-right: 30px;
        }

        .margin-Sides {
            margin-left: 5px;
            margin-right: 5px;
        }

        .margin-Top {
            margin-top: 5px;
            margin-left: 25px;
        }

        .marginleft {
            margin-left: 5px;
        }

        .errormes {
            margin-left: 20px;
        }

        .left {
            margin-left: 25px;
        }

        input {
            width: 50px;
        }

        .none {
            display: none;
        }

        .inline {
            display: inline;
        }

        .memo {
            resize: none;
        }
    </style>
    
    <%--見出し--%>
    <table class="spacing">
        <tr>
            <td>
                <h3><%:this.editTitle %></h3>
            </td>
        </tr>
    </table>
    <%--部分更新--%>
    <%--日付ラベル--%>
    <div class="left">
        <asp:Label
            ID="lblDate"
            runat="server"
            Text="日付"
            CssClass="margin-Right">
        </asp:Label>
        <%--チェックボックス--%>
        <asp:CheckBox
            ID="cbSchedule"
            runat="server"
            OnClick="onMyCheckClicked();"
            Checked="false"
            Text="期間予定"
            CssClass="checkbox" />
        <%--日付エラーメッセージ表示--%>
        <asp:Label
            CssClass="errormes"
            ID="lblError"
            runat="server"
            Text=""
            ForeColor="Red">
        </asp:Label>
    </div>
    <div class="left">
        <asp:TextBox
            ID="txtDateStart"
            runat="server"
            Width="156px"
            >
        </asp:TextBox>
        <asp:Label
            ID="lblDateTilde"
            runat="server"
            Text="～"
            CssClass="margin-Sides">
        </asp:Label>
        <asp:TextBox
            ID="txtDateEnd"
            runat="server"
            Width="155px"
            >
        </asp:TextBox>
    </div>
    <div class="margin-Top">
        <%--時刻ラベル--%>
        時刻<asp:Label
            ID="lblTime"
            runat="server"
            Text=""
            ForeColor="Red">
        </asp:Label>
    </div>
    <div class="left">
        <%--時間数ドロップダウンリスト１--%>
        <asp:DropDownList
            ID="dropStartHour"
            runat="server"
            Width="76"
            Height="26">
            <asp:ListItem>01時</asp:ListItem>
            <asp:ListItem>02時</asp:ListItem>
            <asp:ListItem>03時</asp:ListItem>
            <asp:ListItem>04時</asp:ListItem>
            <asp:ListItem>05時</asp:ListItem>
            <asp:ListItem>06時</asp:ListItem>
            <asp:ListItem>07時</asp:ListItem>
            <asp:ListItem>08時</asp:ListItem>
            <asp:ListItem>09時</asp:ListItem>
            <asp:ListItem>10時</asp:ListItem>
            <asp:ListItem>11時</asp:ListItem>
            <asp:ListItem>12時</asp:ListItem>
            <asp:ListItem>13時</asp:ListItem>
            <asp:ListItem>14時</asp:ListItem>
            <asp:ListItem>15時</asp:ListItem>
            <asp:ListItem>16時</asp:ListItem>
            <asp:ListItem>17時</asp:ListItem>
            <asp:ListItem>18時</asp:ListItem>
            <asp:ListItem>19時</asp:ListItem>
            <asp:ListItem>20時</asp:ListItem>
            <asp:ListItem>21時</asp:ListItem>
            <asp:ListItem>22時</asp:ListItem>
            <asp:ListItem>23時</asp:ListItem>
            <asp:ListItem>24時</asp:ListItem>
        </asp:DropDownList>
        <%--分数ドロップダウンリスト１--%>
        <asp:DropDownList
            ID="dropStertMinute"
            runat="server"
            Width="76"
            Height="26">
            <asp:ListItem>00分</asp:ListItem>
            <asp:ListItem>15分</asp:ListItem>
            <asp:ListItem>30分</asp:ListItem>
            <asp:ListItem>45分</asp:ListItem>
        </asp:DropDownList>
        <%--[～]ラベル--%>
        <asp:Label
            ID="lblTimeTilde"
            runat="server"
            Text="～">
        </asp:Label>
        <%--時間数ドロップダウンリスト２--%>
        <asp:DropDownList
            ID="dropEndHour"
            runat="server"
            Width="76"
            Height="26">
            <asp:ListItem>01時</asp:ListItem>
            <asp:ListItem>02時</asp:ListItem>
            <asp:ListItem>03時</asp:ListItem>
            <asp:ListItem>04時</asp:ListItem>
            <asp:ListItem>05時</asp:ListItem>
            <asp:ListItem>06時</asp:ListItem>
            <asp:ListItem>07時</asp:ListItem>
            <asp:ListItem>08時</asp:ListItem>
            <asp:ListItem>09時</asp:ListItem>
            <asp:ListItem>10時</asp:ListItem>
            <asp:ListItem>11時</asp:ListItem>
            <asp:ListItem>12時</asp:ListItem>
            <asp:ListItem>13時</asp:ListItem>
            <asp:ListItem>14時</asp:ListItem>
            <asp:ListItem>15時</asp:ListItem>
            <asp:ListItem>16時</asp:ListItem>
            <asp:ListItem>17時</asp:ListItem>
            <asp:ListItem>18時</asp:ListItem>
            <asp:ListItem>19時</asp:ListItem>
            <asp:ListItem>20時</asp:ListItem>
            <asp:ListItem>21時</asp:ListItem>
            <asp:ListItem>22時</asp:ListItem>
            <asp:ListItem>23時</asp:ListItem>
            <asp:ListItem>24時</asp:ListItem>
        </asp:DropDownList>
        <%--分数ドロップダウンリスト２--%>
        <asp:DropDownList
            ID="dropEndMinute"
            runat="server"
            Width="76"
            Height="26">
            <asp:ListItem>00分</asp:ListItem>
            <asp:ListItem>15分</asp:ListItem>
            <asp:ListItem>30分</asp:ListItem>
            <asp:ListItem>45分</asp:ListItem>
        </asp:DropDownList>
    </div>
    <%--予定ラベル--%>
    <div class="margin-Top">
        予定<asp:Label
            ID="lblSchedule"
            runat="server"
            ForeColor="Red"
            Text="">
        </asp:Label>
    </div>
    <%--予定ドロップダウンリスト--%>
    <div class="left">
        <asp:DropDownList
            ID="dropSchedule"
            runat="server"
            Width="98"
            Height="26">
        </asp:DropDownList>
        <%--予定テキストボックス--%>
        <asp:TextBox
            ID="txtSchedule"
            runat="server"
            Width="400"
            Height="26"
            MaxLength="25"
            ForeColor="Black">
        </asp:TextBox>
    </div>
    <%--メモ--%>
    <div class="margin-Top">
        <%--メモラベル--%>
        <asp:Label
            ID="lblMemo"
            runat="server"
            Text="メモ">
        </asp:Label>
    </div>
    <div class="left">
        <%--メモテキストボックス--%>
        <asp:TextBox
            CssClass="memo"
            ID="txtMemo"
            runat="server"
            Width="502"
            Height="150"
            MaxLength="255"
            Rows="100"
            TextMode="MultiLine">
        </asp:TextBox>
    </div>
    <%--追加・キャンセルボタン--%>
    <div class="margin-Top">
        <%--追加ボタン--%>
        <asp:Button
            ID="btnAdd"
            runat="server"
            Text="追加"
            class="btn-confirm"
            OnClick="Addbtn_Click"
            OnClientClick="return confirm('この内容でよろしいですか？')" />
        &nbsp;
        <%--キャンセルボタン--%>
        <asp:Button
            ID="btnCancel"
            runat="server"
            Text="キャンセル"
            class="btn-cancel"
            OnClick="btnCancel_Click"/>
    </div>

</asp:Content>
