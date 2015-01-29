Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Public Class AtheleteLogModel
    Inherits ValidationBase
    Public Property Routes As SelectList
    Public Property ActivityValues As New List(Of ActivityModel)
    Public Property RunningValue As RunningModel
    Public Property Day As Date
End Class
