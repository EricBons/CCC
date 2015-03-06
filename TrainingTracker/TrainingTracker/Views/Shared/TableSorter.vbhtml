@ModelType TrainingTracker.TableSorterModel

<table id="SortableTable_@Model.id" class="tablesorter-blue">
    <thead>
        <tr>
            @For Each header In Model.columns
                @<th class="{sorter: @header.sortable.ToString}">@header.name</th>
            Next
        </tr>
    </thead>
    <tbody>
        @For Each row In Model.rows
            @<tr>
                @For Each value In row.values
                    @<td>@value</td>
                Next
            </tr>
        Next
    </tbody>
</table>

<script>
    $(document).ready(function () {
        $("#SortableTable_@Model.id").tablesorter({
            sortList: [[0, 0]],
            cancelSelection: true,
            ignoreCase: true,
            widgets: ["zebra","filter"],
            widgetOptions : {
                filter_columnFilters: true
            }
        });
    });
</script>