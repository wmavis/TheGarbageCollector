cd \Users\WillardPC\github\wmavis.github.io\TheGarbageCollector\
del /q /s Build
del /q /s TemplateData 
xcopy /y /s ..\..\TheGarbageCollector\Build\* .
cd \Users\WillardPC\github\TheGarbageCollector\
