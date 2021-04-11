#Requires -RunAsAdministrator

param(
    [string] $siteName = "mfepoc.rgbco.uk"
)

$ErrorActionPreference = "Stop"

Import-Module WebAdministration
Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force

function SetupSite($siteName) {

    $sitePool = Get-IISAppPool -Name $siteName
    if ($sitePool -eq $null) {
        Write-Host "Creating IIS pool $siteName"
        New-WebAppPool -Name $siteName
    }


    $site = Get-IISSite -Name $siteName
    if ($site -eq $null) {
        Write-Host "Creating IIS site $siteName"
        $site = New-IISSite -Name $siteName -PhysicalPath . -BindingInformation "*:80:$siteName"
    }

    Set-ItemProperty "IIS:\Sites\$siteName" -Name "ApplicationPool" -Value $siteName

    Write-Host $site
}

#Reset-IISServerManager -Confirm:$false

SetupSite $siteName

