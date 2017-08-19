& "C:\Program Files (x86)\MSBuild\14.0\Bin\MSBuild.exe" YoutubeDownloader.sln /p:Configuration=Release /t:Clean,Build
if (-Not (Test-Path -Path "built")) {
    New-Item -ItemType directory -Path "built" | Out-Null
}
& "C:\Program Files (x86)\Microsoft\ILMerge\ILMerge.exe" ./YoutubeDownloader/bin/Release/YoutubeDownloader.exe ./YoutubeDownloader/bin/Release/*.dll /wildcards /out:"built/YoutubeDownloader.exe" /target:winexe /ndebug /lib:"C:\Windows\Microsoft.NET\Framework64\v4.0.30319" /targetplatform:"v4, C:\Windows\Microsoft.NET\Framework\v4.0.30319"
