@Code
    ViewData("Title") = "About This Project"
End Code

<hgroup class="title">
    <h1>@ViewData("Title").</h1>

</hgroup>

<article>
    <p>
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;This project was created at the University of Wisconsin - Stout for the CS-448 Fall 2014 class. It is planned to be continued in the CS-458 class during the spring 2015 semester. It was created by Eric Bonsness, Linnea Grenquist, Joshua Loschen, Alec Mackert, and Wade Meyers. It was completed under the supervision of Dr. Seth Berrier and the coach Matthew Schauf. This project was made for educational purposes. If you have any questions, comments, or concerns please see our @Html.ActionLink("contacts", "Contact", "Home") page.
    </p>
</article>

<aside>
    <h3>Links</h3>
    <p>
        Please check out other areas of our website!
    </p>
    <ul>
        <li>@Html.ActionLink("Home", "Index", "Home")</li>
        <li>@Html.ActionLink("About", "About", "Home")</li>
        <li>@Html.ActionLink("Contact", "Contact", "Home")</li>
        <li>@Html.ActionLink("Log Training", "AtheleteLog", "Forms")</li>
        <li>@Html.ActionLink("Career Overview", "CareerOverview", "Home")</li>
        <li>@Html.ActionLink("Weekly Overview", "WeeklyOverview", "Home")</li>
        <li>@Html.ActionLink("Coach Edit", "CoachEdit", "Home")</li>
    </ul>
</aside>