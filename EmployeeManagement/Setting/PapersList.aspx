<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PapersList.aspx.cs" Inherits="EmployeeManagement.Setting.PapersList" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
         #MainContent_btnAdd {
            vertical-align: top;
            margin-left: 50px
        }
         
    </style>
    <h2 >申請書マスタ<asp:Button runat="server" ID="btnAdd" CssClass="btn-confirm" Text="新規追加" OnClick="btnAdd_Click" /></h2>
     <p>
        <%--表示件数ドロップダウンリスト--%>
        <asp:DropDownList runat="server" ID="dropNumber" AutoPostBack="true" OnSelectedIndexChanged="dropNumber_SelectedIndexChanged">
            <asp:ListItem Text="10" Value="10"></asp:ListItem>
            <asp:ListItem Text="15" Value="15"></asp:ListItem>
            <asp:ListItem Text="20" Value="20"></asp:ListItem>
            <asp:ListItem Text="30" Value="30"></asp:ListItem>
        </asp:DropDownList>件表示
    </p>
    <asp:GridView runat="server" 
        ID="gvPapers" 
        AutoGenerateColumns="false" 
        Width="100%" 
        GridLines="None" 
        HeaderStyle-CssClass ="list-header" 
        Font-Size="15px" 
        OnRowCommand="gvPapers_RowCommand"
        AllowPaging="true"
        PageSize="10"
        AllowSorting="true"
        OnSorting="gvPapers_Sorting"
        OnPageIndexChanging="gvPapers_PageIndexChanging">
        <RowStyle BackColor="#ffffff" VerticalAlign="Middle" HorizontalAlign="Center" Height="30px" />
        <AlternatingRowStyle BackColor="#f2f2f2" VerticalAlign="Middle" Height="30px"></AlternatingRowStyle>
        <Columns>
            <asp:BoundField HeaderText="書類番号" DataField="SEQ" HeaderStyle-Width="10%" HeaderStyle-CssClass="text-center" SortExpression="SEQ"/>
            <asp:BoundField HeaderText="書類名称" DataField="名称" ItemStyle-HorizontalAlign="Left" SortExpression="名称"/>
             <asp:TemplateField HeaderText="編集" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="10%">
                <ItemTemplate>
                    <asp:Button Height="26px" ID="btnEdit" Width="75px" runat="server" Text="編集" CommandName="Edit" CommandArgument='<%# Eval("SEQ") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <%--ページボダン設定--%>
        <PagerSettings Mode="NumericFirstLast" Visible="true" />
        <PagerStyle HorizontalAlign="Right" Font-Size="15px" CssClass="btnPagging" />
    </asp:GridView>
    
</asp:Content>
