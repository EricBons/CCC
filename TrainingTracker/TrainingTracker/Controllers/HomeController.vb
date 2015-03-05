Public Class HomeController
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

    Function CareerOverview() As ActionResult
        Dim provider = New TableProvider(db)
        Dim model = provider.careerTotalsTable(currentUser())
        Return View("CareerTotals", model)
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
