param(
    [ValidateSet("2.10", "3.1")]
    [string]$E3DVersion = "2.10",

    [string]$ModuleName = "Design",

    [string]$InstallRoot = "D:\AVEVA\CAF\AutoSaveAddin"
)

$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$configuration = if ($E3DVersion -eq "3.1") { "Release3" } else { "Release" }
$sourceDll = Join-Path $repoRoot "AutoSaveAddin\bin\$configuration\AutoSaveAddin.dll"

if (-not (Test-Path -LiteralPath $sourceDll)) {
    throw "DLL not found: $sourceDll. Build configuration '$configuration' first."
}

$addinDir = Join-Path $InstallRoot "Addins\E3D-$E3DVersion"
$configDir = Join-Path $InstallRoot "Config"
$targetDll = Join-Path $addinDir "AutoSaveAddin.dll"
$addinsXml = Join-Path $configDir "$($ModuleName)Addins.xml"

New-Item -ItemType Directory -Force -Path $addinDir | Out-Null
New-Item -ItemType Directory -Force -Path $configDir | Out-Null
Copy-Item -LiteralPath $sourceDll -Destination $targetDll -Force

$xml = @"
<?xml version="1.0" encoding="utf-8"?>
<ArrayOfString
    xmlns:xsd="http://www.w3.org/2001/XMLSchema"
    xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">
    <string>AutoSaveAddin</string>
</ArrayOfString>
"@

Set-Content -LiteralPath $addinsXml -Value $xml -Encoding UTF8

function Add-UserPathValue {
    param(
        [string]$Name,
        [string]$PathValue,
        [string]$RemovePrefix = $null
    )

    $current = [Environment]::GetEnvironmentVariable($Name, "User")
    $parts = @()

    if (-not [string]::IsNullOrWhiteSpace($current)) {
        $parts = $current.Split(';') | Where-Object { -not [string]::IsNullOrWhiteSpace($_) }
    }

    if (-not [string]::IsNullOrWhiteSpace($RemovePrefix)) {
        $parts = $parts | Where-Object { -not $_.StartsWith($RemovePrefix, [StringComparison]::OrdinalIgnoreCase) }
    }

    if ($parts -notcontains $PathValue) {
        $parts += $PathValue
    }

    $newValue = [string]::Join(";", $parts)
    [Environment]::SetEnvironmentVariable($Name, $newValue, "User")
    [Environment]::SetEnvironmentVariable($Name, $newValue, "Process")
    return $newValue
}

$cafAddinsPath = Add-UserPathValue -Name "CAF_ADDINS_PATH" -PathValue $addinDir -RemovePrefix (Join-Path $InstallRoot "Addins")
$cafUicPath = Add-UserPathValue -Name "CAF_UIC_PATH" -PathValue $configDir

Write-Host "Installed DLL: $targetDll"
Write-Host "Created config: $addinsXml"
Write-Host "CAF_ADDINS_PATH=$cafAddinsPath"
Write-Host "CAF_UIC_PATH=$cafUicPath"
Write-Host "Restart AVEVA E3D after installation."
