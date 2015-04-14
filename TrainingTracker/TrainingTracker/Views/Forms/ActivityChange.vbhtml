@ModelType TrainingTracker.ActivityModel

@Code
    ViewData("Title") = "ActivityChange"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Activity Change</h2>

@Html.TextBoxFor(Function(x) x.Day, New With {.readOnly = "readOnly", .class = "ActName", .id = "ActName", .Value = ""})

@Using (Html.BeginForm("Activity Change"))
    Html.ValidationSummary(True, "Management Failed, fields.")
    If Model.SubmissionFailed Then
    @<span style="color:red">
        @Model.ErrorMessage
    </span>
End If

@<input type="submit" value="Submit" />
End Using
<script>
    $("#dateOfTraining").change(function () {
        window.location.href = "/Forms/ActivityChange?targetActivity=" + $("#ActName").val();
    });
</script>