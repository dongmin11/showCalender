<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TopPage.aspx.cs" Inherits="EmployeeManagement.TopPage.TopPage" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .left {
            margin-left: 28px;
        }

        .schedule {
            margin-left: 10px;
            table-layout: fixed !important;
        }

        .gridview {
            margin-left: 5px;
            padding-bottom: 2%;
        }

        .black{
            color: black;
        }

    </style>
    <div style="height: auto;">
        <%--見出し--%>
        <h3>予定表</h3>
        <%--スケジュールテーブル--%>
        <%=this.ScheduleTable %>
    </div>
    <table style="margin-left: 10px;">
        <tr>
            <td>
                <%--TODO新着情報--%>
                <div class="box-card box-cardshadow"
                    style="width: 300px; height: 350px; margin-top: 20px;">
                    <label id="lblToDo" class="title-label">TODO新着情報<a id="aToDo" style="float: right" runat="server" href="~/ToDo/TodoList.aspx">TODO一覧へ</a></label>
                    <div style="overflow: auto; padding-bottom: 2%;">
                        <asp:GridView
                            runat="server"
                            CssClass="gridview"
                            AutoGenerateColumns="false"
                            ID="GvToDo"
                            Width="97%"
                            GridLines="Horizontal"
                            BorderWidth="2"
                            RowStyle-BorderWidth="2"
                            OnRowDataBound="GvToDo_RowDataBound"
                            OnRowCommand="LinkButton_RowCommand">
                            <AlternatingRowStyle BackColor="#f2f2f2" VerticalAlign="Top" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField
                                    DataField="種別"
                                    ItemStyle-Width="20%"
                                    ItemStyle-HorizontalAlign="Center" 
                                    />
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton 
                                            ID="BtnToDo"
                                            runat="server"
                                            Text=<%#Eval("タイトル")%>
                                            CommandName="linkToDo"
                                            CommandArgument='<%# Eval("TODO番号") %>'   
                                            ForeColor="Black"
                                            ToolTip= <%#Eval("タイトル")%>>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </td>
            <td>
                <div class="box-card box-cardshadow"
                    style="width: 300px; height: 350px; margin-top: 20px; margin-left: 3.3%;">
                    <label id="lblDocument" class="title-label">未完了一覧情報<a id="aDocument" href="~/Document/DocumentList.aspx" style="float: right" runat="server">書類申請一覧へ</a></label>
                    <div　id="divDocument" class="" runat="server" style="overflow: auto;max-height:300px; padding-bottom: 2%;">
                        <asp:GridView
                            runat="server"
                            CssClass="gridview"
                            AutoGenerateColumns="false"
                            ID="GvDocument"
                            Width="97%"
                            GridLines="Horizontal"
                            BorderWidth="2"
                            RowStyle-BorderWidth="2"
                            OnRowDataBound="GvDocument_RowDataBound"
                            OnRowCommand="LinkButton_RowCommand">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label runat="server" Text=""></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField />
                               <asp:TemplateField>
                                   <ItemTemplate>
                                       <asp:LinkButton
                                           ID="BtnDocument"
                                           runat="server"
                                           Text=<%#Eval("書類No")%>
                                           CommandName="linkDocument"
                                           CommandArgument='<%# Eval("申請ID") %>' 
                                           ForeColor="Black"
                                           ToolTip=<%#Eval("書類No")%>>
                                       </asp:LinkButton>
                                   </ItemTemplate>
                               </asp:TemplateField>
                                <asp:BoundField DataField="ステータス" ItemStyle-HorizontalAlign="Right"/>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </td>
        </tr>
    </table>

</asp:Content>
