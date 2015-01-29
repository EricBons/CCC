@ModelType TrainingTracker.LoginModel

@Code
    ViewData("Title") = "Login"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Login</h2>
@Using (Html.BeginForm("Login"))
    If Model.SubmissionFailed Then
        @<span style="color:red">
             @Model.ErrorMessage
    </span>
    End If
    'Html.ValidationSummary(True, "Login Failed, fields.")
    @<div>
        <fieldset>
            <legend>Login Form</legend>
            <div class="editor-label">@Html.LabelFor(Function(Model) Model.Email)</div>
            <div class="editor-field">
                @Html.TextBoxFor(Function(Model) Model.Email)
                @Html.ValidationMessageFor(Function(Model) Model.Email)
            </div>
            <div class="editor-label">@Html.LabelFor(Function(Model) Model.Password)</div>
            <div class="editor-field">
                @Html.PasswordFor(Function(Model) Model.Password)
                @Html.ValidationMessageFor(Function(Model) Model.Password)
            </div>
            <input type="submit" value="Login" />
        </fieldset>
    </div>
End Using
<script>
    $("#Lgoin").validate();
</script>
