﻿Imports Discord.Commands
Imports Discord.WebSocket
Imports System.Net
Imports System.Net.Sockets
Imports System.Text

Public Class Help

    Inherits ModuleBase(Of SocketCommandContext)
    'In order to trigger a command, the user must type the prefix (|>) expected by the command handler, followed by the command name or alias.

    <Command("help")>
    <Summary("How to use this bot")>
    Public Function HelpAsync() As Task
        Return ReplyAsync("This bot takes the following commands: wiki, page, code, mentions")
    End Function

    <Command("wiki")>
    <Summary("Looks up a link for pages on the keeperRL wiki")>
    Public Function WikiAsync(search As String) As Task
        Return ReplyAsync(GetPages(search, False, False))
    End Function

    <Command("page")>
    <Summary("Finds github page for the keeperRL wiki")>
    Public Function PageAsync(search As String) As Task
        Return ReplyAsync(GetPages(search, False, True))
    End Function

    <Command("code")>
    <Summary("Looks up a code file in the keeperRL source")>
    Public Function CodeAsync(search As String) As Task
        Return ReplyAsync(GetCode(search))
    End Function

    <Command("mention")>
    <Summary("Looks up a pages that mention a text on the keeperRL wiki")>
    Public Function MentionAsync(search As String) As Task
        Return ReplyAsync(GetPages(search, True, False))
    End Function

    Private Function GetPages(search As String, mentions As Boolean, Source As Boolean)
        Dim dir As String = "C:\PRJ\keeperrl_wiki\"
        Dim ret As String = ""
        Dim lst As New List(Of String)
        Dim lst2 As New List(Of String)
        Dim URL As String
        If Source Then
            URL = "https://github.com/miki151/keeperrl_wiki/blob/master/"
        Else
            URL = "https://miki151.github.io/keeperrl_wiki/"
        End If
        For Each fil As String In IO.Directory.GetFiles(dir, "*.md", IO.SearchOption.AllDirectories)
            Dim filDir As String = IO.Path.GetDirectoryName(fil)
            If filDir.EndsWith("Missing") Then Continue For
            If filDir.EndsWith("Idea") Then Continue For
            If filDir.EndsWith("Unfinished") Then Continue For
            If filDir.EndsWith("Modding") Then Continue For
            Dim filName As String = IO.Path.GetFileNameWithoutExtension(fil)
            Dim filText As String = IO.File.ReadAllText(fil)
            If UCase(filName).Contains(UCase(search)) Then
                If Source Then
                    lst.Add(URL + Replace(Replace(fil, dir, ""), "\", "/") + vbCrLf)
                Else
                    lst.Add(URL + filName + vbCrLf)
                End If
            End If
            If UCase(filText).Contains(UCase(search)) Then
                If Source Then
                    lst2.Add(URL + Replace(Replace(fil, dir, ""), "\", "/") + vbCrLf)
                Else
                    lst2.Add(URL + filName + vbCrLf)
                End If
            End If
        Next
        If lst2.Count = 0 Then Return "Wiki pages not found."
        ret = "Wiki pages (page names): " + vbCrLf
        For Each str As String In lst
            ret = ret + str
        Next
        If mentions Then
            ret = ret + vbCrLf + "Wiki pages (mentions): " + vbCrLf
            For Each str As String In lst2
                ret = ret + str
            Next
        End If
        If ret.Length > 2000 Then ret = Left(ret, 1950) + "..."
        Return ret
    End Function

    Private Function GetCode(search As String)
        Dim dir As String = "C:\PRJ\keeperrl\"
        Dim ret As String = ""
        Dim lst As New List(Of String)
        Dim URL As String
        URL = "https://github.com/miki151/keeperrl/blob/master/"
        For Each fil As String In IO.Directory.GetFiles(dir, "*.*", IO.SearchOption.AllDirectories)
            Dim filDir As String = IO.Path.GetDirectoryName(fil)
            Dim filName As String = IO.Path.GetFileNameWithoutExtension(fil)
            If UCase(filName).Contains(UCase(search)) Then
                lst.Add(URL + Replace(Replace(fil, dir, ""), "\", "/") + vbCrLf)
            End If
        Next
        ret = "KeeperRL source code (file names): " + vbCrLf
        For Each str As String In lst
            ret = ret + str
        Next
        If ret.Length > 2000 Then ret = Left(ret, 1950) + "..."
        Return ret
    End Function


End Class

