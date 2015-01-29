Imports System.Diagnostics.CodeAnalysis
Imports System.Security.Principal
Imports System.Transactions
Imports System.Web.Routing
Imports DotNetOpenAuth.AspNet
Imports Microsoft.Web.WebPages.OAuth
Imports WebMatrix.WebData
Imports System.Data.Entity

<Authorize()> _
Public Class FormsController
    Inherits System.Web.Mvc.Controller
    Dim db As New DomainContext
    '
    ' GET: /Forms

    Function AtheleteLog() As ActionResult
        Dim model As New AtheleteLogModel
        Dim routes = db.Routes.Where(Function(x) x.IsActive = True).ToList()
        model.Routes = New SelectList(routes, "Distance", "RouteName")
        Dim activities = db.Activities.Where(Function(x) x.Active = True).ToList()
        For Each x In activities
            Dim temp = New ActivityModel With {.ActivityName = x.ActivityName, .CheckboxOnly = If(x.Description.Equals("CHECKBOX"), True, False),
                                               .Description = x.Description, .IsNumber = x.isNumber}
            model.ActivityValues.Add(temp)
        Next
        Return View(model)
    End Function

    <HttpPost()> _
    Function AtheleteLog(model As AtheleteLogModel) As ActionResult

        Dim routes = db.Routes.Where(Function(x) x.IsActive = True).ToList()
        model.Routes = New SelectList(routes, "Distance", "RouteName")
        Return RedirectToAction("Index", "Home")
        Return View(model)
    End Function

End Class