﻿'{[{
Imports Param_RootNamespace.Core.Helpers
Imports Param_RootNamespace.Core.Services
'}]}
Namespace ViewModels
    Public Class SettingsViewModel
        Implements System.ComponentModel.INotifyPropertyChanged

'{[{
        Private ReadOnly Property UserDataService As UserDataService
            Get
                Return Singleton(Of UserDataService).Instance
            End Get
        End Property

        Private ReadOnly Property IdentityService As IdentityService
            Get
                Return Singleton(Of IdentityService).Instance
            End Get
        End Property
'}]}

        Private _elementTheme As ElementTheme = ThemeSelectorService.Theme
'{[{
        Private _logInCommand As RelayCommand
        Private _logOutCommand As RelayCommand
        Private _isLoggedIn As Boolean
        Private _isBusy As Boolean
        Private _user As UserViewModel
'}]}
        Public Property ElementTheme As ElementTheme

'^^
'{[{
        Public ReadOnly Property LoginCommand As RelayCommand
            Get
                If _loginCommand Is Nothing Then
                    _loginCommand = New RelayCommand(AddressOf OnLogin, Function() Not IsBusy)
                End If

                Return _loginCommand
            End Get
        End Property

        Public ReadOnly Property LogOutCommand As RelayCommand
            Get
                If _logOutCommand Is Nothing Then
                    _logOutCommand = New RelayCommand(AddressOf OnLogOut, Function() Not IsBusy)
                End If

                Return _logOutCommand
            End Get
        End Property

        Public Property IsLoggedIn As Boolean
            Get
                Return _isLoggedIn
            End Get
            Set(value As Boolean)
                [Set](_isLoggedIn, value)
            End Set
        End Property

        Public Property IsBusy As Boolean
            Get
                Return _isBusy
            End Get
            Set(value As Boolean)
                [Set](_isBusy, value)
            End Set
        End Property

        Public Property User As UserViewModel
            Get
                Return _user
            End Get
            Set(value As UserViewModel)
                [Set](_user, value)
            End Set
        End Property
'}]}

        Public Sub New()
        End Sub

        Public Async Function InitializeAsync() As Task
            VersionDescription = GetVersionDescription()
'^^
'{[{
            AddHandler dentityService.LoggedIn, AddressOf OnLoggedIn
            AddHandler IdentityService.LoggedOut, AddressOf OnLoggeOut
            AddHandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
            IsLoggedIn = IdentityService.IsLoggedIn()
            User = Await UserDataService.GetUserAsync()
'}]}
        End Function

'^^
'{[{
        Public Sub UnregisterEvents()
            RemoveHandler IdentityService.LoggedIn, AddressOf OnLoggedIn
            RemoveHandler IdentityService.LoggedOut, AddressOf OnLoggeOut
            RemoveHandler UserDataService.UserDataUpdated, AddressOf OnUserDataUpdated
        End Sub

        Private Sub OnUserDataUpdated(sender As Object, user As UserViewModel)
            User = user
        End Sub

        Private Async Sub OnLogIn()
            IsBusy = True
            Dim loginResult = Await IdentityService.LoginAsync()

            If loginResult <> LoginResultType.Success Then
                Await AuthenticationHelper.ShowLoginErrorAsync(loginResult)
                IsBusy = False
            End If
        End Sub

        Private Async Sub OnLogOut()
            IsBusy = True
            Await IdentityService.LogoutAsync()
        End Sub

        Private Sub OnLoggedIn(sender As Object, e As EventArgs)
            IsLoggedIn = True
            IsBusy = False
        End Sub

        Private Sub OnLoggeOut(sender As Object, e As EventArgs)
            User = Nothing
            IsLoggedIn = False
            IsBusy = False
        End Sub
'}]}
    End Class
End Namespace
