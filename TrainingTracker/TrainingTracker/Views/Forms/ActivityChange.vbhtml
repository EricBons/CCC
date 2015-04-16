@ModelType TrainingTracker.CoachEditModel

@Code
    Dim counter = 0
    ViewData("Title") = "Activity Change"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Activity Change</h2>
@Using (Html.BeginForm("Activity Change"))
    Html.ValidationSummary(True, "Management Failed, fields.")
    If Model.SubmissionFailed Then
    @<span style="color:red">
        @Model.ErrorMessage
    </span>
End If
    @<span>Activity Name</span>@<br />
    @Html.TextBoxFor(Function(x) x.ActivityValues, New With {.readOnly = "readOnly", .class = "actNam", .id = "actNam", .Value = "AM Comments"})
    @For Each activity In Model.ActivityValues
        @<br />@<br />
        @<span>Description (To create an activity that uses a checkbox entry style enter: CHECKBOX)</span>@<br />
            @Html.TextAreaFor(Function(x) x.ActivityValues.Item(counter).Description)
        @<br />@<br />
        @<span>IsNumber</span>@<br />
            @Html.CheckBoxFor(Function(x) x.ActivityValues.Item(counter).CheckboxOnly, New With {.style = "display:none;", .readOnly = "readOnly"})
            @Html.CheckBoxFor(Function(x) x.ActivityValues.Item(counter).IsNumber)
        @<br />@<br />
        @<span>Is Active</span>@<br />
            @Html.CheckBoxFor(Function(x) x.ActivityValues.Item(counter).CheckboxOnly, New With {.style = "display:none;", .readOnly = "readOnly"})
            @Html.CheckBoxFor(Function(x) x.ActivityValues.Item(counter).Active)
        @<br />@<br />
    counter = counter + 1
Next

    @<input type="submit" value="Submit" />
End Using
<script>
    $("#actName").change(function () {
        window.location.href = "/Forms/AtheleteLog?targetActivity=" + $("#actNam").val();
    });

</script>

