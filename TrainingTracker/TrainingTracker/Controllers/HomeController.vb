Public Class HomeController
    Inherits System.Web.Mvc.Controller

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
End Class
