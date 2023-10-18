<%@ Page Title="書類申請一覧" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="DocumentList.aspx.cs" Inherits="EmployeeManagement.Document.DocumentList" %>

<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        #MainContent_btnAdd {
            vertical-align: top;
            margin-left: 50px
        }
        
    </style>
    <%--タイトルと新規追加ボダン--%>
    <h2 >書類申請一覧<asp:Button runat="server" ID="btnAdd" CssClass="btn-confirm" Text="新規追加" OnClick="btnAdd_Click" /></h2>
    <p>
        <%--表示件数ドロップダウンリスト--%>
        <asp:DropDownList runat="server" ID="dropNumber" AutoPostBack="true" OnSelectedIndexChanged="dropNumber_SelectedIndexChanged">
            <asp:ListItem Text="10" Value="10"></asp:ListItem>
            <asp:ListItem Text="15" Value="15"></asp:ListItem>
            <asp:ListItem Text="20" Value="20"></asp:ListItem>
            <asp:ListItem Text="30" Value="30"></asp:ListItem>
        </asp:DropDownList>件表示
        <%--ステータスドロップダウンリスト--%>
        <label style="float: right;">ステータス：<asp:DropDownList Height="25px" runat="server" ID="dropStatus" AutoPostBack="true" OnSelectedIndexChanged="dropStatus_SelectedIndexChanged"></asp:DropDownList></label>
    </p>
    <%--書類申請一覧グリッドビュー--%>
    <asp:GridView runat="server" ID="gvDcList"
        GridLines="None"
        Width="100%"
        HeaderStyle-CssClass="list-header"
        OnRowDataBound="gvDcList_RowDataBound"
        OnRowCommand="gvDcList_RowCommand"
        OnPageIndexChanging="gvDcList_PageIndexChanging"
        PageSize="10"
        AllowPaging="true"
        Font-Size="15px"
        OnSorting="gvDcList_Sorting"
        AllowSorting="true"
        AutoGenerateColumns="false">
        <RowStyle BackColor="#ffffff" VerticalAlign="Middle" Height="30px" />
        <AlternatingRowStyle BackColor="#f2f2f2" VerticalAlign="Middle" Height="30px"></AlternatingRowStyle>
        <Columns>
            <%--申請日カラム--%>
            <asp:BoundField HeaderText="申請日"  HeaderStyle-Width="10%" DataField="申請日" DataFormatString="{0:yyyy/MM/dd}" SortExpression="申請日" />
            <%--申請者カラム--%>
            <asp:BoundField HeaderText="申請者" HeaderStyle-Width="22%" DataField="申請者" SortExpression="申請者No" />
            <%--書類名称カラム--%>
            <asp:BoundField HeaderText="書類名称"  DataField="書類No" SortExpression="書類No" />
            <%--受取予定日カラム--%>
            <asp:BoundField HeaderText="受取予定日" HeaderStyle-Width="10%" DataFormatString="{0:yyyy/MM/dd}" DataField="受取予定日" SortExpression="受取予定日" />
            <%--承認者カラム--%>
            <asp:BoundField HeaderText="承認者" HeaderStyle-Width="22%" DataField="承認者" SortExpression="承認者No" />
            <%--ステータスカラム--%>
            <asp:BoundField HeaderText="ステータス" HeaderStyle-Width="10%" DataField="ステータス" SortExpression="ステータス" />
            <%--詳細ボダンカラム--%>
            <asp:TemplateField HeaderText="詳細" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" HeaderStyle-Width="10%">
                <ItemTemplate>
                    <asp:Button Height="26px" Width="75px" runat="server" Text="詳細" CommandName="Detail" CommandArgument='<%# Eval("申請ID") %>' />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <%--ページボダン設定--%>
        <PagerSettings Mode="NumericFirstLast" Visible="true" />
        <PagerStyle HorizontalAlign="Right" Font-Size="15px" CssClass="btnPagging" />
    </asp:GridView>

</asp:Content>
