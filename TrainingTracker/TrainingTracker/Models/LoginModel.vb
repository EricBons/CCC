Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Public Class LoginModel
    Inherits ValidationBase
    <Required()> _
    <Display(Name:="Email")> _
    Public Property Email As String
    <Required()> _
    <DataType(DataType.Password)> _
    <Display(Name:="Password")> _
    Public Property Password As String
End Class
