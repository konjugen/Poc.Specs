@ECHO off

set /p tag=Specified Tag:

if Not Exist packages\SpecFlow.2.1.0\tools\specflow.exe ".nuget/nuget.exe" restore

packages\SpecFlow.2.1.0\tools\specflow.exe generateAll Specs\Specs.csproj /force /verbose

call %WINDIR%\Microsoft.NET\Framework\v4.0.30319\msbuild Specs.sln /t:Build /p:Configuration=Debug /p:Platform="Any CPU"

if Exist TestResult.txt del TestResults/TestResult.txt 
if Exist TestResult.xml del TestResults/TestResult.xml 
if Exist TestResult.html del TestResults/TestResult.html 

"packages\NUnit.Runners.2.6.4\tools\nunit-console.exe" /labels /out=TestResults/TestResult.txt /xml=TestResults/TestResult.xml Specs\bin\Debug\Specs.dll /include:%tag%

"packages\SpecFlow.2.1.0\tools\specflow.exe" nunitexecutionreport Specs\Specs.csproj /xmlTestResult:TestResults/TestResult.xml /testOutput:TestResults/TestResult.txt /out:TestResults/TestResult.html 

if Exist TestResults/TestResult.html start TestResults/TestResult.html
if Not Exist TestResults/TestResult.html (set /p v=Something went wrong. Press any key to continue . . .)