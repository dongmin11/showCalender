<%@ Page Title="TODO" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TodoList.aspx.cs" Inherits="EmployeeManagement.ToDoList.TodoList" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <style>
		.txtbox{
			margin: 2px;
		}
		.header{
			border-bottom-style:solid;
			border-top-style:solid;
			border-width:thin;
			border-color:black;
			height:25px;
		}
		.header a{
			color:black;
		}
		.header th{
			text-align:center;
		}
		.gv{
			margin-bottom:10px;
		}
	</style>
    
       <h2 >TODOリスト</h2>
	<div class="box-card" style="width:84%;float:right;min-height:350px"  > 
		<label   class="title-label" ><%: 　title %></label> 
		<div  style="width:98%;margin:0 auto;">
			<%--検索部分--%>
		<table style="width:70%;float:right; height: 36px;">
			<tr> 
				<td style="text-align:right">タイトル<asp:TextBox ID="txtTitle" class="txtbox" runat="server"></asp:TextBox></td> 
				<td style="text-align:right"> <asp:Label ID="Label3"  runat="server" >
         発信者
        <asp:TextBox  runat="server" width="60%"  class="txtbox" ID="txtWriter"  /> 
        <asp:Button OnClick="btnSearch_Click" ID="btnSearch"  Text="検索" Width="70px" BorderColor="#2e75b6" BackColor="#2e75b6" runat="server"/>
        </asp:Label></td></tr></table>
			<%--一覧表部分--%>
		<div  style="width:100%;float:right;">
			<asp:GridView 
				ID="GvTodoList"
				runat="server"
				GridLines="None"
				AutoGenerateColumns="false"
				Width="100%"
				Font-Size="small"
				OnRowDataBound="GvTodoList_RowDataBound"
				HeaderStyle-CssClass="header"
				OnRowCommand="GvTodoList_RowCommand"
				AllowPaging="true"
				OnPageIndexChanging="GvTodoList_PageIndexChanging"
				PageSize="20"
				OnSorting="GvTodoList_Sorting"
				AllowSorting="true"
				CssClass="gv"
				>
				<RowStyle BackColor="#ffffff" VerticalAlign="Top" />
				<AlternatingRowStyle BackColor="#f2f2f2" VerticalAlign="Top" ></AlternatingRowStyle>
				<Columns>
					<asp:TemplateField>
						<ItemTemplate>
							<asp:HiddenField runat="server" ID="TodoID" Value=<%#Eval("TODO番号") %>/>
							<asp:HiddenField runat="server" ID="Files" Value=<%#Eval("添付ファイル１").ToString()+Eval("添付ファイル２").ToString()+Eval("添付ファイル３").ToString()%>  />
						</ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField HeaderText="種別" ItemStyle-HorizontalAlign="Center" DataField="種別" HeaderStyle-Width="5%"  SortExpression="種別" />
					<asp:BoundField HeaderText="状況" ItemStyle-HorizontalAlign="Center" DataField="完了フラグ" HeaderStyle-Width="5%"   />
					<asp:TemplateField HeaderText="タイトル"  SortExpression="タイトル"　 >
					 <ItemTemplate>
						 <asp:LinkButton ID="lbtnTitle" CommandName="GoConfirm" CommandArgument='<%# Eval("TODO番号") %>'   runat="server" Text=<%#Eval("タイトル")%>></asp:LinkButton>
						 <asp:Image ID="image" runat="server" ImageUrl="~/Images/icons8-添付-16.png" Visible="false" />
					 </ItemTemplate>
					</asp:TemplateField>
					<asp:BoundField HeaderText="発信者" ItemStyle-HorizontalAlign="Left" DataField="社員名称" HeaderStyle-Width="20%"  SortExpression="社員名称" />
					<asp:BoundField HeaderText="発信日" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" DataField="発信日時" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HeaderStyle-Width="15%"  SortExpression="発信日時" />
					<asp:BoundField HeaderText="開封日" ItemStyle-Font-Size="Small" ItemStyle-HorizontalAlign="Center" DataField="開封日時" DataFormatString="{0:yyyy/MM/dd HH:mm:ss}" HeaderStyle-Width="15%"  SortExpression="開封日時" />
					<asp:BoundField HeaderText="コメント" DataField="コメント" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="10%"   />
				</Columns>
				<PagerSettings Mode="NumericFirstLast" Visible="true"  />
				<PagerStyle  HorizontalAlign="Right" Font-Size="Small" CssClass="btnPagging" />
			</asp:GridView>
		</div>
		</div>
	</div>
    <%--メニュー部分--%>
    <div id="menudiv"  class="todo-menu box-card" >
	<label   class="title-label" >　メニュー</label> <table style="width:100%"  >
			<tr><td style="text-align:left" ><a id="aTodoEdit" href="TodoEdit.aspx" >・新規作成</a></td></tr><tr><th>・受信一覧</th></tr><tr><td><a id="aAllReceive" href="TodoList.aspx">　・すべて受信一覧</a></td></tr><tr><td><a id="aUncomplete" href="TodoList.aspx?ReceiveSta=0"  >　・未完了一覧</a></td></tr><tr><th>・送信一覧</th></tr><tr><td><a id="aSent" href="TodoList.aspx?SendSta=1">　・送信済み一覧</a></td></tr><tr><td><a id="aSendWaite" href="TodoList.aspx?SendSta=0">　・送信待ち一覧</a></td></tr></table></div></asp:Content>