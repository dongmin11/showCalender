<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Defult.aspx.cs" Inherits="EmployeeManagement._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div style="height:auto;">
      <div class="box-card box-cardshadow">]aa<br /><br /><br /><br /></div>
    </div>
    <div class="box-card box-cardshadow" style="width:300px;height:350px;margin-top:20px"> 
        <label class="title-label">TODO新着情報<a style="float:right" >TODO一覧へ</a></label>
        <asp:GridView runat="server" AutoGenerateColumns="false" >
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label runat="server" Text="" ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Content>
