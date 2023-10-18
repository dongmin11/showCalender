<%@ Page Title="TODO" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="TodoEdit.aspx.cs" Inherits="EmployeeManagement.ToDo.TodoEdit" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
<style>
		.dropdownlist{
			width:200px;
			margin-right:100px;
		}
		.membergrid{
			width:200px;
			
			height:180px;
			border-style:solid;
			border-width:1px;
			border-color:black;
		}
	   #divMember,#divRecerverEdit,#divReceiver{
			display:inline-block;
			overflow:auto;
			
	   }
	   .row-selected{
		   background-color:#00A9FF
	   }
	   .fileup input[type=file]{
		   margin-top:10px;
		   min-width:500px;
	   }
</style>

	<h3>Todoリスト</h3>
	<div runat="server" id="maindiv" class="box-card"  style="width:84%;float:right;" >
		<label  class="title-label" ><%: TodoEditTitle %></label>
		<div style="width:96%;margin:0 auto;">
			<asp:UpdatePanel runat="server" ChildrenAsTriggers="true"  >
				<ContentTemplate>
			<%--宛名先選択部分--%>
		<p>
			<asp:DropDownList ID="dropDepartment" DataValueField="部署ＩＤ" OnSelectedIndexChanged="dropDepartment_SelectedIndexChanged" DataTextField="部署名" CssClass="dropdownlist" runat="server"  AutoPostBack="true" ></asp:DropDownList>
			宛名先：
			<asp:Label runat="server" ID="lbRecerver" ForeColor="Red" ></asp:Label>
		 </p>
			
		<div style="width:100%">
		<div id="divMember" class="membergrid">
			<asp:GridView   Width="100%" OnSelectedIndexChanging="Grid_SelectedIndexChanging"   GridLines="None" ID="MemberGrid" ShowHeader="false" runat="server" AutoGenerateColumns="false" OnRowDataBound="Grid_RowDataBound" >
				<Columns>
					<asp:TemplateField>
						<ItemTemplate>
							<asp:HiddenField runat="server" ID="社員番号" Value=<%#Eval("社員番号")%> />
						</ItemTemplate>
					</asp:TemplateField>
					
					<asp:BoundField  DataField="社員名称" HeaderStyle-Width="80%"/>
					<asp:TemplateField HeaderStyle-Width="20%" >
						<ItemTemplate>
							<asp:CheckBox ID="chkUser" runat="server"  Visible="false" />
						</ItemTemplate>
					</asp:TemplateField>
					
				</Columns>
			</asp:GridView>
		</div>
		<div id="divRecerverEdit" style="width:90px;height:180px;">
		<table style="margin:0 auto;margin-top:60px">
			<tr><td><asp:LinkButton ID="btnAdd" OnClick="btnRecerverEdit_Click"  runat="server" CausesValidation="false" Text="追加>>" ></asp:LinkButton></td></tr>
			<tr><td><asp:LinkButton runat="server" ID="btnDelete" OnClick="btnRecerverEdit_Click" CausesValidation="false" Text ="<<削除"></asp:LinkButton></td></tr>
		</table>
         </div>
		<div id="divReceiver" class="membergrid">
			<asp:GridView   Width="100%" OnSelectedIndexChanging="Grid_SelectedIndexChanging" GridLines="None" ID="RecieverGrid" ShowHeader="false" runat="server" AutoGenerateColumns="false" OnRowDataBound="Grid_RowDataBound">
				<Columns>
					<asp:BoundField DataField="部署名" ItemStyle-Wrap="false"/>
					<asp:BoundField  DataField="社員名称" ItemStyle-Wrap="false" />
					<asp:TemplateField >
						<ItemTemplate>
							<asp:CheckBox ID="chkReceiver" runat="server" Visible="false" />
						</ItemTemplate>
					</asp:TemplateField>
				</Columns>
			</asp:GridView>
		</div>
			</div>
		<%--種別選択部分--%>
		<p id="div4" >
		種別：
		<br />
		<asp:DropDownList Width="290" runat="server" ID="dropKind" DataTextField="名称" DataValueField="SEQ">
			
		</asp:DropDownList>
		</p>
		<%--属性選択部分--%>
		<p >
        属性：
			<br />
		    <asp:CheckBox  ID="ck1" runat="server"  Text="重要　" AutoPostBack="True" />
			<asp:CheckBox  ID="ck2" runat="server" Text="至急　" />
			<asp:CheckBox  ID="ck3" runat="server" Text="親展　" />
			<asp:CheckBox  ID="ck4" runat="server" Text="転送　" />
		</p>
			</ContentTemplate>
			
				</asp:UpdatePanel>
		<%--タイトル入力フォーム--%>
		<p >
		タイトル：
			<asp:Label runat="server" ID="lbTile" ForeColor="Red" ></asp:Label>
		<br />
		<asp:TextBox runat="server" MaxLength="25" ID="txtTile" width="60%"></asp:TextBox>
			
	    </p>
		<%--本文入力フォーム--%>
		<p>
		本文
			<asp:Label runat="server" ID="lbDetail" ForeColor="Red" ></asp:Label>
	    <br />
		<asp:TextBox runat="server" ID="txtDetail" MaxLength="1000" width="60%" Height="200px" TextMode="MultiLine"></asp:TextBox>
		</p>
			<%--添付ファイル追加--%>
		<p class="fileup">
			添付ファイル<asp:Label runat="server" Text="（１ファイル 最大10MB）" ID="lbFileupload"></asp:Label>
			
			<asp:FileUpload ID="FileUpload1" runat="server"  /> 
			<asp:FileUpload ID="FileUpload2" runat="server" />
			<asp:FileUpload ID="FileUpload3" runat="server" />
			
		</p>
		<%--発信・キャンセルボダン--%>	
		<p style="margin-top:10px">
		<asp:Button ID="btnConfirm" runat="server" CssClass="btn-confirm" Text="発信"　　OnClick="btnConfirm_Click"　/>
	    <asp:Button　ID="btnCancel" runat="server" CssClass="btn-cancel"  Text="キャンセル"　OnClientClick="return confirm('キャンセルしてもよろしいですか？')" OnClick="btnCancel_Click"  />
		</p>
			</div>
					
		</div>
	<%--メニュー部分--%>
    <div id="menudiv"  class="todo-menu box-card" >
	<label  id="lab1" class="title-label" >　メニュー</label> <table style="width:100%"  >
			<tr><td style="text-align:left" ><a href="TodoEdit.aspx" >・新規作成</a></td></tr><tr><th>・受信一覧</th></tr><tr><td><a href="TodoList.aspx">　・すべて受信一覧</a></td></tr><tr><td><a href="TodoList.aspx?ReceiveSta=0"  >　・未完了一覧</a></td></tr><tr><th>・送信一覧</th></tr><tr><td><a href="TodoList.aspx?SendSta=1">　・送信済み一覧</a></td></tr><tr><td><a href="TodoList.aspx?SendSta=0">　・送信待ち一覧</a></td></tr></table></div>
	
</asp:Content>

