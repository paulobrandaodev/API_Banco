using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using api_banco.Models;
using api_banco.DataBase;
using Microsoft.AspNetCore.Mvc;

namespace api_banco.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CadastroController : ControllerBase
    {

        // 1 - Instanciamos nossa classe de Conexão
        Conexao conexao = new Conexao(); 

        // 2 - Chamamos nosso objeto que dará os comandos SQL
        SqlCommand cmd = new SqlCommand();

        // 3 - Criamos uam String para guardar a msg de erro ou sucesso
        public string mensagem = "";

        // 4 - Referenciamos o nosso CadastroModel para pegar os atributos
        CadastroModel cadastroModel = new CadastroModel();

        
        // Trocamos o tipo de Retorno para uma Lista de Cadastro
        // GET api/values
        [HttpGet]
        public List<CadastroModel> Get(){            

                // 4 -  Conecto com o banco
                cmd.Connection = conexao.Conectar();

                // 5 - Preparo minha Query 
                cmd.CommandText = "SELECT * FROM cadastro";

                // 6 - Executo o comando para ler
                SqlDataReader dados = cmd.ExecuteReader();
                
                // 7 - Crio uma lista para exibir meus cadastros
                List<CadastroModel> cadastros = new List<CadastroModel>();

                while(dados.Read()){
                    cadastros.Add( 
                        new CadastroModel() { 
                            IdCadastro = Convert.ToInt32(dados.GetValue(0)),
                            Nome =dados.GetValue(1).ToString(),
                            Email =dados.GetValue(2).ToString(),
                        }                        
                    );
                }

                // 7 - Desconectar
                conexao.Desconectar(); 

            return cadastros;

        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/CadastroModel
        [HttpPost]
        public string Post([FromBody] CadastroModel dado)
        {   

            try
            {
                // 4 -  Conecto com o banco
                cmd.Connection = conexao.Conectar();

                // 5 - Preparo minha Query 
                cmd.CommandText = "INSERT INTO cadastro (Nome, Email) VALUES (@nome, @email)";
                cmd.Parameters.AddWithValue("@nome" , dado.Nome);
                cmd.Parameters.AddWithValue("@email", dado.Email);

                // 6 - Executo o comando
                cmd.ExecuteNonQuery();

                // 7 - Desconectar
                conexao.Desconectar();

                // 8 - Mostrar MSG de sucesso
                this.mensagem = "Cadastrado com sucesso!";


            }
            catch (System.Exception ex)
            {                
                this.mensagem = "Erro ao tentar se conectar com o banco de dados: " + ex;
            }

            return this.mensagem;

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
