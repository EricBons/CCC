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
                Dim row = New Row With {.values = New List(Of String) From {person.FName, person.LName, person.Email}}
                Dim totalXT As Double
                db.PersonActivities.Where(Function(x) x.Email = person.Email AndAlso x.ActivityName = "XTraining").ToList().ForEach(Function(y) totalXT = y.PAnumber + totalXT)
                row.values.Add(totalXT.ToString)
                Dim totalDistance As Double
                db.Running.Where(Function(x) x.Email = person.Email).ToList().ForEach(Function(y) totalDistance = y.Miles + totalDistance)
                row.values.Add(totalDistance.ToString)
                table.rows.Add(row)
            Next
        Else
            Dim row = New Row With {.values = New List(Of String) From {currentUser.FName, currentUser.LName, currentUser.Email}}
            Dim totalXT As Double
            db.PersonActivities.Where(Function(x) x.Email = currentUser.Email AndAlso x.ActivityName = "XTraining").ToList.ForEach(Function(x) totalXT = x.PAnumber + totalXT)
            row.values.Add(totalXT.ToString)
            Dim totalDistance As Double
            db.Running.Where(Function(x) x.Email = currentUser.Email).ToList.ForEach(Function(x) totalDistance = x.Miles + totalDistance)
            row.values.Add(totalDistance.ToString)
            table.rows.Add(row)
        End If
        table.id = "CareerTotals"
        Return table
    End Function
    Public Function weeklyTotalsTable(ByVal target As Person, ByVal currentUser As Person) As TableSorterModel
        Return New TableSorterModel
    End Function
End Class
