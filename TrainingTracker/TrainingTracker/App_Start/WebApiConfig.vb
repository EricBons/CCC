Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web.Http

Public Class WebApiConfig
    Public Shared Sub Register(ByVal config As HttpConfiguration)
        config.Routes.MapHttpRoute( _
            name:="DefaultApi", _
            routeTemplate:="api/{controller}/{id}", _
            defaults:=New With {.id = RouteParameter.Optional} _
        )
        
        'Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable(Of T) return type.
        'To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
        'For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
        'config.EnableQuerySupport()
    End Sub
End Class