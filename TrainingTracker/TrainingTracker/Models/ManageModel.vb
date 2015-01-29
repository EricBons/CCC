Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Public Class ManageModel
    Inherits ValidationBase
    <Required()> _
    <Display(Name:="Email")> _
    Public Property Email As String
    <Display(Name:="Change Password")> _
    Public Property ChangePassword As Boolean
    <Required()> _
    <DataType(DataType.Password)> _
    <Display(Name:="Password")> _
    Public Property OldPassword As String
    <Required()> _
    <DataType(DataType.Password)> _
    <Display(Name:="New Password")> _
    Public Property Password As String
    <Required()> _
    <DataType(DataType.Password)> _
    <Display(Name:="Re-enter Password")> _
    <Compare("Password", ErrorMessage:="Passwords must match")> _
    Public Property PasswordConfirm As String
    <Required()> _
    <Display(Name:="First Name")> _
    Public Property FName As String
    <Required()> _
    <Display(Name:="Last Name")> _
    Public Property LName As String
End Class
