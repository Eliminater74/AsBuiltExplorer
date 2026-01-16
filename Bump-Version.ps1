param (
    [Parameter(Mandatory = $true)]
    [string]$Version
)

$ErrorActionPreference = "Stop"

# Define file paths
$csprojPath = ".\AsBuiltExplorer.csproj"
$assemblyInfoPath = ".\AssemblyInfo.cs"
$issPath = ".\setup.iss"

Write-Host "Bump-Version: Setting version to $Version..." -ForegroundColor Cyan

# 1. Update AsBuiltExplorer.csproj
if (Test-Path $csprojPath) {
    Write-Host "Updating $csprojPath..."
    $content = Get-Content $csprojPath -Raw
    $newContent = $content -replace "<ApplicationVersion>.*?</ApplicationVersion>", "<ApplicationVersion>$Version</ApplicationVersion>"
    Set-Content -Path $csprojPath -Value $newContent -Encoding UTF8
}
else {
    Write-Warning "$csprojPath not found."
}

# 2. Update AssemblyInfo.cs
if (Test-Path $assemblyInfoPath) {
    Write-Host "Updating $assemblyInfoPath..."
    $content = Get-Content $assemblyInfoPath -Raw
    $newContent = $content -replace '\[assembly: AssemblyFileVersion\(".*?"\)\]', "[assembly: AssemblyFileVersion(`"$Version`")]"
    $newContent = $newContent -replace '\[assembly: AssemblyVersion\(".*?"\)\]', "[assembly: AssemblyVersion(`"$Version`")]"
    Set-Content -Path $assemblyInfoPath -Value $newContent -Encoding UTF8
}
else {
    Write-Warning "$assemblyInfoPath not found."
}

# 3. Update setup.iss
if (Test-Path $issPath) {
    Write-Host "Updating $issPath..."
    $content = Get-Content $issPath -Raw
    # Regex matches: #define MyAppVersion "..."
    $newContent = $content -replace '#define MyAppVersion ".*?"', "#define MyAppVersion `"$Version`""
    Set-Content -Path $issPath -Value $newContent -Encoding UTF8
}
else {
    Write-Warning "$issPath not found."
}

# 4. Git Operations
Write-Host "Performing Git operations..." -ForegroundColor Cyan

# Function to find Git
function Get-GitPath {
    if (Get-Command git -ErrorAction SilentlyContinue) {
        return "git"
    }
    
    $commonPaths = @(
        "C:\Program Files\Git\cmd\git.exe",
        "C:\Program Files\Git\bin\git.exe",
        "C:\Program Files (x86)\Git\cmd\git.exe",
        "C:\Program Files (x86)\Git\bin\git.exe",
        "C:\Program Files\Microsoft Visual Studio\18\Insiders\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Git\cmd\git.exe",
        "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Git\cmd\git.exe",
        "C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Git\cmd\git.exe",
        "C:\Program Files\Microsoft Visual Studio\2022\Enterprise\Common7\IDE\CommonExtensions\Microsoft\TeamFoundation\Team Explorer\Git\cmd\git.exe"
    )
    
    foreach ($path in $commonPaths) {
        if (Test-Path $path) {
            return $path
        }
    }
    
    return $null
}

$gitPath = Get-GitPath

if ($gitPath) {
    Write-Host "Using Git at: $gitPath" -ForegroundColor Gray
    
    & $gitPath add $csprojPath $assemblyInfoPath $issPath
    & $gitPath commit -m "Bump version to v$Version"
    
    $tagName = "v$Version"
    # Check if tag exists
    $tagExists = & $gitPath tag -l $tagName
    if ($tagExists) {
        Write-Warning "Tag $tagName already exists. Skipping tag creation."
    }
    else {
        & $gitPath tag $tagName
        Write-Host "Created tag: $tagName" -ForegroundColor Green
    }

    Write-Host "Version bumped to $Version successfully!" -ForegroundColor Green
    Write-Host "Don't forget to push: git push && git push --tags" -ForegroundColor Yellow
}
else {
    Write-Error "Git command not found in PATH or common locations. Changes modified but not committed/tagged."
}
