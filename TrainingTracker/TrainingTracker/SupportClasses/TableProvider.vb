Public Class TableProvider
    Private db As DomainContext
    Sub New(ByVal domainContext As DomainContext)
        db = domainContext
    End Sub
    Public Function careerTotalsTable(ByVal currentUser As Person) As TableSorterModel
        Dim table As New TableSorterModel
        table.title = "Career Totals"
        table.columns = New List(Of Column) From
            {
            New Column With {.filterable = False, .sortable = True, .name = "First Name"},
            New Column With {.filterable = False, .sortable = True, .name = "Last Name"},
            New Column With {.filterable = False, .sortable = True, .name = "Email"},
            New Column With {.filterable = False, .sortable = True, .name = "XTraining"},
            New Column With {.filterable = False, .sortable = True, .name = "Distance"}
            }
        table.rows = New List(Of Row)
        If currentUser.Admin Then
            Dim people = db.People.ToList()
            For Each person In people
                Dim row = New Row With {.values = New List(Of String) From {person.FName, person.LName, "<a href='/Home/WeeklyOverview?targetEmail=" + person.Email + "'>" + person.Email + "</a>"}}
                Dim totalXT As Double = 0
                Dim crossTraining = db.PersonActivities.Where(Function(x) x.Email = person.Email AndAlso x.ActivityName = "XTraining").ToList()
                For Each xt In crossTraining
                    totalXT = totalXT + xt.PAnumber
                Next
                row.values.Add(totalXT.ToString)
                Dim totalDistance As Double = 0
                Dim running = db.Running.Where(Function(x) x.Email = person.Email).ToList()
                For Each run In running
                    totalDistance = totalDistance + run.Miles
                Next
                row.values.Add(totalDistance.ToString)
                table.rows.Add(row)
            Next
        Else
            Dim row = New Row With {.values = New List(Of String) From {currentUser.FName, currentUser.LName, currentUser.Email}}
            Dim totalXT As Double = 0
            Dim crossTraining = db.PersonActivities.Where(Function(x) x.Email = currentUser.Email AndAlso x.ActivityName = "XTraining").ToList
            For Each xt In crossTraining
                totalXT = totalXT + xt.PAnumber
            Next
            row.values.Add(totalXT.ToString)
            Dim totalDistance As Double = 0
            Dim running = db.Running.Where(Function(x) x.Email = currentUser.Email).ToList()
            For Each run In running
                totalDistance = totalDistance + run.Miles
            Next
            row.values.Add(totalDistance.ToString)
            table.rows.Add(row)
        End If
        table.id = "CareerTotals"
        Return table
    End Function

    Public Function weeklyTotalsTable(ByVal currentUser As Person, ByVal endDate As DateTime, Optional ByVal target As Person = Nothing) As TableSorterModel
        Dim targetActivities As List(Of PersonActivity) = Nothing
        Dim targetRunning As List(Of Running) = Nothing
        Dim table As New TableSorterModel
        Dim startDateString As String = endDate.AddDays(-6).ToString("yyyy-MM-dd")
        Dim endDateString As String = endDate.ToString("yyyy-MM-dd")
        If target IsNot Nothing AndAlso currentUser.Admin Then
            'target is somthing, react accordingly
            targetActivities = db.PersonActivities.Where(Function(x) x.Email = target.Email AndAlso startDateString <= x.DayOfActivity AndAlso x.DayOfActivity <= endDateString).ToList
            targetRunning = db.Running.Where(Function(x) x.Email = target.Email AndAlso startDateString <= x.DayOfRunning AndAlso x.DayOfRunning <= endDateString).ToList
            table.title = target.FName + " " + target.LName + ", " + target.Email
        Else
            'no target, fetch table for current user
            targetActivities = db.PersonActivities.Where(Function(x) x.Email = currentUser.Email AndAlso startDateString <= x.DayOfActivity AndAlso x.DayOfActivity <= endDateString).ToList
            targetRunning = db.Running.Where(Function(x) x.Email = currentUser.Email AndAlso startDateString <= x.DayOfRunning AndAlso x.DayOfRunning <= endDateString).ToList
            table.title = currentUser.FName + " " + currentUser.LName + ", " + currentUser.Email
        End If
        table.columns = New List(Of Column) From
            {
            New Column With {.filterable = False, .sortable = True, .name = "Date"},
            New Column With {.filterable = False, .sortable = True, .name = "Activity"},
            New Column With {.filterable = False, .sortable = True, .name = "Value"}
            }
        table.rows = New List(Of Row)
        For Each log In targetActivities
            Dim row = New Row With {.values = New List(Of String) From {log.DayOfActivity.ToShortDateString, log.ActivityName}}
            If log.isNumber Then
                row.values.Add(log.PAnumber)
            ElseIf log.Activity.Description = "CHECKBOX" Then
                row.values.Add("Completed")
            Else
                row.values.Add(log.PAdescription)
            End If
            table.rows.Add(row)
        Next
        'add final row for totals of XT and Running for the given targetActivities
        Dim XTrow = New Row With {.values = New List(Of String) From {endDate.AddDays(-6).ToShortDateString + "-" + endDate.ToShortDateString, "Total XT"}}
        Dim crossTraining = targetActivities.Where(Function(x) x.ActivityName = "XTraining").ToList()
        Dim totalXT As Double = Nothing 'sum total xt for the week
        For Each xt In crossTraining
            totalXT = totalXT + xt.PAnumber
        Next
        XTrow.values.Add(totalXT)
        table.rows.Add(XTrow)

        For Each run In targetRunning
            Dim row = New Row With {.values = New List(Of String) From {run.DayOfRunning.ToShortDateString, "Route: " + run.RouteName, run.Miles}}
            table.rows.Add(row)
        Next

        Dim runningRow = New Row With {.values = New List(Of String) From {endDate.AddDays(-6).ToShortDateString + "-" + endDate.ToShortDateString, "Total Distance"}}
        Dim totalDistance As Double = Nothing 'sum total running for the week
        For Each run In targetRunning
            totalDistance = totalDistance + run.Miles
        Next
        runningRow.values.Add(totalDistance)
        table.rows.Add(runningRow)
        table.id = "WeeklyTotals"
        Return table
    End Function

    Public Function coachActivitiesTable(ByVal currentUser As Person) As TableSorterModel
        Dim table As New TableSorterModel
        table.title = "coachActivities"
        table.columns = New List(Of Column) From
            {
            New Column With {.filterable = False, .sortable = True, .name = "Active"},
            New Column With {.filterable = False, .sortable = True, .name = "Activity Name"},
            New Column With {.filterable = False, .sortable = True, .name = "Description"},
            New Column With {.filterable = False, .sortable = True, .name = "Number Value"},
            New Column With {.filterable = False, .sortable = True, .name = "Edit"}
            }
        table.rows = New List(Of Row)
        If currentUser.Admin Then
            Dim activities = db.Activities.ToList()
            Dim act = ""
            table.rows.Add(New Row With {.values = New List(Of String) From {"", "", "", "", "<a href='/Forms/ActivityChange?targetActivity='>New Activity</a>"}})
            For Each activity In activities
                If activity.Active = True Then
                    'act = "<input type='checkbox' name='ActAct' id='ActAct' Checked='True'>"
                    act = "True"
                Else
                    'act = "<input type='checkbox' name='ActAct' id='ActAct'>"
                    act = "False"
                End If
                Dim row = New Row With {.values = New List(Of String) From {act, activity.ActivityName, activity.Description, activity.isNumber, "<a href='/Forms/ActivityChange?targetActivity=" + activity.ActivityName + "'>Edit</a>"}}
                table.rows.Add(row)
            Next
        End If
        table.id = "coachActivities"
        Return table
    End Function

    Public Function coachRoutesTable(ByVal currentUser As Person) As TableSorterModel
        Dim table As New TableSorterModel
        table.title = "coachRoutes"
        table.columns = New List(Of Column) From
            {
            New Column With {.filterable = False, .sortable = True, .name = "Active"},
            New Column With {.filterable = False, .sortable = True, .name = "Route Name"},
            New Column With {.filterable = False, .sortable = True, .name = "Distance"},
            New Column With {.filterable = False, .sortable = True, .name = "Edit"}
            }
        table.rows = New List(Of Row)
        If currentUser.Admin Then
            Dim routes = db.Routes.ToList()
            Dim act = ""
            table.rows.Add(New Row With {.values = New List(Of String) From {" ", " ", " ", "<a href='/Forms/RouteChange?targetRoute='>New Route</a>"}})
            For Each route In routes
                If route.IsActive = True Then
                    'act = "<input type='checkbox' name='RouAct' id='RouAct' Checked='True'>"
                    act = "True"
                Else
                    act = "False"
                    'act = "<input type='checkbox' name='RouAct' id='RouAct'>"
                End If
                Dim row = New Row With {.values = New List(Of String) From {act, route.RouteName, route.Distance, "<a href='/Forms/RouteChange?targetRoute=" + route.RouteName + "'>Edit</a>"}}
                table.rows.Add(row)
            Next
        End If
        table.id = "coachRoutes"
        Return table
    End Function

    Public Function plainTextTableString(ByVal table As TableSorterModel) As String
        Dim text As String = ""
        For Each col In table.columns
            text = text + col.name + "\t"
        Next
        text = text + "\n"
        For Each tuple In table.rows
            For Each value In tuple.values
                text = text + value + "\t"
            Next
            text = text + "\n"
        Next
        Return text
    End Function
End Class
