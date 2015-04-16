@ModelType TrainingTracker.CoachEditModel

@Code
    Dim counter = 0
    ViewData("Title") = "Route Change"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Route Change</h2>
@Using (Html.BeginForm("Route Change"))
    Html.ValidationSummary(True, "Management Failed, fields.")
    If Model.SubmissionFailed Then
    @<span style="color:red">
        @Model.ErrorMessage
    </span>
End If
    @<span>Route Name</span>@<br />
    @Html.TextBoxFor(Function(x) x.RouteValues, New With {.readOnly = "readOnly", .class = "rouNam", .id = "rouNam", .Value = "3M Female"})
    @For Each route In Model.RouteValues
        @<br />@<br />
        @<span>Distance</span>@<br />
            @Html.TextBoxFor(Function(x) x.RouteValues.Item(counter).Distance, New With {.type = "number"})
        @<br />@<br />
        @<span>Is Active</span>@<br />
            @Html.CheckBoxFor(Function(x) x.RouteValues.Item(counter).CheckboxOnly, New With {.style = "display:none;", .readOnly = "readOnly"})
            @Html.CheckBoxFor(Function(x) x.RouteValues.Item(counter).CheckBoxValue)
        @<br />@<br />
        counter = counter + 1
    Next

    @<input type="submit" value="Submit" />
End Using
<script>
    $("#rouName").change(function () {
        window.location.href = "/Forms/RouteChange?targetRoute=" + $("#rouNam").val();
    });

</script>

