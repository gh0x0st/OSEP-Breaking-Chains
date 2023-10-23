Function Invoke-LSARecon {
    try {
        $RegistryPath = "HKLM:\SYSTEM\CurrentControlSet\Control\Lsa"
        $LSA = Get-ItemProperty -Path $RegistryPath -Name "RunAsPPL" -ErrorAction 0
        $KeyValue = $LSA.RunAsPPL

        switch ($KeyValue) {
            0 { Write-Output "RunAsPPL is Disabled" }
            1 { Write-Output "RunAsPPL is Enabled" }
            2 { Write-Output "RunAsPPL is Enabled" }
            $null { Write-Output "The 'RunAsPPL' registry key does not exist" }
            default { Write-Output "The 'RunAsPPL' registry key value is unexpected: $KeyValue" }
        }
    }
    catch {
        Write-Output "Error: $_"
    }
}
