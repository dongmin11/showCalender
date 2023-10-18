<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DepartmentList.aspx.cs" Inherits="EmployeeManagement.Setting.DepartmentList" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
         #MainContent_btnAdd {
            vertical-align: top;
            margin-left: 50px
        }
    </style>
    <h2 >部署一覧<asp:Button runat="server" ID="btnAdd" CssClass="btn-confirm" Text="新規追加" OnClick="btnAdd_Click"/></h2>
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
        ID="gvDeparment" 
        AutoGenerateColumns="false" 
        Width="100%" 
        GridLines="None" 
        HeaderStyle-CssClass ="list-header" 
        Font-Size="15px" 
        OnRowCommand="gvDeparment_RowCommand"
        AllowPaging="true"
        PageSize="10"
        AllowSorting="true"
        OnSorting="gvDeparment_Sorting"
        OnPageIndexChanging="gvDeparment_PageIndexChanging">
        <RowStyle BackColor="#ffffff" VerticalAlign="Middle" HorizontalAlign="Left" Height="30px" />
        <AlternatingRowStyle BackColor="#f2f2f2" VerticalAlign="Middle" Height="30px"></AlternatingRowStyle>
        <Columns>
            <asp:BoundField HeaderText="部署名" DataField="部署名" ItemStyle-HorizontalAlign="Left" SortExpression="部署ＩＤ" HeaderStyle-Width="35%"/>
            <asp:BoundField HeaderText="作成日"　DataField="登録日時" DataFormatString="{0:yyyy/MM/dd}" SortExpression="登録日時" HeaderStyle-Width="12.5%"/>
            <asp:BoundField HeaderText="作成者"　DataField="登録者" SortExpression="登録者" HeaderStyle-Width="15%"/>
            <asp:BoundField HeaderText="更新日"　DataField="修正日時" DataFormatString="{0:yyyy/MM/dd}" SortExpression="修正日時" HeaderStyle-Width="12.5%"/>
            <asp:BoundField HeaderText="更新者"　DataField="修正者" SortExpression="修正者" HeaderStyle-Width="15%"/>
             <asp:TemplateField HeaderText="編集" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="10%">
                <ItemTemplate>
                    <asp:Button Height="26px" Width="75px" runat="server" Text="編集" CommandName="Edit" CommandArgument='<%# Eval("部署ＩＤ") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <%--ページボダン設定--%>
        <PagerSettings Mode="NumericFirstLast" Visible="true" />
        <PagerStyle HorizontalAlign="Right" Font-Size="15px" CssClass="btnPagging" />
    </asp:GridView>
</asp:Content>
