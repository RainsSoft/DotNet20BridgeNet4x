echo ilmerge [/lib:directory]* [/log[:filename]] [/keyfile:filename [/delaysign]] [/internalize[:filename]] [/t[arget]:(library|exe|winexe)] [/closed] [/ndebug] [/ver:version] [/copyattrs [/allowMultiple]] [/xmldocs] [/attr:filename] ([/targetplatform:<version>[,<platformdir>]]|v1|v1.1|v2|v4) [/useFullPublicKeyForReferences] [/zeroPeKind] [/wildcards] [/allowDup[:typename]]* [/allowDuplicateResources] [/union] [/align:n] /out:filename <primary assembly> [<other assemblies>...]

ilmerge.exe /?
ilmerge.exe /log:merge1.log /t:library /targetplatform:v2 /ndebug /out:DotNet20BridgeNet4x.Net20.dll DotNet20BridgeNet4x.dll System.Mono.Posix.Net20.dll System.Core.Net20.dll System.Threading.Net20.dll

pause