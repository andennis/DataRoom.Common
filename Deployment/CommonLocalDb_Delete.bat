"%ProgramFiles%\Microsoft SQL Server\130\Tools\Binn\SqlLocalDB.exe" stop "CommonLocalDb"
"%ProgramFiles%\Microsoft SQL Server\130\Tools\Binn\SqlLocalDB.exe" delete "CommonLocalDb"
RD /S /Q "%userprofile%\appdata\local\Microsoft\Microsoft SQL Server Local DB\Instances\CommonLocalDb"