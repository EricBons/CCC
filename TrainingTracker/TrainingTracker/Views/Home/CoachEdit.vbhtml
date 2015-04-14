@Code
    ViewData("Title") = "CoachEdit"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>CoachEdit</h2>

<p>
    <ul>
        <li>@Html.ActionLink("Activity Edit", "ActivityEdit", "Home")</li>
        <li>@Html.ActionLink("Route Edit", "RouteEdit", "Home")</li>
    </ul>
</p>
