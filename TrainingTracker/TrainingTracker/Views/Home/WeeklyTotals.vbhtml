@ModelType TrainingTracker.WeeklyOverviewModel

@Code
    ViewData("Title") = "WeeklyTotals"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Weekly Totals</h2>
<br />
<h3>@Model.table.title</h3>

<span>End Date</span><br />
@Html.TextBoxFor(Function(x) x.endDate, New With {.readOnly = "readOnly", .class = "datepicker datefield", .id = "dateOfTraining", .Value = Model.endDate.ToShortDateString})

@Html.Partial("TableSorter", Model.table)

<script>
    $(function () { // will trigger when the document is ready
        $('.datepicker').datepicker(); //Initialise any date pickers
    });
    $("#dateOfTraining").change(function () {
        window.location.href = "/Home/WeeklyOverview?targetEmail=@Model.email" + "&endDate="+$("#dateOfTraining").val();
    });
</script>