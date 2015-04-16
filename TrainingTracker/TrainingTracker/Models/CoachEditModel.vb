Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Public Class CoachEditModel
    Inherits ValidationBase
    Public Property ActivityValues As New List(Of ActivityModel)
    Public Property RouteValues As New List(Of RouteModel)
End Class
