param ($config = "Debug", $SqlPackageVer = 140)

$SqlPackageExe = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Enterprise\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\$SqlPackageVer\SqlPackage.exe"
if (-not (Test-Path $SqlPackageExe)) 
{
	$SqlPackageExe = "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\$SqlPackageVer\SqlPackage.exe"
}

& $SqlPackageExe /a:Publish /sf:"..\Tests\Common.Repository.Dapper.DB\bin\$config\Common.Repository.DB.dacpac" /pr:"..\Tests\Common.Repository.DB\Common.Repository.DB.publish.xml"