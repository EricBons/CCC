@ModelType TrainingTracker.AtheleteLogModel

@Code
    Dim counter = 0
    ViewData("Title") = "AtheleteLog"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Athelete Log</h2>
@Using (Html.BeginForm("Log Training"))
    Html.ValidationSummary(True, "Management Failed, fields.")
    If Model.SubmissionFailed Then
    @<span style="color:red">
        @Model.ErrorMessage
    </span>
End If
@For Each activity In Model.ActivityValues
    @<div>
        <span>@activity.ActivityName</span><br/>
        @If activity.CheckboxOnly Then
                @Html.CheckBoxFor(Function(x) x.ActivityValues.Item(counter).CheckBoxValue)
        ElseIf activity.IsNumber Then
            @<span>@activity.Description</span>@<br/>
            @Html.TextBoxFor(Function(x) x.ActivityValues.Item(counter).NumericValue, New With {.type = "number"})
        Else
                @<span>@activity.Description</span>@<br/>
                @Html.TextAreaFor(Function(x) x.ActivityValues.Item(counter).Value)
                
            End If
        <br/>    
    </div>
    counter=counter+1
Next
@<div>
    @Html.DropDownList("RunningValue_Route", Model.Routes)<br/>
    @Html.TextBoxFor(Function(x) x.RunningValue.Route, New With {.id = "RunningRouteName", .style = "display:none;"})
    @Html.TextBoxFor(Function(x) x.RunningValue.Distance, New With {.type = "number", .id = "RouteDistance"})
</div>
@<input type="submit" value="Submit" />
End Using
<script>
    //$("#RunningValue_Route").selectedIndex = 0;
    //$("#RunningValue_Route").val($("#RunningValue_Route option:first").val());
    $("#RouteDistance").val($("#RunningValue_Route").val());
    $("#RunningRouteName").val($("#RunningValue_Route option:selected").text());
    $("#RunningValue_Route").change(function () {
        $("#RouteDistance").val($("#RunningValue_Route").val());
        $("#RunningRouteName").val($("#RunningValue_Route option:selected").text());
    });
</script>

