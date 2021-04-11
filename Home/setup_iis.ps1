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


    $site = Get-Website | Where-Object { $_.Name -eq $siteName }
    if ($site -eq $null) {
        Write-Host "Creating IIS site $siteName"
        $path = Get-Location
        New-IISSite -Name $siteName -PhysicalPath $path -BindingInformation "*:80:$siteName"
    }

    Set-ItemProperty "IIS:\Sites\$siteName" -Name "ApplicationPool" -Value $siteName
}

Reset-IISServerManager -Confirm:$False

SetupSite $siteName

