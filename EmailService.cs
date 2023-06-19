using System;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Elarax.AwsMailService
{
	public class EmailService : IEmailService
	{
		private readonly IAwsEmailServiceConfig _config;
		private readonly ILogger<EmailService> _logger;

		//  Put your AWS secret access key in this environment variable.
		private readonly byte VERSION = 0x02;

		//  Used to generate the HMAC signature. Do not modify.
		private readonly string MESSAGE = "SendRawEmail";

		//  Version number. Do not modify.

		/// <summary>
		/// The configuration for a File Handler / Service
		/// </summary>
		/// <param name="emailServiceConfig">The configuration to be used</param>
		public EmailService(IOptions<AwsEmailServiceConfig> emailServiceConfig, ILogger<EmailService> logger)
		{
			if (emailServiceConfig == null)
				throw new ApplicationException("SNS Service configuration failed");

			_config = emailServiceConfig?.Value;
			_logger = logger;

			// GenerateSmtpCredentials();
		}



		private void ValidateConfiguration()
		{
			if (_config == null) throw new ArgumentNullException(nameof(_config), "SMS Service configuration was not supplied");
		}


		public bool SendEmail(MailMessage mailMessage)
		{
			try
			{
				SmtpClient client = GetAwsSmtpClient();
				client.Send(mailMessage);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An Error occured while sending Email");
				return false;
			}
		}

		public async Task<bool> SendEmailAsync(MailMessage mailMessage)
		{
			try
			{
				SmtpClient client = GetAwsSmtpClient();
				await client.SendMailAsync(mailMessage);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An Error occured while sending Email");
				return false;
			}
		}

		public async Task<bool> SendEmailAsync(MailMessage mailMessage, IEmailServiceConfig emailServiceConfig)
		{
			try
			{
				SmtpClient client = GetSmtpClient(emailServiceConfig);
				await client.SendMailAsync(mailMessage);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An Error occured while sending Email");
				return false;
			}
		}


		public async Task<bool> SendEmailAsync(
				string to, string subject, string body, bool isbodyHtml = true)
		{
			try
			{
				SmtpClient client = GetAwsSmtpClient();

				var mailMessage = new MailMessage
				{
					IsBodyHtml = isbodyHtml,
					From = new MailAddress(_config.From),
					Subject = subject,
					Body = body
				};
				mailMessage.To.Add(new MailAddress(to));
				await client.SendMailAsync(mailMessage);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An Error occured while sending Email");
				return false;
			}
		}


		public async Task<bool> SendEmailAsync(
				string to, string subject, string body, bool isbodyHtml, IEmailServiceConfig emailServiceConfig)
		{
			try
			{
				SmtpClient client = GetSmtpClient(emailServiceConfig);

				var mailMessage = new MailMessage
				{
					IsBodyHtml = isbodyHtml,
					From = new MailAddress(emailServiceConfig?.From ?? _config.From),
					Subject = subject,
					Body = body
				};
				mailMessage.To.Add(new MailAddress(to));
				await client.SendMailAsync(mailMessage);
				return true;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "An Error occured while sending Email");
				return false;
			}
		}

		public async Task<MailMessage[]> SendMultipleEmailsAsync(params MailMessage[] mailMessages)
		{
			var succeeded = new List<MailMessage>();

			for (var i = 0; i < mailMessages.Length; i++)
			{
				var mail = mailMessages[i];
				if (await SendEmailAsync(mail))
					succeeded.Add(mail);
			}

			return succeeded.ToArray();
		}


		private SmtpClient GetSmtpClient(IEmailServiceConfig config)
		{
			return new SmtpClient(config.Server, config.Port)
			{
				DeliveryMethod = config.SmtpDeliveryMethod,
				PickupDirectoryLocation = config.PickupDirectoryLocation,
				UseDefaultCredentials = false,
				EnableSsl = config.EnableSsl,
				Credentials = new NetworkCredential(config.Username, config.Password),
			};
		}

		private SmtpClient GetAwsSmtpClient()
		{
			var key = _config.ApiSecret;

			byte[] keyBytes = Encoding.UTF8.GetBytes(key);
			byte[] messageBytes = Encoding.UTF8.GetBytes(MESSAGE);

			HMACSHA256 hmac = new HMACSHA256(keyBytes);
			// Compute the HMAC signature on the input data bytes.
			byte[] rawSignature = hmac.ComputeHash(messageBytes);

			// Prepend the version number to the signature.
			byte[] rawSignatureWithVersion = new byte[rawSignature.Length + 1];
			byte[] versionArray = { VERSION };
			Array.Copy(versionArray, 0, rawSignatureWithVersion, 0, 1);
			Array.Copy(rawSignature, 0, rawSignatureWithVersion, 1, rawSignature.Length);

			var _password = Convert.ToBase64String(rawSignatureWithVersion);
			_logger.LogInformation("SMTP password used here | " + _password);

			return new SmtpClient(_config.Server, _config.Port)
			{
				DeliveryMethod = _config.SmtpDeliveryMethod,
				PickupDirectoryLocation = _config.PickupDirectoryLocation,
				UseDefaultCredentials = false,
				EnableSsl = _config.EnableSsl,
				Credentials = new NetworkCredential(_config._username, _password),
			};
		}


	}
}
