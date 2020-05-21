using System.ComponentModel.DataAnnotations;

namespace RabbitMQOrdering.Api.Helpers
{
    public class  RabbitMQConfigurations
    {
        public string HostName { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string  VirtualHost { get; set; }
        public int ManagementPort { get; set; }
    }

    public class Contador
    {
        private int _valorAtual = 0;

        public int ValorAtual { get => _valorAtual; }

        public void Incrementar()
        {
            _valorAtual++;
        }
    }

    public class Conteudo
    {
        [Required]
        public string Mensagem { get; set; }
    }
    public class Resultado
    {
    }
}