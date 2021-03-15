using dockerapi.Models;
using dockerapi.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dockerapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioRespository usuarioRespository;

        public UsuarioController(UsuarioRespository _usuarioRespository, ApiDbContext context)
        {
            usuarioRespository = _usuarioRespository;
        }

        /// <summary>
        /// Retorna todos os usuários cadastrados
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Usuario> Get()
        {
            return usuarioRespository.GetAll(); ;
        }

        /// <summary>
        /// Retorna um usuário a partir do ID
        /// </summary>
        /// <param name="id">ID do usuário</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public Usuario GetById(long id)
        {
            return usuarioRespository.GetById(id);
        }

        /// <summary>
        /// Cadastra um usuário
        /// </summary>
        /// <param name="usuario">Informações do usuário</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody] Usuario usuario)
        {
            Response retorno = new Response();

            try
            {
                var user = usuarioRespository.Save(usuario);

                retorno.status = 200;
                retorno.mensagem = "Usuario criado com sucesso!";
                retorno.objeto = user;

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

