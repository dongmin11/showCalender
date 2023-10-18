<%@ Page Title="社員追加・編集" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="EmployeeManagement.Setting.UserEdit" MaintainScrollPositionOnPostback="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1/i18n/jquery.ui.datepicker-ja.min.js"></script>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <link rel="stylesheet" href="~/Content/Site.css">
    <script>
        $(function () {
            $("[id*=TxtEntryDate]").datepicker({
                dateFormat: "yy/mm/dd"
                , maxDate: 0
            });
            $("[id*=TxtBirthDay]").datepicker({
                dateFormat: "yy/mm/dd"
                , maxDate: 0
            });
            $("[id*=TxtVisitDate]").datepicker({
                dateFormat: "yy/mm/dd"
                , maxDate: 0
            });
            $("[id*=TxtGraduation]").datepicker({
                dateFormat: "yy/mm/dd"
                , maxDate: 0
            });

        });
    </script>
    <style>
        html body {
            padding-bottom: 5%;
        }

        textarea {
            resize: none;
        }

        .areaInfoSizing {
            width: 45%;
            height: 900px;
            border: groove;
            display: inline-block;
            vertical-align: top;
            margin-left: 2%;
            margin-right: 2%;
        }

            .areaInfoSizing p {
                padding-left: 5%;
                padding-right: 5%;
            }

        .areaBukaSizing {
            height: 300px;
            display: inline-block;
            vertical-align: top;
            margin-left: 2%;
            margin-right: 2%;
        }

            .areaBukaSizing p {
                padding-left: 5%;
            }

        .textBox {
            max-width: 500px;
            width: 100%;
        }

        .header {
            font-size: 25px;
            background-color: #e5e3e3;
            background-size: 20px;
        }

        .gvBuka {
            border-style: none;
            border-width: 1px;
            border-color: black;
            width: 100%;
            height: 100%;
            max-width: 100%;
            word-break: break-word;
        }

        .gvHeader {
            min-width: 100px;
        }
    </style>

    <h1><%:this.editTitle %></h1>
    <div>
        <div class="areaInfoSizing box-card box-cardshadow">
            <label id="LblKihonInfo"
                class="title-label">
                基本情報</label>
            <p>
                ユーザー
                <asp:Label ID="LblUser"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox ID="TxtUser"
                    runat="server"
                    Text=""
                    MaxLength="35"
                    CssClass="textBox"
                    ToolTip="入力必須項目です"></asp:TextBox>
            </p>
            <p>
                パスワード
                <asp:Label ID="LblPwd"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox ID="TxtPwd"
                    runat="server"
                    Text=""
                    CssClass="textBox"
                    MaxLength="25"
                    TextMode="Password"
                    ToolTip="入力必須項目です"></asp:TextBox>
            </p>
            <p>
                社員名称
                <asp:Label ID="LblUserName"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox ID="TxtUserName"
                    runat="server"
                    Text=""
                    MaxLength="25"
                    CssClass="textBox"
                    ToolTip="入力必須項目です"></asp:TextBox>
            </p>
            <p>
                フリガナ
                <asp:Label ID="LblKana"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox ID="TxtKana"
                    runat="server"
                    Text=""
                    MaxLength="25"
                    CssClass="textBox"
                    ToolTip="入力必須項目です"></asp:TextBox>
            </p>
            <p>
                社員名称表示
                <asp:Label ID="LblNickName"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox ID="TxtNickName"
                    runat="server"
                    Text=""
                    CssClass="textBox"
                    MaxLength="25"
                    ToolTip="入力必須項目です"></asp:TextBox>
            </p>
            <p>
                メール
                <asp:Label ID="LblMail"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox ID="TxtMail"
                    runat="server"
                    Text=""
                    MaxLength="35"
                    CssClass="textBox"
                    ToolTip="入力必須項目です"></asp:TextBox>
            </p>
            <p>
                会社番号
                <asp:Label ID="LblCompanyNum"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox ID="TxtCompanyNum"
                    runat="server"
                    Text=""
                    CssClass="textBox"></asp:TextBox>
            </p>
            <p>
                社名
                <asp:TextBox ID="TxtCompany"
                    runat="server"
                    Text=""
                    MaxLength="25"
                    CssClass="textBox"></asp:TextBox>
            </p>
            <p>
                所属
                <asp:DropDownList ID="DdlDepart"
                    runat="server"
                    DataTextField="部署名"
                    DataValueField="部署ＩＤ"
                    CssClass="textBox">
                </asp:DropDownList>
            </p>
            <p>
                入社日
                <asp:Label ID="LblEntryDate"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox ID="TxtEntryDate"
                    runat="server"
                    MaxLength="10"
                    Text=""
                    CssClass="textBox"></asp:TextBox>
            </p>
            <p>
                性別
                <asp:DropDownList ID="DdlSex"
                    runat="server"
                    DataTextField="性別"
                    CssClass="textBox">
                </asp:DropDownList>
            </p>
            <p>
                生年月日
                <asp:Label ID="LblBirthDay"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox ID="TxtBirthDay"
                    runat="server"
                    Text=""
                    MaxLength="10"
                    CssClass="textBox"></asp:TextBox>
            </p>
            <p>
                履歴書ファイル
                <%--<asp:FileUpload ID="PersonalHistory" runat="server" />--%>
                <asp:TextBox ID="TxtPersonalHistory"
                    runat="server"
                    MaxLength="500"
                    CssClass="textBox"></asp:TextBox>
            </p>
            <p>
                権限
                <asp:DropDownList ID="DdlAuthority"
                    runat="server"
                    DataTextField="権限名"
                    CssClass="textBox">
                </asp:DropDownList>
            </p>
            <p>
                社員フラグ
                <asp:DropDownList ID="DdlEmployeeFlag"
                    runat="server"
                    CssClass="textBox">
                </asp:DropDownList>
            </p>
        </div>
        <div class="areaInfoSizing box-card box-cardshadow">
            <label id="LblPersonalInfo"
                class="title-label">
                個人情報</label>
            <p>
                電話
                <asp:Label ID="LblTelNum"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox ID="TxtTelNum"
                    runat="server"
                    CssClass="textBox"
                    MaxLength="15"
                    ToolTip="-を含めて入力してください"></asp:TextBox>
            </p>
            <p>
                国籍
                <asp:DropDownList ID="DdlNational"
                    DataTextField="名称"
                    DataValueField="SEQ"
                    runat="server"
                    CssClass="textBox">
                </asp:DropDownList>
            </p>
            <p>
                来日年月日
                <asp:Label ID="LblVisitDate"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <asp:TextBox ID="TxtVisitDate"
                    runat="server"
                    MaxLength="10"
                    CssClass="textBox"></asp:TextBox>
            </p>
            <p>
                学歴
                <asp:DropDownList ID="DdlStudies"
                    DataTextField="名称"
                    DataValueField="SEQ"
                    runat="server"
                    CssClass="textBox">
                </asp:DropDownList>
            </p>
            <p>
                学校名
                <asp:TextBox ID="TxtSchool"
                    runat="server"
                    MaxLength="25"
                    CssClass="textBox"></asp:TextBox>
            </p>
            <p>
                専攻学科
                <asp:TextBox ID="TxtMajor"
                    runat="server"
                    MaxLength="25"
                    CssClass="textBox"></asp:TextBox>
            </p>
            <p>
                学位
                <asp:DropDownList ID="DdlDegree"
                    DataTextField="名称"
                    DataValueField="SEQ"
                    runat="server"
                    CssClass="textBox">
                </asp:DropDownList>
            </p>
            <p>
                卒業年月日
                <asp:Label ID="LblGraduation"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox ID="TxtGraduation"
                    MaxLength="10"
                    runat="server"
                    CssClass="textBox"></asp:TextBox>
            </p>
            <p>
                郵便番号
                <asp:Label ID="LblPostal"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <br />
                <asp:TextBox ID="TxtPostal"
                    runat="server"
                    MaxLength="8"
                    CssClass="textBox"
                    ToolTip="-を含めて入力してください"></asp:TextBox>
            </p>
            <p>
                住所
                <asp:TextBox ID="TxtAddress"
                    runat="server"
                    MaxLength="50"
                    CssClass="textBox"></asp:TextBox>
            </p>
            <p>
                最寄り駅
                <asp:TextBox ID="TxtNearby"
                    runat="server"
                    MaxLength="25"
                    CssClass="textBox"></asp:TextBox>
            </p>
            <p>
                備考
                <asp:TextBox ID="TxtNote"
                    runat="server"
                    Height="100px"
                    MaxLength="1000"
                    CssClass="textBox"
                    TextMode="MultiLine"></asp:TextBox>
            </p>
        </div>
    </div>
    <asp:UpdatePanel runat="server">
        <ContentTemplate>
            <div>
                <h2 style="padding-top: 2%">部下一覧</h2>
                <div class="areaBukaSizing box-card box-cardshadow"
                    style="width: 20%;">
                    <label id="LblEmploList"
                        class="title-label">
                        社員一覧</label>
                    <p>
                        <asp:DropDownList ID="DdlBukaDepart"
                            Width="80%"
                            runat="server"
                            DataTextField="部署名"
                            DataValueField="部署ＩＤ"
                            OnSelectedIndexChanged="DdlBukaDepart_SelectedIndexChanged"
                            AutoPostBack="True">
                        </asp:DropDownList>
                    </p>
                    <p>
                        <asp:ListBox ID="ListBuka"
                            runat="server"
                            Width="80%"
                            Height="200px"
                            DataTextField="社員名称"
                            DataValueField="社員番号"
                            OnSelectedIndexChanged="ListBuka_SelectedIndexChanged"
                            AutoPostBack="true"></asp:ListBox>
                    </p>
                </div>
                <div class="areaBukaSizing box-card box-cardshadow"
                    style="width: 70%;">
                    <label id="LblBukaInfo"
                        class=" title-label">
                        部下情報</label>
                    <p>
                        <div style="overflow: auto scroll; max-height: 240px; margin: 0 2% 2% 2%; padding-bottom: 2%;">
                            <asp:GridView ID="GvBUkaInfo"
                                CssClass="gvBuka"
                                GridLines="Horizontal"
                                DataKeyNames="社員番号"
                                OnRowDeleting="GvBUkaInfo_RowDeleting"
                                AutoGenerateColumns="false"
                                runat="server">
                                <AlternatingRowStyle BackColor="#eeeeee"></AlternatingRowStyle>
                                <Columns>
                                    <asp:BoundField HeaderText="社員名"
                                        ItemStyle-CssClass="gvHeader"
                                        DataField="社員名称" />
                                    <asp:BoundField HeaderText="メール"
                                        ItemStyle-CssClass="gvHeader"
                                        DataField="メール" />
                                    <asp:BoundField HeaderText="所属"
                                        ItemStyle-CssClass="gvHeader"
                                        DataField="部署名" />
                                    <asp:ButtonField HeaderText="削除"
                                        HeaderStyle-CssClass="text-center"
                                        ItemStyle-CssClass="text-center"
                                        Text="削除"
                                        ButtonType="Button"
                                        CommandName="DELETE"
                                        HeaderStyle-Width="80px"
                                        ItemStyle-Width="80px" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </p>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="margin-top: 3%; margin-left: 2%;">
        <asp:Button ID="BtnUserEdit"
            runat="server"
            Text="追加"
            CssClass="btn-confirm"
            OnClientClick="return confirm('この内容でよろしいですか？')"
            OnClick="BtnUserEdit_Click" />
        <asp:Button ID="BtnCancel"
            runat="server"
            Text="キャンセル"
            OnClientClick="return confirm('キャンセルしてもよろしいですか？')"
            OnClick="BtnCancel_Click"
            CssClass="btn-cancel" />
    </div>
</asp:Content>
