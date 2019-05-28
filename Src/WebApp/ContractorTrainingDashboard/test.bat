SonarScanner.MSBuild.exe begin /k:"org.sonarqube:sonarqube-scanner-msbuild" /n:"Data Integration APP" /v:"4.2"
MSBuild.exe /t:Rebuild
SonarScanner.MSBuild.exe end