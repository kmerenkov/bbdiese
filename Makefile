bbdiese: token.cs parser.cs tag.cs AssemblyInfo.cs
	gmcs $? -v -t:library -O:all -out:$@.dll

test: bbdiese
	gmcs test.cs -r:bbdiese.dll -t:exe -O:all -v -out:test.exe

gendarme: bbdiese
	gendarme --severity all --confidence all --quiet --html $?.dll.html $?.dll

shell: bbdiese
	csharp -r:$?.dll

clean:
	-rm *.mdb *.dll *.exe *.html
