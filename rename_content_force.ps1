Get-ChildItem -Recurse -File -Force | Where-Object { $_.FullName -notmatch '\\.git|\\node_modules|\\bin|\\obj' } | ForEach-Object {
    $content = Get-Content $_.FullName -Raw
    $newContent = $content.Replace('UGenix', 'UGenix').Replace('ugenix', 'ugenix')
    if ($content -ne $newContent) {
        Set-Content $_.FullName $newContent
        Write-Host "Updated: $($_.FullName)"
    }
}

