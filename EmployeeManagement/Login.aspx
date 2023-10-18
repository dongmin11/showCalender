<%@ Page Title="ログイン" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="EmployeeManagement.Login" %>

<html lang="ja">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title><%: Page.Title %> - 社員システム</title>
    <link rel="stylesheet" href="./Content/Site.css">
    <webopt:BundleReference runat="server" Path="~/Content/css" />
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
</head>
<body>
    <style>
        html, body {
            height: 100%;
            display: flex;
            flex-direction: column;
        }
        input{
            width:40px;
        }
    </style>
    <form runat="server">
        <div class="navbar navbar-inverse navbar-fixed-top">
            <div>
                <div class="navbar-header"
                    style="width: 230px;">
                    <a class="navbar-brand"
                        runat="server">
                        <table>
                            <tr>
                                <td>
                                    <img alt="Meisen"
                                        style="max-width: 50px;"
                                        src="./Content/Images/logo_.png" />
                                </td>
                                <td>
                                    <span style="color: white; font-size: 18px">社員システム</span>
                                </td>
                            </tr>
                        </table>
                    </a>
                </div>
            </div>
        </div>
        <div>
            <main id="main"
                style="margin-top: 100px; margin-bottom: 0px; padding-bottom: 100px; padding-top: 100px;width:400px;margin: 0 auto;">
                <h1 class="h3">ログイン</h1>
                <asp:Label ID="LblLogin"
                    runat="server"
                    Text=""
                    ForeColor="Red"></asp:Label>
                <div class="form-group row mb-4">
                    <label for="username"
                        class="col-sm-4 login-font">
                        ユーザーID</label>
                    <div class="col-sm-8">
                        <input type="text"
                            id="TxtUserName"
                            name="userName"
                            class="form-control"
                            maxlength="20"
                            runat="server" />
                    </div>
                </div>
                <div class="form-group row mb-4">
                    <label for="password"
                        class="col-sm-4 login-font">
                        パスワード</label>
                    <div class="col-sm-8">
                        <input type="password"
                            id="TxtPassword"
                            name="password"
                            class="form-control"
                            maxlength="16"
                            runat="server" />
                    </div>
                </div>
                <div>
                    <label>
                        <input id="loginKeep"
                            type="checkbox"
                            runat="server" />ログイン状態を保持
                    </label>
                </div>
                <br />
                <div>
                    <asp:Button ID="BtnSignin"
                        runat="server"
                         OnClick ="BtnSignin_Click"
                        Text="ログイン"
                        class="btn btn-lg  btn-block btn-login-color"
                         />
                </div>
                <div class="mt35" id="repassword">
                    <a href="./repassword.aspx">パスワードを忘れた場合</a>
                </div>
            </main>
        </div>
    </form>
    <div class="fixed-bottom">
        <footer class="bg-light">
            <div class="text-center p-3">
                <p>&copy; <%: DateTime.Now.Year %> , Meisen Soft & Network Technology Co.,Ltd.</p>
            </div>
        </footer>
    </div>
</body>
</html>

