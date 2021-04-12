#Requires -RunAsAdministrator

param(
    [string] $siteName = "mfepoc.rgbco.uk"
)

$ErrorActionPreference = "Stop"

Import-Module WebAdministration
Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force

function Unzip($folder) {
    $zipFile = "$folder\publish.zip"
    if (Test-Path $zipFile) {
        Write-Host "unziping $zipFile"
        Expand-Archive -Path $zipFile -DestinationPath $folder -Force
        Rename-Item -Path $zipFile -NewName "publish_unzipped.zip"
    }
}

function SetupSite($siteName, $path) {

    $sitePool = Get-IISAppPool -Name $siteName
    if ($sitePool -eq $null) {
        Write-Host "Creating IIS pool $siteName"
        Reset-IISServerManager -Confirm:$False
        New-WebAppPool -Name $siteName
    }

    $site = Get-IISSite -Name $siteName
    if ($site -eq $null) {
        Write-Host "Creating IIS site $siteName"
        Reset-IISServerManager -Confirm:$False
        New-IISSite -Name $siteName -PhysicalPath $path -BindingInformation "*:80:$siteName"
    }

    Set-ItemProperty "IIS:\Sites\$siteName" -Name "PhysicalPath" -Value $path
    Set-ItemProperty "IIS:\Sites\$siteName" -Name "ApplicationPool" -Value $siteName
    Set-ItemProperty "IIS:\AppPools\$siteName" -Name recycling.disallowOverlappingRotation -Value True
}

function SetupApp($siteName, $appName, $path) {

    $poolName = "$siteName.$appName"
    Write-Host $poolName
    $pool = Get-IISAppPool -Name $poolName
    if ($pool -eq $null) {
        Write-Host "Creating IIS pool $poolName"
        Reset-IISServerManager -Confirm:$False
        New-WebAppPool -Name $poolName
    }

    Set-ItemProperty "IIS:\AppPools\$poolName" -Name recycling.disallowOverlappingRotation -Value True
}

Unzip "home"
Unzip "dashboard"
Unzip "blazorss1"
Unzip "blazorss2"
Unzip "blazorcs1"
Unzip "blazorcs2"

Reset-IISServerManager -Confirm:$False
SetupSite $siteName "$((Get-Location).Path)\home"
SetupApp $siteName "Dashboard" "$((Get-Location).Path)\dashboard"
