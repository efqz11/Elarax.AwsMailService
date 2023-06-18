using System.Net.Mail;

namespace Elarax.AwsMailService
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public class EmailServiceConfig : IEmailServiceConfig
    {
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public int Port { get; set; }

		/// <summary>
		/// <inheritdoc />
		/// </summary>
		public string Username { get; set; }
		/// <summary>
		/// <inheritdoc />
		/// </summary>
		public string Password { get; set; }


        private bool? _enableSsl;
        public bool EnableSsl
        {
            get => _enableSsl ?? true;
            set => _enableSsl = value;
        }

        private SmtpDeliveryMethod? _smtpDeliveryMethod;
        public SmtpDeliveryMethod SmtpDeliveryMethod
        {
            get => _smtpDeliveryMethod ?? SmtpDeliveryMethod.Network;
            set => _smtpDeliveryMethod = value;
        }

        public string PickupDirectoryLocation { get; set; }

        public EmailServiceConfig()
        {
            // Set defaults:
            _smtpDeliveryMethod = null;
            _enableSsl = null;
        }
    }
}
