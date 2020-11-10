HttpHeaderCharScanner

httpclient Request headers must contain only ASCII characters

- https://github.com/dotnet/runtime/issues/37386

- https://github.com/dotnet/runtime/issues/37024

I would say that it is safe to use any character from 32 to 126 in a http header. I had some issue with postman and the character 127 ⌂
