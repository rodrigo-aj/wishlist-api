using dockerapi.Models;
using dockerapi.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace dockerapi.Service
{
    public class WishListService
    {
        private readonly WishListRepository wishListRepository;
        private static IHostingEnvironment environment;

        public WishListService(WishListRepository _wishListRepository, IHostingEnvironment _environment)
        {
            wishListRepository = _wishListRepository;
            environment = _environment;
        }


        public Response uploadImagem(IFormFile arquivo, long id)
        {

            Response retorno = new Response();

            string[] tiposArquivosPermitidos = { "jpeg", "png" };

            try
            {
                if (!Directory.Exists(environment.WebRootPath + "\\imagens\\"))
                {
                    Directory.CreateDirectory(environment.WebRootPath + "\\imagens\\");
                }

                using (FileStream filestream = System.IO.File.Create(environment.WebRootPath + "\\imagens\\" + arquivo.FileName))
                {
                    arquivo.CopyTo(filestream);
                    filestream.Flush();
                }

                var wishList = wishListRepository.GetById(id);

                wishList.fotoProduto = environment.WebRootPath + "\\imagens\\" + arquivo.FileName;

                wishListRepository.alterarRegistro(wishList);

                retorno.objeto = wishList;

                return retorno;
            }
            catch (Exception ex)
            {
                retorno.status = 500;
                retorno.mensagem = "Não foi possível realizar operação: " + ex.Message; ;
                retorno.objeto = null;

                return retorno;
            }
        }
    }
}
