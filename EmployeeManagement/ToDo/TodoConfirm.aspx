<%@ Page Title="TODO" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TodoConfirm.aspx.cs" Inherits="EmployeeManagement.ToDo.TodoConfirm" %>
<%@ MasterType VirtualPath="~/Site.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
		.btn_all{
        	height:24px;
        	background-color:#00b0f0;
        	border-style:solid;
        	border-width:thin;
        	margin-right:5px;
        	display:inline
        }
		#divMain input[type=text]{
        	width:60%;
        	background-color:lightgray;
			border-color:black;
			border-width:thin;
        }
		.comment-label{
			word-break:break-all;
			word-wrap:break-word
		}
		#MainContent_txtComment{
			resize:none;
		}
    </style>
	
	<h3>Todoリスト</h3>
	<us:PopupWindow runat="server" ID="pw1"/>
	<div  class="box-cardshadow"  style="width:84%;float:right;" >
		<label  class="title-label"  ><%: ConfirmTitle %></label>
		<div id="divMain" style="width:98%;float:right;margin-top:20px">
			<%--Todoメッセージ発信確認用ボダン--%>
			<p runat="server" id="p1" >
				<asp:Button CssClass="btn_all" runat="server" ID="btnSend" Text="送信"  OnClick="btnSend_Click" ></asp:Button>
				<asp:Button CssClass="btn_all" runat="server" ID="btnSave" Text="保存（送信待ち）"　OnClientClick="return confirm('編集内容を保存しますか？（添付ファイルは保存できません）')" OnClick="btnSave_Click" />
				<asp:Button CssClass="btn_all" runat="server" ID="btnUpdate" Text="編集"　OnClientClick="return confirm('入力内容を編集したいですか？(添付ファイルは再びアップロードしてください。)')" OnClick="btnUpdate_Click" ></asp:Button>
				<asp:Button CssClass="btn_all" runat="server" ID="btnCancel" Text="キャンセル" OnClientClick="return confirm('編集内容を削除して、送信キャンセルしてもよろしいですか？')" OnClick="btnCancel_Click" ></asp:Button>
			</p>
			<%--Todoメッセージ確認用ボダン--%>
			<p runat="server" id="p2" >
				<asp:Button CssClass="btn_all" runat="server"  ID="btnConfirm" Text="確定"　OnClick="btnSend_Click" 　OnClientClick="return confirm('この内容でよろしいでしょうか？')"  />
				<asp:Button CssClass="btn_all" runat="server"  ID="btnReturn" Text="戻る"　 OnClick="btnCancel_Click" />
			</p> 
			<asp:Label ID="lbText" runat="server" Text="以下の内容でよろしいでしょうか?"></asp:Label>
			<%--コメント・完了状況編集部分--%>
			<p runat="server" id="p3"　>
				コメント　　
				<asp:CheckBox runat="server" ID="ckComplete" Text="完了" />
				<br />
				<asp:TextBox runat="server" ID="txtComment" MaxLength="100" Width="60%" Height="66px" TextMode="MultiLine"  ></asp:TextBox>
			</p>
			<%--種別、属性表示部分--%>
			<div>
				種別：
			</div>	
			<p runat="server" id="panel1" style="width:60%;height:26px;background-color:lightgray;border-style:solid;border-width:thin" >
				<asp:Label runat="server" ID="lbKind"  Text="　"></asp:Label>
			</p>
			<%--タイトル表示部分--%>
			<p  >
		        タイトル：
		        <br />
		        <asp:TextBox runat="server" BorderStyle="Solid" ID="txtTile" ReadOnly="true"  ></asp:TextBox>
	        </p>
			<%--本文表示部分--%>
			<p>
		        本文
	            <br />
		        <asp:TextBox runat="server" ID="txtDetail" BackColor="LightGray" Width="60%" Height="200px" TextMode="MultiLine" ReadOnly="true" ></asp:TextBox>
		     </p>
			<%--添付ファイル表示部分--%>
			<div style="margin-top:10px" >
				添付ファイル:
				<br />
				<asp:GridView  runat="server" ID="fileGrid" ShowHeader="false" GridLines="None" Font-Size="Medium" AutoGenerateColumns="false">
					<Columns>
						<asp:TemplateField>
							<ItemTemplate>
								<asp:LinkButton runat="server" ID="linkDownload"  Text='<%# Eval("Text") %>' CommandArgument='<%# Eval("Value") %>' OnClick="DownloadFile" ></asp:LinkButton>
							</ItemTemplate>
						</asp:TemplateField>
						<asp:BoundField DataField="Byte" />
					</Columns>
				</asp:GridView>
			</div>
			<%--宛名先一覧表--%>
			<div style="margin-top:10px;margin-bottom:20px;">
				宛名先一覧
				<br/>
				<asp:GridView  ID="receiverGrid" Width="75%" EmptyDataText="（ありません）" runat="server" AutoGenerateColumns="false" OnRowDataBound="receiverGrid_RowDataBound" >
					<HeaderStyle CssClass="text-center" />
					<Columns>
						<asp:BoundField HeaderStyle-CssClass="text-center" HeaderText  ="名前" DataField="社員名称"  />
						<asp:BoundField HeaderStyle-CssClass="text-center" HeaderText  ="所属"　DataField="部署名"	/>
						<asp:BoundField HeaderStyle-Width="15%" HeaderStyle-CssClass="text-center" HeaderText="開封日" DataField="開封日時" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
						<asp:BoundField HeaderStyle-Width="5%" HeaderStyle-CssClass="text-center" ItemStyle-HorizontalAlign="Center"  HeaderText="状況" DataField="完了フラグ" />
						<asp:BoundField HeaderStyle-Width="15%" HeaderStyle-CssClass="text-center" HeaderText="完了日" DataField="完了日時" DataFormatString="{0:yyyy/MM/dd HH:mm}" />
						<asp:TemplateField HeaderStyle-Width="15%" ItemStyle-Width="15%" HeaderText="コメント">
							<ItemTemplate>
								<asp:Label ID="lbCmtDate" runat ="server" Text='<%# Eval("更新日時","{0:yyyy/MM/dd HH:mm}") %>'  ></asp:Label>
								<br />
								<asp:Label CssClass="comment-label" ID="lbCmt"  runat="server" Text='<%# Eval("コメント") %>'></asp:Label>
							</ItemTemplate>
						</asp:TemplateField>
					</Columns>
				</asp:GridView>
			</div>
		</div>
	</div>
	<%--メニュー部分--%>
    <div id="menudiv"  class="todo-menu box-card" >
	<label  id="lab1" class="title-label" >　メニュー</label> <table style="width:100%"  >
			<tr><td style="text-align:left" ><a href="TodoEdit.aspx" >・新規作成</a></td></tr><tr><th>・受信一覧</th></tr><tr><td><a href="TodoList.aspx">　・すべて受信一覧</a></td></tr><tr><td><a href="TodoList.aspx?ReceiveSta=0"  >　・未完了一覧</a></td></tr><tr><th>・送信一覧</th></tr><tr><td><a href="TodoList.aspx?SendSta=1">　・送信済み一覧</a></td></tr><tr><td><a href="TodoList.aspx?SendSta=0">　・送信待ち一覧</a></td></tr></table></div>
	<script type="text/javascript">
      
	</script>
</asp:Content>
