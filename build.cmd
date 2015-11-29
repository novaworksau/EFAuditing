@echo off
cd %~dp0

SETLOCAL
SET NUGET_VERSION=latest
SET CACHED_NUGET=%LocalAppData%\NuGet\nuget.%NUGET_VERSION%.exe
SET BUILDCMD_KOREBUILD_VERSION=
SET BUILDCMD_DNX_VERSION=

IF EXIST %CACHED_NUGET% goto copynuget
echo Downloading latest version of NuGet.exe...
IF NOT EXIST %LocalAppData%\NuGet md %LocalAppData%\NuGet
@powershell -NoProfile -ExecutionPolicy unrestricted -Command "$ProgressPreference = 'SilentlyContinue'; Invoke-WebRequest 'https://dist.nuget.org/win-x86-commandline/%NUGET_VERSION%/nuget.exe' -OutFile '%CACHED_NUGET%'"

:copynuget
IF EXIST .nuget\nuget.exe goto restore
md .nuget
copy %CACHED_NUGET% .nuget\nuget.exe > nul

:restore
.nuget\NuGet.exe sources add -Name OconicsCI -Source "https://www.myget.org/F/oconicsci/api/v3/index.json"
IF EXIST packages\Sake goto getdnx
IF "%BUILDCMD_KOREBUILD_VERSION%"=="" (
    .nuget\nuget.exe install OconicsBuild -ExcludeVersion -o packages -nocache -pre
) ELSE (
    .nuget\nuget.exe install OconicsBuild -version %BUILDCMD_KOREBUILD_VERSION% -ExcludeVersion -o packages -nocache -pre
)
.nuget\NuGet.exe install Sake -ExcludeVersion -Source https://www.nuget.org/api/v2/ -Out packages

:getdnx
IF "%BUILDCMD_DNX_VERSION%"=="" (
    SET BUILDCMD_DNX_VERSION=latest
)
IF "%SKIP_DNX_INSTALL%"=="" (
    CALL packages\OconicsBuild\build\dnvm install %BUILDCMD_DNX_VERSION% -runtime CoreCLR -arch x64 -alias default
    CALL packages\OconicsBuild\build\dnvm install default -runtime CLR -arch x64 -alias default
) ELSE (
    CALL packages\OconicsBuild\build\dnvm use default -runtime CLR -arch x64
)

packages\Sake\tools\Sake.exe -I packages\OconicsBuild\build -f makefile.shade %*