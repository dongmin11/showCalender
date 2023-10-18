<%@ Page Title="書類申請" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocumentApply.aspx.cs" Inherits="EmployeeManagement.Document.DocumentApply" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="https://code.jquery.com/jquery-3.5.1.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/jquery-ui.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jqueryui/1/i18n/jquery.ui.datepicker-ja.min.js"></script>
    <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/jqueryui/1.12.1/themes/smoothness/jquery-ui.css">
    <style>
        #divApply,#divAuthorize{
            display:inline-block;
            width:45%;
            min-width:400px;
            vertical-align: top;
            margin-left:3.3%;
            height:500px;
        }
        
        #MainContent_ckl1 input[type=checkbox]{
            margin-right:5px;
        }
        #MainContent_cklDocument label{
            font-weight:100;
            vertical-align:middle
        }
        #MainContent_txtCmt1,#MainContent_txtCmt2{
            resize:none;
        }
        .notclickable
        {
            pointer-events:none;
        }
        
    </style>
    <script>
        $(function () {
            $("[id*=txtDate]").datepicker({
                dateFormat: "yy/mm/dd"
                , changeMonth: true
                , changeYear: true
                ,minDate:0
            });
        });
    </script>
    <h2>書類申請</h2>
    <div id="divApply"   class="box-card" >
      <label class="title-label">　申請資料</label>
         <div  style="margin-left:20px" >
        <p>
            <asp:Label runat="server" Text="申請者" Font-Size="15px"></asp:Label>
            <br />
            <asp:DropDownList Width="60%" runat="server" ID="dropUser"  DataTextField="社員名称" DataValueField="社員番号"></asp:DropDownList>
            <asp:TextBox Width="60%"  runat="server" ID="txtUser" ReadOnly="true" Visible="false"></asp:TextBox>
        </p>
         <div style="margin-top:20px;margin-bottom:20px">
            <asp:Label runat="server" Text="申請資料" Font-Size="15px"></asp:Label>
             <asp:Label runat="server" ID="lbDocument" Text="" ForeColor="Red"></asp:Label>
               <br />
             <div style="max-height:120px;overflow-y:auto;width:60%">
            <asp:CheckBoxList runat="server" ID="cklDocument" DataTextField="名称" DataValueField="SEQ" ></asp:CheckBoxList>
               </div>
         </div>
             <p>
                 <asp:Label runat="server" Text="受取予定日" Font-Size="15px" ></asp:Label>
                 <asp:Label runat="server" ID="lbDate" ForeColor="Red" Text=""></asp:Label>
                 <br />
                 <asp:TextBox MaxLength="10" runat="server" ID="txtDate" Width="60%"  PlaceHolder="選んでください" ></asp:TextBox>
             </p>
             <p>
                 <asp:Label runat="server" Text="コメント" Font-Size="15px"></asp:Label>
                 
                 <br />
                 <asp:TextBox runat="server" ID="txtCmt1" Width="60%" Height="120px" TextMode="MultiLine" MaxLength="100" ></asp:TextBox>
             </p>
        </div>
    </div>
    <div id="divAuthorize" class="box-card"  >
      <label class="title-label">　承認情報</label>
        <div style="margin-left:20px">
            <p>
             <asp:Label runat="server" Text="承認者" Font-Size="15px"></asp:Label>
             <asp:Label runat="server" ID="lbAuthorizer" ForeColor="Red" Text=""></asp:Label>
            <br />
            <asp:DropDownList Width="60%" runat="server" ID="dropAuthorizer"  DataTextField="社員名称" DataValueField="社員番号"></asp:DropDownList>
            <asp:TextBox runat="server" ID="txtAuthorizer" ReadOnly="true" Visible="false" Width="60%"></asp:TextBox>
            </p>
            <p style="margin-top:30px;margin-bottom:30px">
             <asp:Label runat="server" Text="ステータス" Font-Size="15px"></asp:Label>
             <br />
            <asp:DropDownList Width="60%" runat="server" ID="dropStatus" ></asp:DropDownList>
            <asp:TextBox Width="60%" runat="server" ID="txtStatus" ReadOnly="true" Visible="false"></asp:TextBox>
            </p>
            <p>
                <asp:Label runat="server" Text="コメント" Font-Size="15px"></asp:Label>
                 <br />
                 <asp:TextBox runat="server" ID="txtCmt2" Width="60%" Height="120px" TextMode="MultiLine" MaxLength="100"  ></asp:TextBox>
            </p>
        </div>
    </div>
    <div style="margin-top:30px;margin-left:4%">
        <asp:Button CssClass="btn-confirm" runat="server" ID="btnConfirm" Text="確定" OnClick="btnConfirm_Click"  OnClientClick="return confirm('この内容で、よろしいでしょうか？')"/>
        <asp:Button CssClass="btn-cancel" runat="server" ID="btnCancel" Text="キャンセル" OnClick="btnCancel_Click" OnClientClick="return confirm('キャンセルしても、よろしいでしょうか？')" />
        <asp:CheckBox runat="server" ID="ckTodo" Text="TODO作成" />
    </div>
</asp:Content>
