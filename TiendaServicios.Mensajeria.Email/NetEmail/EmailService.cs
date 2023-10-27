using System.Net;
using System.Net.Mail;

namespace TiendaServicios.Mensajeria.Email.NetEmail
{
	public class EmailService : IEmailService
	{
		public Task SendEmailAsync(string email, string subject, string message)
		{
			var mail = "sender@email.com";
			var password = "passwordSenderEmail";

			var client = new SmtpClient("smtp-mail.outlook.com", 587)
			{
				EnableSsl = true,
				UseDefaultCredentials = false,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				Credentials = new NetworkCredential(mail, password)
			};


			try
			{
				return client.SendMailAsync(
				new MailMessage(from: mail,
				to: email,
				subject,
				message
				));
			}
			catch (Exception e)
			{
				throw new Exception(e.Message);
			}
		}
	}
}
