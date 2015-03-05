@ModelType TrainingTracker.TableSorterModel

@Code
    ViewData("Title") = "CareerTotals"
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<h2>Career Totals</h2>

@Html.Partial("TableSorter",Model)
