bbdiese: src/token.cs src/parser.cs src/tag.cs src/tag_handlers.cs src/AssemblyInfo.cs
	gmcs $? -v -t:library -O:all -out:bin/$@.dll
	gmcs src/test_application.cs -r:bin/bbdiese.dll -t:exe -O:all -v -out:bin/test.exe

# I suck at writing Makefiles
test: src/AssemblyInfo.cs src/tests/parser.cs
	gmcs $? -v -t:library -O:all -r:nunit.framework.dll,bin/bbdiese.dll -out:bin/bbdiese_tests.dll

gendarme: bbdiese
	gendarme --severity all --confidence all --quiet --html $?.dll.html bin/$?.dll

shell: bbdiese
	csharp -r:bin/$?.dll

clean:
	-rm bin/*.mdb bin/*.dll bin/*.exe *.html
