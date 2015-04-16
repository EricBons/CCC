@Code
    ViewData("Title") = "Routes Edit"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@Html.ActionLink("Coach Edit", "CoachEdit", "Home")
<br />
<br />
<h2>Route Edit</h2>
<br />
<br />
@Html.Partial("TableSorter", Model)
<br />
<input type="submit" value="Submit" />
<script>
    
</script>