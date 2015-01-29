@ModelType TrainingTracker.ManageModel

@Code
    ViewData("Title") = "Manage"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Manage</h2>
@Using (Html.BeginForm("Manage"))
    Html.ValidationSummary(True, "Management Failed, fields.")
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
            <div class="editor-label">@Html.LabelFor(Function(Model) Model.OldPassword)</div>
            <div class="editor-field">
                @Html.PasswordFor(Function(Model) Model.OldPassword)
                @Html.ValidationMessageFor(Function(Model) Model.OldPassword)
            </div>
            <div class="editor-label">@Html.LabelFor(Function(Model) Model.ChangePassword)</div>
            <div class="editor-field">
                @Html.CheckBoxFor(Function(Model) Model.ChangePassword, New with {.id="ChangePasswordCheckBox"})
                @Html.ValidationMessageFor(Function(Model) Model.ChangePassword)
            </div>
            <div id="ChangePasswordComponents" style="display:none">
                <div class="editor-label">@Html.LabelFor(Function(Model) Model.Password)</div>
                <div class="editor-field">
                    @Html.PasswordFor(Function(Model) Model.Password)
                    @Html.ValidationMessageFor(Function(Model) Model.Password)
                </div>
                <div class="editor-label">@Html.LabelFor(Function(Model) Model.PasswordConfirm)</div>
                <div class="editor-field">
                    @Html.PasswordFor(Function(Model) Model.PasswordConfirm)
                    @Html.ValidationMessageFor(Function(Model) Model.PasswordConfirm)
                </div>
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
            <input type="submit" value="Update" />
        </fieldset>
    </div>
End Using
<script>
    $("#Manage").validate();
    $("#ChangePasswordCheckBox").click(function () {
        $("#ChangePasswordComponents").toggle();
    }
        )
</script>