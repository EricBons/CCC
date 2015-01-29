<!DOCTYPE html>
<html lang="en">
    <head>
        <meta charset="utf-8" />
        <title>@ViewData("Title") - Training Tracker</title>
        <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
        <meta name="viewport" content="width=device-width" />
        @Styles.Render("~/Content/css")
        @Scripts.Render("~/bundles/modernizr")
        @Scripts.Render("~/bundles/jquery")
        @Scripts.Render("~/bundles/jqueryui")
        @Scripts.Render("~/bundles/jqueryval")
        @RenderSection("scripts", required:=False)
    </head>
    <body>
        <header>
            <div class="content-wrapper">
                <div class="float-left">
                    <p class="site-title"><span class="logo" style="margin-right:-5px"></span>@Html.ActionLink("tout Training Tracker", "Index", "Home")</p>
                </div>
                <div class="float-right">
                    <section id="login">                        
                        @Html.Partial("_LoginPartial")
                    </section>
                    <nav>
                        <ul id="menu">
                            <li>@Html.ActionLink("Home", "Index", "Home")</li>
                            <li>@Html.ActionLink("About", "About", "Home")</li>
                            <li>@Html.ActionLink("Contact", "Contact", "Home")</li>
                        </ul>
                    </nav>
                </div>
            </div>
        </header>
        <div id="body">
            @RenderSection("featured", required:=false)
            <section class="content-wrapper main-content clear-fix">
                @RenderBody()
            </section>
        </div>
        <footer>
            <div class="content-wrapper">
                <div class="float-left">
                    <p>&copy; @DateTime.Now.Year - Training Tracker</p>
                </div>
                <img src="~/Images/Stout.png" style="width: 100%;bottom: 0;left:0">
            </div>
        </footer>

        
    </body>
</html>
