$Env:PackageType = 'development'; # default value
if($Env:APPVEYOR_REPO_TAG -eq 'true') { # if there is a repo tag pushed
  if($Env:APPVEYOR_REPO_TAG_NAME -match 'v(\d+(\.)?){1,4}') { #if it matches v{Maj}.{Min}.{Rev}.{Build}
    $Env:PackageType = 'release'; # this is a release package
  }
}
# set the package label
$Env:LABEL = "$Env:PackageType-" + $Env:APPVEYOR_BUILD_NUMBER.PadLeft(5, "0")
Write-Output "Packages will be suffixed with $Env:LABEL" ;
