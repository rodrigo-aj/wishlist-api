using dockerapi.Models;
using dockerapi.Repository;
using dockerapi.Service;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace dockerapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishListController : ControllerBase
    {

        private readonly WishListRepository wishListRepository;
        private static IHostingEnvironment environment;

        public WishListController(WishListRepository _wishListRepository, IHostingEnvironment _environment)
        {
            wishListRepository = _wishListRepository;
            environment = _environment;
        }

        /// <summary>
        /// Retorna todos as wishLists cadastradas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<WishList> Get()
        {
            return wishListRepository.GetAll();
        }

        /// <summary>
        /// Retorna um item a partir do ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public WishList GetById(long id)
        {
            return wishListRepository.GetById(id);
        }

        /// <summary>
        /// Retorna um item de randomicamente
        /// </summary>
        /// <returns></returns>
        [HttpGet("random")]
        public WishList GetRadom()
        {
            return wishListRepository.GetRandom();
        }

        /// <summary>
        /// Cadastra uma wishList
        /// </summary>
        /// <param name="wishlist">WishList</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] WishList wishlist)
        {
            Response retorno = new Response();

            try
            {
                var lista = wishListRepository.Save(wishlist);

                retorno.status = 200;
                retorno.mensagem = "Lista adicionada com sucesso";
                retorno.objeto = lista;

                return StatusCode(200, retorno);
            }
            catch (Exception ex)
            {
                retorno.status = 500;
                retorno.mensagem = "Não foi possível realizar operação: " + ex.Message; ;
                retorno.objeto = null;

                return StatusCode(500, retorno);
            }
        }

        /// <summary>
        /// Marca um item como ganho/comprado
        /// </summary>
        /// <param name="id">ID da wishList</param>
        /// <returns></returns>
        [HttpPost("{id}/itemGanhoOuAdquirido")]
        public IActionResult PostGanhouItemWishList(long id)
        {
            Response retorno = new Response();

            try
            {
                var lista = wishListRepository.marcouItemComoGanho(id);

                if (lista == null)
                {
                    retorno.status = 500;
                    retorno.mensagem = "Lista já foi informada como ganha/adquirida.";
                    retorno.objeto = null;

                    return StatusCode(500, retorno);
                }
                else
                {
                    retorno.status = 200;
                    retorno.mensagem = "Lista marcada como ganha/adquirida";
                    retorno.objeto = lista;

                    return StatusCode(200, retorno);
                }
            }
            catch (Exception ex)
            {
                retorno.status = 500;
                retorno.mensagem = "Não foi possível realizar operação: " + ex.Message; ;
                retorno.objeto = null;

                return StatusCode(500, retorno);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost("{id}/uploadImagem")]
        public IActionResult UploadImagem(long id)
        {
            WishListService wishListService = new WishListService(wishListRepository, environment);
            string[] tiposArquivosPermitidos = { ".jpeg", ".png", ".jpg", ".bmp", ".tif", ".tiff" };

            var arquivo = Request.Form.Files;

            Response retorno = new Response();

            try
            {

                if (arquivo.Count() > 1)
                {
                    retorno.status = 500;
                    retorno.mensagem = "Só é possível adicionar uma foto";
                    retorno.objeto = null;

                    return StatusCode(500, retorno);
                }

                if (arquivo.Count() == 0)
                {
                    retorno.status = 500;
                    retorno.mensagem = "É necessário adicionar uma foto para continuar";
                    retorno.objeto = null;

                    return StatusCode(500, retorno);
                }

                string extensaoArquivo = System.IO.Path.GetExtension(arquivo.FirstOrDefault().FileName);

                if (!tiposArquivosPermitidos.Contains(extensaoArquivo))
                {
                    retorno.status = 500;
                    retorno.mensagem = "Formato " + extensaoArquivo + " não permitido. Os formtados permitidos são: png, jpeg, jpg, bmp, tif, tiff!";
                    retorno.objeto = null;

                    return StatusCode(500, retorno);
                }

                var upload = wishListService.uploadImagem(arquivo.FirstOrDefault(), id);

                if (upload == null)
                {
                    retorno.status = 500;
                    retorno.mensagem = "Não foi possível realizar operação!";
                    retorno.objeto = null;

                    return StatusCode(500, retorno);
                }

                retorno.status = 200;
                retorno.mensagem = "Upload realizado com sucesso.";
                retorno.objeto = upload.objeto; ;

                return StatusCode(200, retorno);
            }
            catch (Exception ex)
            {
                retorno.status = 500;
                retorno.mensagem = "Não foi possível realizar operação: " + ex.Message; ;
                retorno.objeto = null;

                return StatusCode(500, retorno);
            }
        }
    }
}