Imports System.Diagnostics.CodeAnalysis
Imports System.Security.Principal
Imports System.Transactions
Imports System.Web.Routing
Imports DotNetOpenAuth.AspNet
Imports Microsoft.Web.WebPages.OAuth
Imports WebMatrix.WebData
Imports System.Data.Entity
Imports System.Web.Helpers

<Authorize()> _
Public Class AccountController
    Inherits System.Web.Mvc.Controller
    Dim db As New DomainContext
    '
    ' GET: /Account/Login

    <AllowAnonymous()> _
    Public Function Login(ByVal returnUrl As String) As ActionResult
        ViewData("ReturnUrl") = returnUrl
        Return View(New LoginModel)
    End Function

    '
    ' POST: /Account/Login

    <HttpPost()> _
    <AllowAnonymous()> _
    Public Function Login(ByVal model As LoginModel, ByVal returnUrl As String) As ActionResult
        If isValid(model.Email, model.Password) Then
            FormsAuthentication.SetAuthCookie(model.Email, False)
            Return RedirectToAction("Index", "Home")
        Else
            model.ErrorMessage = "Login credentials are incorrect."
        End If
        Return View(model)
    End Function

    '
    ' POST: /Account/LogOff

    <HttpPost()> _
    <ValidateAntiForgeryToken()> _
    Public Function LogOff() As ActionResult
        FormsAuthentication.SignOut()

        Return RedirectToAction("Index", "Home")
    End Function

    '
    ' GET: /Account/Register

    <AllowAnonymous()> _
    Public Function Register() As ActionResult
        Dim model As New RegisterModel
        Return View(model)
    End Function

    '
    ' POST: /Account/Register

    <HttpPost()> _
    <AllowAnonymous()> _
    Public Function Register(ByVal model As RegisterModel) As ActionResult
        Dim user = db.People.Where(Function(x) x.Email = model.Email).FirstOrDefault()
        Try
            If ModelState.IsValid AndAlso user Is Nothing Then 'add code to check an exact email does not exist already
                Dim newUser = db.People.Create()
                Dim salt = "Green Eggs and Ham"
                newUser.FName = model.FName
                newUser.LName = model.LName
                newUser.Email = model.Email
                newUser.Passwd = Crypto.SHA256(model.Password + salt).ToString()
                'newUser.Passwd = model.Password
                db.People.Add(newUser)
                db.SaveChanges()
                Return RedirectToAction("Index", "Home")
            Else
                model.ErrorMessage = "The email you entered already exists, please use a new one."
            End If
        Catch e As Exception
            model.ErrorMessage = "An unexpected error has occurred. Please reload the page and try again."
        End Try

        ' If we got this far, something failed, redisplay form
        Return View(model)
    End Function

    <Authorize> _
    Public Function Manage() As ActionResult
        Dim account = currentUser()
        Dim model As New ManageModel With {.Email = account.Email, .FName = account.FName, .LName = account.LName}
        Return View(model)
    End Function

    <HttpPost()> _
    <Authorize> _
    Public Function Manage(model As ManageModel) As ActionResult
        If (model.ChangePassword AndAlso ModelState.IsValid) OrElse
            Not (model.ChangePassword OrElse String.IsNullOrWhiteSpace(model.OldPassword) OrElse
            String.IsNullOrWhiteSpace(model.Email) OrElse String.IsNullOrWhiteSpace(model.FName) OrElse
            String.IsNullOrWhiteSpace(model.LName)) Then
            Dim user = db.People.Where(Function(x) x.Email = model.Email).FirstOrDefault()
            Dim salt = "Green Eggs and Ham"
            model.OldPassword = Crypto.SHA256(model.OldPassword + salt).ToString()
            If user Is Nothing OrElse (user.Passwd = model.OldPassword) Then
                Dim currentAcct = currentUser()
                currentAcct.Email = model.Email
                currentAcct.FName = model.FName
                currentAcct.LName = model.LName
                If model.ChangePassword Then
                    currentAcct.Passwd = Crypto.SHA256(model.Password + salt).ToString()
                End If
                db.SaveChanges()
                FormsAuthentication.SignOut()
                FormsAuthentication.SetAuthCookie(model.Email, False)
                Return RedirectToAction("Index", "Home")
            Else
                If user IsNot Nothing AndAlso user.Email <> currentUser().Email Then
                    model.ErrorMessage = "The email you entered already exists for another user."
                Else
                    model.ErrorMessage = "Your password is incorrect."
                End If
            End If
        End If
        Return View(model)
    End Function

    Public Function isValid(email As String, password As String) As Boolean
        Dim validity As Boolean = False
        Dim salt = "Green Eggs and Ham"
        password = Crypto.SHA256(password + salt).ToString()
        Dim user = db.People.FirstOrDefault(Function(x) x.Email = email And x.Passwd = password)
        If user IsNot Nothing Then
            validity = True
        End If
        Return validity
    End Function

#Region "Helpers"
    Private Function RedirectToLocal(ByVal returnUrl As String) As ActionResult
        If Url.IsLocalUrl(returnUrl) Then
            Return Redirect(returnUrl)
        Else
            Return RedirectToAction("Index", "Home")
        End If
    End Function

    ''' <summary>
    ''' Gets the current user. If the user is not logged in, it will return nothing.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function currentUser() As Person
        Dim account As Person = Nothing
        Try
            Dim email = User.Identity.Name
            account = db.People.FirstOrDefault(Function(x) x.Email = email)
        Catch e As Exception

        End Try
        Return account
    End Function
#End Region

End Class
