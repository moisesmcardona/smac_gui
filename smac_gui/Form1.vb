Public Class Form1
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim CompressThread As New System.Threading.Thread(Sub() Run_smac("", "", "/c"))
        CompressThread.Start
    End Sub
     Private Sub Run_smac(Input As String, Output As String, action As String)
        Dim smacProcessInfo As New ProcessStartInfo
        Dim smacProcess As Process
        smacProcessInfo.FileName = "SMAC.EXE"
        smacProcessInfo.Arguments = action + " """ & Input & """"
        smacProcessInfo.CreateNoWindow = True
        smacProcessInfo.RedirectStandardOutput = True
        smacProcessInfo.UseShellExecute = False
        smacProcess = Process.Start(smacProcessInfo)
        Dim smacProcessOutput As String = String.Empty
        Dim currentOutput As String = String.Empty
        While smacProcess.HasExited = False
            While smacProcess.StandardOutput.EndOfStream = False
                currentOutput = smacProcess.StandardOutput.ReadLine & vbCrLF
                If smacProcessOutput.Contains(currentOutput) = False Then
                    If currentOutput.Contains("Elapsed time") Then
                        Exit While
                    End If
                    smacProcessOutput = smacProcessOutput + currentOutput
                    UpdateLog(currentOutput)
                End If
            End While
            If smacProcessOutput.Contains("Finished!") Then
                smacProcess.Kill
            End If
        End While

    End Sub

    Private Delegate Sub UpdateLogInvoker(message As String)
    Private Sub UpdateLog(message As string)
        If RichTextBox1.Invokerequired Then
            RichTextBox1.Invoke(New UpdateLogInvoker(AddressOf UpdateLog), message)
         Else
            RichTextBox1.Text = RichTextBox1.Text + message
        End If
    End Sub
End Class
