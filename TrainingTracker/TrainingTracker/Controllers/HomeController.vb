﻿Public Class HomeController
    Inherits System.Web.Mvc.Controller
    Dim db As New DomainContext
    Function Index() As ActionResult
        ViewData("Message") = "Training Trackers home page."

        Return View()
    End Function

    Function About() As ActionResult
        ViewData("Message") = "Additional Info"

        Return View()
    End Function

    Function Contact() As ActionResult
        ViewData("Message") = "The Development Team"

        Return View()
    End Function

    <Authorize()> _
    Function CareerOverview() As ActionResult
        Dim provider = New TableProvider(db)
        Dim model = provider.careerTotalsTable(currentUser())
        Return View("CareerTotals", model)
    End Function

    <Authorize()> _
    Function WeeklyOverview(Optional ByVal targetEmail As String = Nothing, Optional ByVal endDate As DateTime = Nothing) As ActionResult
        Dim model As New WeeklyOverviewModel
        If endDate = Nothing Then
            endDate = System.DateTime.Today
        End If
        model.endDate = endDate
        Dim target As Person = Nothing
        If Not String.IsNullOrWhiteSpace(targetEmail) Then
            target = db.People.Where(Function(x) x.Email = targetEmail).FirstOrDefault
            model.email = target.Email
        End If
        Dim provider = New TableProvider(db)
        model.table = provider.weeklyTotalsTable(currentUser(), endDate, target)
        Return View("WeeklyTotals", model)
    End Function

    Private Function currentUser() As Person
        Dim account As Person = Nothing
        Try
            Dim email = User.Identity.Name
            account = db.People.FirstOrDefault(Function(x) x.Email = email)
        Catch e As Exception

        End Try
        Return account
    End Function
End Class
