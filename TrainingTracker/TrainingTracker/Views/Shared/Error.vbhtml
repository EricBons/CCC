@ModelType System.Web.Mvc.HandleErrorInfo

@Code
    ViewData("Title") = "Error"
End Code

<hgroup class="title">
    <h1 class="error">Somthing went wrong with your request. Please try again.</h1><br />
    <h2 class="error">If the problem persists, contact the developers @Html.ActionLink("here", "Contact", "Home").</h2>
</hgroup>
