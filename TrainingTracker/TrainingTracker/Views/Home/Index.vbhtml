@Code
    ViewData("Title") = "Home Page"
End Code

@section featured
    <section class="featured">
    <div class="content-wrapper">
        <hgroup class="title">
            <h1>@ViewData("Title")</h1>
            <h2>@ViewData("Message")</h2>
        </hgroup>
        <p>
            To learn more about our project visit
            @Html.ActionLink("About", "About", "Home", New With {.class = "HomeLink"}). If you have questions for the developers please visit the @Html.ActionLink("Contacts", "Contact", "Home", New With {.class = "HomeLink"}) page.
        </p>
    </div>
</section>
End Section

<h3>Training Tracker Goals:</h3>
<ol class="round">
    <li class="one">
        <h5>Individualized</h5>
        Each athlete will have their own account. They can update their name, the attached email address, and password. 
        They will be able to submit data based on training on a day to day basis.
        Coaches will be able to view information from all atheletes in an easy to understand format.
    </li>

    <li class="two">
        <h5>UI and Interface</h5>
        It is very important that both the atheletes and the coach are able to navigate the site without problems
        and that forms and clear and accurate.
    </li>

    <li class="three">
        <h5>Mobile Interface</h5>
        We plan to design both an Android and IOS application to interface with the system. This would allow submitting information while offline.
    </li>
</ol>

<h3>Start improving your training:</h3>
<ol class="round">
    <li class="one">
        <h5>Getting Started</h5>
        If you have not created an account please do so @Html.ActionLink("here", "Register", "Account").
    </li>

    <li class="two">
        <h5>Already have an account?</h5>
        Then log in @Html.ActionLink("here", "Login", "Account").
    </li>

    <li class="three">
        <h5>Already Logged in?</h5>
        Begin tracking your training!
    </li>
</ol>
