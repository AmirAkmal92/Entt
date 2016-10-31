& "C:\Program Files (x86)\Windows Kits\8.1\bin\x86\makecert.exe" -r -pe -n "CN=ERYMUZUAN-WS27" -b 01/01/2000 -e 01/01/2036 -eku 1.3.6.1.5.5.7.3.1 -ss my -sr localMachine -sky exchange -sp "Microsoft RSA SChannel Cryptographic Provider" -sy 12

netsh http add urlacl url=https://localhost:50433/ user=everyone
netsh http delete urlacl url=https://localhost:50433/