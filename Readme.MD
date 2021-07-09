Todas as respostas HTTP / 1.1 devem definir Content-Length ou Transfer-Encoding: chunked. O padrão do ASP.NET Core é fragmentado se você não definir o comprimento do conteúdo. Isso permite que as respostas sejam enviadas imediatamente sem buffer e ajuda a minimizar a latência, sobrecarga de memória, etc.

Porém em alguns casos precisamos que a resposta não tenha o cabeçalho Transfer-Encoding: chunked e a única forma de fazer isso é adicionando o lenth ao Header.

Para isso é necessário um Middleware que lê a Response, identifica o tamanho e seta o Content-Length no Header.