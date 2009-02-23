bbdiese: token.cs parser.cs tag.cs
	gmcs $? -v -t:library -out:$@.dll

gendarme: bbdiese
	gendarme --severity all --confidence all --quiet --html $?.dll.html $?.dll

shell: bbdiese
	csharp -r:$?.dll

clean:
	-rm *.mdb *.dll *.html
