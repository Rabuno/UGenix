$root = "C:\Users\Rabuno\Documents\AHihi\TestWebsite"
$backendSrc = Join-Path $root "ugenix-backend\src"

$projects = @(
    "API",
    "Application",
    "Contracts",
    "Domain",
    "Infrastructure",
    "Persistence",
    "Shared"
)

foreach ($proj in $projects) {
    $oldDir = Join-Path $backendSrc "UGenix.$proj"
    $newDir = Join-Path $backendSrc "UGenix.$proj"
    
    if (Test-Path $oldDir) {
        Move-Item $oldDir $newDir -Force
        Write-Host "Renamed directory: $oldDir -> $newDir"
        
        # Rename csproj
        $oldCsproj = Join-Path $newDir "UGenix.$proj.csproj"
        $newCsproj = Join-Path $newDir "UGenix.$proj.csproj"
        if (Test-Path $oldCsproj) {
            Move-Item $oldCsproj $newCsproj -Force
            Write-Host "Renamed file: $oldCsproj -> $newCsproj"
        }
        
        # Rename http for API
        if ($proj -eq "API") {
            $oldHttp = Join-Path $newDir "UGenix.API.http"
            $newHttp = Join-Path $newDir "UGenix.API.http"
            if (Test-Path $oldHttp) {
                Move-Item $oldHttp $newHttp -Force
                Write-Host "Renamed file: $oldHttp -> $newHttp"
            }
        }
    }
}

# Rename Solution file
$oldSln = Join-Path $root "ugenix-backend\UGenix.sln"
$newSln = Join-Path $root "ugenix-backend\UGenix.sln"
if (Test-Path $oldSln) {
    Move-Item $oldSln $newSln -Force
    Write-Host "Renamed file: $oldSln -> $newSln"
}

# Rename Top level directories
$oldBackend = Join-Path $root "ugenix-backend"
$newBackend = Join-Path $root "ugenix-backend"
Move-Item $oldBackend $newBackend -Force
Write-Host "Renamed directory: $oldBackend -> $newBackend"

$oldFrontend = Join-Path $root "ugenix-frontend"
$newFrontend = Join-Path $root "ugenix-frontend"
Move-Item $oldFrontend $newFrontend -Force
Write-Host "Renamed directory: $oldFrontend -> $newFrontend"

