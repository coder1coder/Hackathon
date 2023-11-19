$files = Get-ChildItem -Path (Get-Location).path -Filter *.csproj -Recurse -ErrorAction SilentlyContinue -Force
$hash = @{}

foreach ($file in $files) {
    Select-Xml -Path $file.FullName -XPath '/Project/ItemGroup/PackageReference' | ForEach-Object { 
        if ($hash.ContainsKey($_.Node.Include)) {
            if ($hash[$_.Node.Include] -lt $_.Node.Version){
                $hash[$_.Node.Include] = $_.Node.Version
            }
        } else {
            $hash.Add($_.Node.Include, $_.Node.Version);
        }
     };
}

$sorted = $hash.GetEnumerator() | Sort-Object -Property Name;

foreach ($row in $sorted){
    Write-Host "<PackageVersion Include=""$($row.key.Trim())"" Version=""$($row.value.Trim())"" />";
}