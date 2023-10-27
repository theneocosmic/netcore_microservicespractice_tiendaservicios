using TiendaServicios.Mensajeria.Email.NetEmail;
using TiendaServicios.RabbitMQ.Bus.BusRabbit;
using TiendaServicios.RabbitMQ.Bus.EventoQueue;

namespace TiendaServicios.Api.Autor.ManejadorRabbit
{
	public class EmailEventoManejador : IEventoManejador<EmailEventoQueue>
	{
		private readonly ILogger<EmailEventoManejador> _logger;
		private readonly IEmailService _email;

		public EmailEventoManejador() { }

		public EmailEventoManejador(ILogger<EmailEventoManejador> logger, IEmailService email)
		{
			_logger = logger;
			_email = email;
		}

		public async Task Handle(EmailEventoQueue @event)
		{
			_logger.LogInformation(@event.Titulo);
			string destinationEmail = "here@someemail.com";
			await _email.SendEmailAsync(destinationEmail, @event.Titulo, "Test " + @event.Titulo);
		}
	}
}
