Imports System
Imports System.Collections.Generic
Imports System.Text
Imports System.Web.Mvc
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports TrainingTracker

<TestClass()> Public Class HomeControllerTest

    <TestMethod()> Public Sub Index()
        ' Arrange
        Dim controller As New HomeController()

        ' Act
        Dim result As ViewResult = DirectCast(controller.Index(), ViewResult)

        ' Assert
        Dim viewData As ViewDataDictionary = result.ViewData
        Assert.AreEqual("Training Trackers home page.", viewData("Message"))
    End Sub

    <TestMethod()> Public Sub About()
        ' Arrange
        Dim controller As New HomeController()

        ' Act
        Dim result As ViewResult = DirectCast(controller.About(), ViewResult)

        ' Assert
        Assert.IsNotNull(result)
    End Sub

    <TestMethod()> Public Sub Contact()
        ' Arrange
        Dim controller As New HomeController()

        ' Act
        Dim result As ViewResult = DirectCast(controller.Contact(), ViewResult)

        ' Assert
        Assert.IsNotNull(result)
    End Sub
End Class
