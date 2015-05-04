Imports System.Diagnostics.CodeAnalysis
Imports System.Security.Principal
Imports System.Transactions
Imports System.Web.Routing
Imports DotNetOpenAuth.AspNet
Imports Microsoft.Web.WebPages.OAuth
Imports WebMatrix.WebData
Imports System.Data.Entity

<Authorize()> _
Public Class FormsController
    Inherits System.Web.Mvc.Controller
    Dim db As New DomainContext
    '
    ' GET: /Forms

    Function AtheleteLog(Optional ByVal selectedDate As Date = Nothing) As ActionResult
        Dim current_user As Person = currentUser()
        Dim model As New AtheleteLogModel
        If selectedDate.Year = 1 Then
            model.Day = System.DateTime.Today.ToShortDateString
        Else
            model.Day = selectedDate
        End If
        Dim routes = db.Routes.Where(Function(x) x.IsActive = True).ToList()
        model.Routes = New SelectList(routes, "Distance", "RouteName")
        Dim activities = db.Activities.Where(Function(x) x.Active = True).ToList()
        Dim personActivities = db.PersonActivities.Where(Function(x) x.DayOfActivity = model.Day AndAlso x.Email = current_user.Email).ToList
        Dim running = db.Running.FirstOrDefault(Function(x) x.DayOfRunning = model.Day AndAlso x.Email = current_user.Email)
        If running IsNot Nothing Then
            model.RunningValue.Route = running.RouteName
            model.RunningValue.Distance = running.Miles
        End If
        For Each x In activities
            Dim pair = personActivities.FirstOrDefault(Function(y) y.ActivityName = x.ActivityName)
            Dim temp = Nothing
            If pair IsNot Nothing Then
                If (pair.Activity.isNumber) Then
                    temp = New ActivityModel With {.ActivityName = x.ActivityName, .CheckboxOnly = If(x.Description.Equals("CHECKBOX"), True, False),
                                               .Description = x.Description, .IsNumber = x.isNumber, .NumericValue = Double.Parse(pair.PAnumber)}
                ElseIf pair.Activity.Description = "CHECKBOX" Then
                    temp = New ActivityModel With {.ActivityName = x.ActivityName, .CheckboxOnly = True,
                                               .Description = x.Description, .IsNumber = x.isNumber, .CheckBoxValue = True}
                Else
                    temp = New ActivityModel With {.ActivityName = x.ActivityName, .CheckboxOnly = False,
                                               .Description = x.Description, .IsNumber = x.isNumber, .Value = pair.PAdescription}
                End If
            Else
                temp = New ActivityModel With {.ActivityName = x.ActivityName, .CheckboxOnly = If(x.Description.Equals("CHECKBOX"), True, False),
                                               .Description = x.Description, .IsNumber = x.isNumber}
            End If
            
            model.ActivityValues.Add(temp)
        Next
        Return View(model)
    End Function

    Function ActivityChange(ByVal targetActivity As String) As ActionResult
        Dim current_user As Person = currentUser()
        Dim model As New CoachEditModel
        Dim ActNam = targetActivity
        Dim activities = db.Activities.Where(Function(x) x.ActivityName = ActNam).ToList()
        Dim temp = Nothing
        If targetActivity = "" Then
            temp = New ActivityModel With {.ActivityName = "", .Description = "", .IsNumber = False, .Active = True, .IsNew = True}
            model.ActivityValues.Add(temp)
        Else
            For Each x In activities
                Dim pair = activities.FirstOrDefault(Function(y) y.ActivityName = x.ActivityName)
                temp = Nothing
                If pair IsNot Nothing Then
                    temp = New ActivityModel With {.ActivityName = x.ActivityName, .Description = x.Description, .IsNumber = x.isNumber, .Active = x.Active, .IsNew = False}
                Else
                    temp = New ActivityModel With {.ActivityName = x.ActivityName, .Description = x.Description, .IsNumber = False, .Active = True, .IsNew = True}
                End If

                model.ActivityValues.Add(temp)
            Next
        End If
        Return View(model)
    End Function

    Function RouteChange(ByVal targetRoute As String) As ActionResult
        Dim current_user As Person = currentUser()
        Dim model As New CoachEditModel
        Dim rounam = targetRoute
        Dim routes = db.Routes.Where(Function(x) x.RouteName = rounam).ToList()
        Dim temp = Nothing
        If targetRoute = "" Then
            temp = New RouteModel With {.RouteName = "", .Distance = 0, .IsActive = True}
            model.RouteValues.Add(temp)
        Else
            For Each x In routes
                Dim pair = routes.FirstOrDefault(Function(y) y.RouteName = x.RouteName)
                temp = Nothing
                If pair IsNot Nothing Then
                    temp = New RouteModel With {.RouteName = x.RouteName, .Distance = x.Distance, .IsActive = x.IsActive, .IsNew = False}
                Else
                    temp = New RouteModel With {.RouteName = "", .Distance = 0, .IsActive = True, .IsNew = True}
                End If
                model.RouteValues.Add(temp)
            Next
        End If

        Return View(model)
    End Function

    <HttpPost()> _
    Function RouteChange(model As CoachEditModel) As ActionResult
        Dim current_user As Person = currentUser()
        Dim routes = db.Routes.ToList()
        model.Routes = New SelectList(routes, "Distance", "RouteName")
        For Each x In model.RouteValues
            Dim existingData = db.Routes.Where(Function(y) y.RouteName = x.RouteName).FirstOrDefault()
            If (x.RouteName = "") Then
                Console.Write("Routes require names")
            ElseIf (x.RouteName IsNot Nothing OrElse x.RouteName = "") And (x.IsNew = True) Then
                Dim newRoute = db.Routes.Create()
                newRoute.RouteName = x.RouteName
                newRoute.Distance = x.Distance
                newRoute.IsActive = x.IsActive
                db.Routes.Add(newRoute)
                db.SaveChanges()
            ElseIf (x.RouteName IsNot Nothing OrElse x.RouteName = "") And (x.IsNew = False) Then
                existingData.Distance = x.Distance
                existingData.IsActive = x.IsActive
                db.SaveChanges()
            End If
        Next
        Return RedirectToAction("CoachEdit", "Home")
        Return View(model)
    End Function

    <HttpPost()> _
    Function ActivityChange(model As CoachEditModel) As ActionResult
        Dim current_user As Person = currentUser()
        Dim activities = db.Activities.ToList()
        model.Activities = New SelectList(activities, "Description", "ActivityName")
        For Each x In model.ActivityValues
            Dim existingData = db.Activities.Where(Function(y) y.ActivityName = x.ActivityName).FirstOrDefault()
            If (existingData Is Nothing) Then
                x.IsNew = True
            End If
            If (x.ActivityName = "") Then
                Console.Write("Routes require names")
            ElseIf (x.ActivityName IsNot Nothing OrElse x.ActivityName = "") And (x.IsNew = True) Then
                Dim newAct = db.Activities.Create()
                newAct.ActivityName = x.ActivityName
                newAct.Description = x.Description
                newAct.isNumber = x.IsNumber
                newAct.Active = x.Active
                db.Activities.Add(newAct)
                db.SaveChanges()
            ElseIf (x.ActivityName IsNot Nothing OrElse x.ActivityName = "") And (x.IsNew = False) Then
                existingData.Description = x.Description
                existingData.isNumber = x.IsNumber
                existingData.Active = x.Active
                db.SaveChanges()
            End If
        Next
        Return RedirectToAction("CoachEdit", "Home")
        Return View(model)
    End Function

    <HttpPost()> _
    Function AtheleteLog(model As AtheleteLogModel) As ActionResult
        Dim current_user As Person = currentUser()
        Dim routes = db.Routes.Where(Function(x) x.IsActive = True).ToList()
        model.Routes = New SelectList(routes, "Distance", "RouteName")
        For Each x In model.ActivityValues
            Dim existingData = db.PersonActivities.Where(Function(y) y.DayOfActivity = model.Day AndAlso y.Email = current_user.Email AndAlso x.ActivityName = y.ActivityName).FirstOrDefault()
            If (x.CheckboxOnly = True) Then
                If existingData IsNot Nothing Then
                    'it exists, and should if true, if false, we must remove it
                    If x.CheckBoxValue = False Then
                        db.PersonActivities.Remove(existingData)
                        db.SaveChanges()
                    End If
                Else
                    If x.CheckBoxValue = True Then
                        Dim newActivity = db.PersonActivities.Create()
                        newActivity.ActivityName = x.ActivityName
                        newActivity.DayOfActivity = model.Day
                        newActivity.Email = current_user.Email
                        newActivity.isNumber = False
                        newActivity.PAdescription = "CHECKBOX"
                        db.PersonActivities.Add(newActivity)
                        db.SaveChanges()
                    End If
                End If
            ElseIf x.Value IsNot Nothing OrElse x.NumericValue = 0 Then 'will it come back an empty string or null? if empty this is perfect
                If existingData IsNot Nothing Then
                    If x.Value IsNot Nothing Then
                        existingData.PAdescription = x.Value
                        db.SaveChanges()
                    Else
                        db.PersonActivities.Remove(existingData)
                        db.SaveChanges()
                    End If
                Else
                    If x.Value IsNot Nothing Then
                        Dim newActivity = db.PersonActivities.Create()
                        newActivity.ActivityName = x.ActivityName
                        newActivity.DayOfActivity = model.Day
                        newActivity.Email = current_user.Email
                        newActivity.isNumber = False
                        newActivity.PAdescription = x.Value
                        db.PersonActivities.Add(newActivity)
                        db.SaveChanges()
                    End If
                End If
            ElseIf Not (x.NumericValue = 0) Then
                    If existingData IsNot Nothing Then
                        existingData.PAnumber = x.NumericValue
                        db.SaveChanges()
                    Else
                        Dim newActivity = db.PersonActivities.Create()
                        newActivity.ActivityName = x.ActivityName
                        newActivity.DayOfActivity = model.Day
                        newActivity.Email = current_user.Email
                        newActivity.isNumber = True
                        newActivity.PAdescription = "0"
                        newActivity.PAnumber = x.NumericValue
                        db.PersonActivities.Add(newActivity)
                        db.SaveChanges()
                    End If
            End If
        Next
        If model.RunningValue.Distance <> -1 Then
            Dim existingData = db.Running.Where(Function(y) y.DayOfRunning = model.Day AndAlso y.Email = current_user.Email).FirstOrDefault
            If existingData IsNot Nothing Then
                existingData.Miles = model.RunningValue.Distance
                existingData.RouteName = model.RunningValue.Route
                db.SaveChanges()
            Else
                Dim newRun = db.Running.Create
                newRun.DayOfRunning = model.Day
                newRun.Email = current_user.Email
                newRun.Miles = model.RunningValue.Distance
                newRun.RouteName = model.RunningValue.Route
                db.Running.Add(newRun)
                db.SaveChanges()
            End If
        End If
        Return RedirectToAction("Index", "Home")
        Return View(model)
    End Function

    <HttpPost()> _
    Function logFeedback(ByVal email As String, ByVal feedback As String, ByVal endDate As Date) As ActionResult
        Dim emailFinal As String = email
        If String.IsNullOrWhiteSpace(email) Then
            emailFinal = currentUser().Email
        End If
        Dim existingFeedback = db.Feedbacks.Where(Function(y) y.EndDate = endDate AndAlso y.Athelete = emailFinal).FirstOrDefault
        If existingFeedback IsNot Nothing Then
            existingFeedback.Feedback1 = feedback
            db.SaveChanges()
        Else
            Dim newFeedback = db.Feedbacks.Create
            newFeedback.Feedback1 = feedback
            newFeedback.Athelete = emailFinal
            newFeedback.EndDate = endDate
            db.Feedbacks.Add(newFeedback)
            db.SaveChanges()
        End If
        Return RedirectToAction("WeeklyOverview", "Home", New With {.targetEmail = email, .endDate = endDate})
    End Function

    Private Function currentUser() As Person
        Dim account As Person = Nothing
        Try
            Dim email = User.Identity.Name
            account = db.People.FirstOrDefault(Function(x) x.Email = email)
        Catch e As Exception

        End Try
        Return account
    End Function

End Class