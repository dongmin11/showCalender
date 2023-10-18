<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PopUPWindow.ascx.cs" Inherits="EmployeeManagement.Common.PopUPWindow" %>
<style>
    .dialog { 
    background-color: #F0FFFF;
    width:400px;
    min-width: 400px;
    }
    .dialog .title{ 
        background-color: #B0C4DE;
        padding: 5px 10px;
    }
    .dialog .content 
    {
        width: 359px;
        padding: 10px;
        margin-bottom: 10px;
        height: 44px;
    }
    .dialog .icon
    {
        float: left;
        width: 48px;
        margin-right: 12px;
    }
    .dialog .message
    {
        float: left;
        width: 320px;
        word-break: break-all; 
        word-wrap: break-word;
    }    
    .dialog .button
    {
        padding: 10px 0;
        text-align: center;
    }
    .modalBackground
    {
        background-color:black;
        opacity:0.50;            
        zoom:1;                  
    }
</style>
<script type="text/javascript">
    function OnButtonClick() {
        window.opener.location.reload();
    }
</script>
<!-- ダミーボタン(非表示) -->
<asp:Button ID="btnDummy" runat="server" Text="Button" Style="display: none" />
<!-- ポップアップ部分 -->
<asp:Panel ID="pnlDialog" runat="server">
    <div class="dialog">
        <div class="title" style="background-color: #336699; color: #FFFFFF">
            <asp:Label  ID="lblTitle" runat="server" Text="社員システム"  ></asp:Label>
        </div>
        <div class="content">
            <div class="icon">
                <asp:Image ID="Image1" runat="server" ImageUrl="~/Images/Info.png" Height="31px" Width="36px" /></div>
            <div class="message">
                <asp:Label ID="lblMsg" runat="server"></asp:Label></div>
        </div>
        <br class="clearfloat" />
        <div class="button">
            <asp:Button ID="btnOk" runat="server" Text="OK" />
        </div>
    </div>
</asp:Panel>
<!-- ModalPopupExtender -->