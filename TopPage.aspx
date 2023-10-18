<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TopPage.aspx.cs" Inherits="EmployeeManagement.TopPage.TopPage" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .left{
            margin-left: 10px;
        }
        .schedule{
            margin-left: 30px;
            table-layout: fixed!important;
        }
    </style>
    <div style="height:auto;">
        <%--見出し--%>
      <div class="box-card box-cardshadow"><h3 class="left">予定表</h3>
          <%--スケジュールテーブル--%>
            <%=this.ScheduleTable %>
          <br /><br /><br /><br /></div>
    </div>
    <%--他k差固定--%>
    <div class="box-card box-cardshadow" style="width:300px;height:350px;margin-top:20px"> 
        <label class="title-label">TODO新着情報<a style="float:right" >TODO一覧へ</a></label>
        <asp:GridView runat="server" AutoGenerateColumns="false" id="GvToDo" PageSize="10" AllowPaging="true">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label runat="server" Text="" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="種別"/>
                <asp:BoundField DataField="タイトル"/>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
