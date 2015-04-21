Public Class HomeController
    Inherits System.Web.Mvc.Controller
    Dim db As New DomainContext
    Function Index() As ActionResult
        ViewData("Message") = "Training Trackers home page."

        Return View()
    End Function

    Function ErrorPage() As ActionResult
        Return View("Error")
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
    Function CoachEdit() As ActionResult
        Dim user = currentUser()
        If user.Admin Then
            Return View("CoachEdit")
        Else
            Dim p = New StringBuilder
            p.Append(<p>You are not authorized for this page</p>)
            Return View(p)
        End If
    End Function

    <Authorize()> _
    Function ActivityEdit() As ActionResult
        Dim provider = New TableProvider(db)
        Dim model = provider.coachActivitiesTable(currentUser())
        Return View("ActivityEdit", model)
    End Function

    <Authorize()> _
    Function RouteEdit() As ActionResult
        Dim provider = New TableProvider(db)
        Dim model = provider.coachRoutesTable(currentUser())
        Return View("RouteEdit", model)
    End Function

    <Authorize()> _
    Function WeeklyOverview(Optional ByVal targetEmail As String = Nothing, Optional ByVal endingDate As DateTime = Nothing) As ActionResult
        Dim model As New WeeklyOverviewModel
        Dim user = currentUser()
        If user.Admin Then
            model.displayFeedbackLink = True
        End If
        If endingDate = Nothing Then
            endingDate = System.DateTime.Today
        End If
        model.endingDate = endingDate
        Dim target As Person = Nothing
        If Not String.IsNullOrWhiteSpace(targetEmail) Then
            target = db.People.Where(Function(x) x.Email = targetEmail).FirstOrDefault
            model.email = target.Email
        Else
            model.email = currentUser().Email
        End If
        Dim startDateString As String = endingDate.AddDays(-3).ToString("yyyy-MM-dd")
        Dim endDateString As String = endingDate.AddDays(3).ToString("yyyy-MM-dd")
        Dim difference As TimeSpan = New TimeSpan(8, 0, 0, 0, 0)
        Dim possibleFeedback As List(Of Feedback) = db.Feedbacks.Where(Function(feedback) feedback.Athelete = model.email AndAlso startDateString <= feedback.EndDate AndAlso feedback.EndDate <= endDateString).ToList
        For Each entry In possibleFeedback
            If difference.Duration > (endingDate - entry.EndDate).Duration Then
                difference = (endingDate - entry.EndDate)
                model.feedback = entry.Feedback1
                If difference.TotalDays = 0 Then
                    Exit For
                End If
            End If
        Next
        Dim provider = New TableProvider(db)
        model.table = provider.weeklyTotalsTable(user, endingDate, target)
        model.plainTextTableInput = provider.plainTextTableString(model.table)
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
