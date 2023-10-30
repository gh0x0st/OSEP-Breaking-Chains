function Parse-XML {
    param (
        [Parameter(Mandatory = $true)]
        [string]$XmlString
    )
    begin {
        [xml]$xml = $XmlString
    }
    process {
        if ($xml.DocumentElement -eq 'FilePublisherRule') {
            $Parsed = $xml.FilePublisherRule
        }
        elseif ($xml.DocumentElement -eq 'FilePathRule') {
            $Parsed = $xml.FilePathRule
        }
        else {
            $Parsed = $xml.DocumentElement
        }       
    }
    end {
        return $Parsed
    }
}

function Invoke-ApplockerRecon {

    # Check if applocker will apply to the current user context
    $CurrentUser = [System.Security.Principal.WindowsIdentity]::GetCurrent().Name
    $BuiltIn = @("NT AUTHORITY\SYSTEM", "NT AUTHORITY\LOCAL SERVICE", "NT AUTHORITY\NETWORK SERVICE", "IIS APPPOOL\DefaultAppPool")
    if ($CurrentUser -in $BuiltIn) {
        Write-Output "Applocker policies do not apply to $CurrentUser"
    } 

    # Parse through available rules
    $Rules = Get-ChildItem -Path "HKLM:\SOFTWARE\Policies\Microsoft\Windows\SrpV2" -ErrorAction 0
    If ($Rules) {
        foreach ($ID in $Rules) {
            $RuleName = $($ID.PSChildName)
            switch ($(Get-ItemProperty -Path "$($ID.PSPath)" -Name "EnforcementMode" -ErrorAction SilentlyContinue).EnforcementMode) {
                1 { $Enforcement = 'Enforced' }
                0 { $Enforcement = 'Not Enforced' }
                $Null { $Enforcement = 'Not Configured' }
            }
    
            $Actions = Get-ChildItem -Path "$($ID.PSPath)" -ErrorAction SilentlyContinue
            foreach ($Action in $Actions) {
                $XML = Get-ItemProperty -Path "$($Action.PSPath)" -Name "Value" -ErrorAction SilentlyContinue
                Parse-XML -XmlString $XML.Value | Select @{l = 'Rule'; e = { $RuleName } }, @{l = 'Enforcement'; e = { $Enforcement } }, ID, Name, Description, UserOrGroupSid, Action, @{l = 'Conditions'; e = { $_.Conditions.FilePathCondition.Path } }
            }
        }
    }
    Else {
        Write-Output 'Applocker is not configured'
    }
}
