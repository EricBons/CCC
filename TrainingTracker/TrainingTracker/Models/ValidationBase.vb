Public Class ValidationBase
    Public ReadOnly Property SubmissionFailed
        Get
            Return Not String.IsNullOrWhiteSpace(ErrorMessage)
        End Get
    End Property
    Public Property ErrorMessage As String
End Class
