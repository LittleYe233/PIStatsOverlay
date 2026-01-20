param(
    [Parameter(Mandatory=$true)]
    [string]$configuration
)

# Change direcotry to project root
Set-Location "$PSScriptRoot\.."

$gitSha = git rev-parse --short HEAD

$jsonPath = "Statics\Info.json"
$version = (Get-Content $jsonPath -Raw | ConvertFrom-Json).Version

$destPath = "bin\$configuration\PIStatsOverlay-$configuration-$version-$gitSha.zip"

$files = @(
    "bin\$configuration\PIStatsOverlay.dll",
    "bin\$configuration\I18N.DotNet.dll",
    $jsonPath
)
Compress-Archive -Path $files -DestinationPath $destPath -Force
