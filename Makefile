bbdiese: src/token.cs src/parser.cs src/tag.cs src/AssemblyInfo.cs
	gmcs $? -v -t:library -O:all -out:bin/$@.dll
	gmcs src/test_application.cs -r:bin/bbdiese.dll -t:exe -O:all -v -out:bin/test.exe

gendarme: bbdiese
	gendarme --severity all --confidence all --quiet --html $?.dll.html bin/$?.dll

shell: bbdiese
	csharp -r:bin/$?.dll

clean:
	-rm bin/*.mdb bin/*.dll bin/*.exe *.html
