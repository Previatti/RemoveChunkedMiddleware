using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace RemoveChunked.Middleware
{
    public class RemoveChunkedMiddleware
    {
        private readonly RequestDelegate _next;
        public RemoveChunkedMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context)
        {
            //recebe o body original da response
            var originalBody = context.Response.Body;

            // cria um objeto do tipo MemoryStream
            using (var ms = new MemoryStream())
            {
                context.Response.Body = ms;
                long length = 0;

                // fica aguardando o start do response, para poder gravar o tamanho da resposta no header
                context.Response.OnStarting((state) =>
                {
                    context.Response.Headers.ContentLength = length;
                    return Task.FromResult(0);
                }, context);

                await _next(context);

                // lê o tamanho do body, e quando isso acontece, o valor é setado no header
                length = context.Response.Body.Length;

                //é necessário setar o position = 0 para poder devolver o body que já foi lido no início do método
                context.Response.Body.Position = 0;

                //seta o originalBody para o ms, que vai registrar novamente o body na request
                await ms.CopyToAsync(originalBody);
            }
        }


    }
}
