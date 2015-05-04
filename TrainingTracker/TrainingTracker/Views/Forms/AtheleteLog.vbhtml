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
@<span>Date</span>@<br/>
@Html.TextBoxFor(Function(x) x.Day, New With {.readOnly = "readOnly",.class="datepicker datefield",.id="dateOfTraining",.Value=Model.Day.ToShortDateString})
@For Each activity In Model.ActivityValues
    @<div>
        <span>@activity.ActivityName</span><br/>
            @Html.TextAreaFor(Function(x) x.ActivityValues.Item(counter).ActivityName, New With {.style = "display:none;",.readOnly="readOnly"})
        @If activity.CheckboxOnly Then
                @Html.CheckBoxFor(Function(x) x.ActivityValues.Item(counter).CheckboxOnly, New With {.style = "display:none;", .readOnly = "readOnly"})
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
    $(function () { // will trigger when the document is ready
        $('.datepicker').datepicker(); //Initialise any date pickers
        @If String.IsNullOrWhiteSpace(Model.RunningValue.Route) Then
            @Html.Raw("$('#RunningRouteName').val($('#RunningValue_Route option:selected').text());")
        End If
    });
    $("#dateOfTraining").change(function () {
        window.location.href = "/Forms/AtheleteLog?selectedDate=" + $("#dateOfTraining").val();
    });
    $("#RouteDistance").val(
        @If Model.RunningValue.Distance <>-1 Then
            @Model.RunningValue.Distance
        Else
            @Html.Raw("$('#RunningValue_Route').val()")
        End If
        );
    $('#RunningValue_Route option').each(function () {        
        if ($(this).text() == "@Model.RunningValue.Route") {
            $(this).attr('selected', 'selected');
            $('#RunningRouteName').val($('#RunningValue_Route option:selected').text());
        }
    });
    $("#RunningValue_Route").change(function () {
        $("#RouteDistance").val($("#RunningValue_Route").val());
        $("#RunningRouteName").val($("#RunningValue_Route option:selected").text());
    });
</script>

