@Code
    ViewData("Title") = "Activity Edit"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@Html.ActionLink("Coach Edit", "CoachEdit", "Home")
<br />
<br />
<h2>Activity Edit</h2>
<br />
<br />
@Html.Partial("TableSorter", Model)