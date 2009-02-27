bbdiese: src/token.cs src/parser.cs src/tag.cs src/bbcode.cs src/tag_handlers.cs src/AssemblyInfo.cs src/tag_handlers/simple.cs src/tag_handlers/link.cs src/tag_handlers/image.cs src/tag_handlers/color.cs src/tag_handlers/quote.cs
	gmcs $? -v -t:library -O:all -r:System.Web.dll -out:bin/$@.dll
	gmcs src/test_application.cs -r:bin/bbdiese.dll -t:exe -O:all -v -out:bin/test.exe

all: clean bbdiese test doc

# I suck at writing Makefiles
test: src/AssemblyInfo.cs src/tests/parser.cs src/tests/tag_handlers.cs src/tests/smileys.cs
	gmcs $? -v -t:library -O:all -r:nunit.framework.dll,bin/bbdiese.dll -out:bin/bbdiese_tests.dll

doc: bbdiese
	-rm -r doc/*
	ndoc -documenter=LinearHtml bin/bbdiese.dll

gendarme: bbdiese
	gendarme --severity all --confidence all --quiet --html $?.dll.html bin/$?.dll

shell: bbdiese
	csharp -r:bin/$?.dll

clean:
	-rm bin/*.mdb bin/*.dll bin/*.exe *.html
