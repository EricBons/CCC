@ModelType TrainingTracker.RegisterModel

@Code
    ViewData("Title") = "Register"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Register</h2>
@Using (Html.BeginForm("Register"))
    Html.ValidationSummary(True, "Registeration Failed, fields.")
    If Model.SubmissionFailed Then
        @<span style="color:red">
             @Model.ErrorMessage
    </span>
    End If
    @<div>
        <fieldset>
            <legend>Register Form</legend>
            <div class="editor-label">@Html.LabelFor(Function(Model) Model.Email)</div>
            <div class="editor-field">
                @Html.TextBoxFor(Function(Model) Model.Email)
            @Html.ValidationMessageFor(Function(Model) Model.Email)
        </div>
        <div class="editor-label">@Html.LabelFor(Function(Model) Model.Password)</div>
        <div class="editor-field">
            @Html.PasswordFor(Function(Model) Model.Password)
        @Html.ValidationMessageFor(Function(Model) Model.Password)
        <div class="editor-label">@Html.LabelFor(Function(Model) Model.PasswordConfirm)</div>
        <div class="editor-field">
            @Html.PasswordFor(Function(Model) Model.PasswordConfirm)
            @Html.ValidationMessageFor(Function(Model) Model.PasswordConfirm)
        </div>
        <div class="editor-label">@Html.LabelFor(Function(Model) Model.FName)</div>
        <div class="editor-field">
            @Html.TextBoxFor(Function(Model) Model.FName)
        @Html.ValidationMessageFor(Function(Model) Model.FName)
    </div>
    <div class="editor-label">@Html.LabelFor(Function(Model) Model.LName)</div>
    <div class="editor-field">
        @Html.TextBoxFor(Function(Model) Model.LName)
    @Html.ValidationMessageFor(Function(Model) Model.LName)
</div>
<input type="submit" value="Register" />
</fieldset>
</div>
End Using
<script>
    $("#Register").validate();
</script>