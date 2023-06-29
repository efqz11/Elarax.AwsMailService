using System.Net.Mail;

namespace Elarax.AwsMailService
{
    /// <summary>
    /// <inheritdoc />
    /// </summary>
    public class AwsEmailServiceConfig : IAwsEmailServiceConfig
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
        public string ApiKey { get; set; }

			public string ApiSecret { get; set; }
        /// <summary>
        /// <inheritdoc />
        /// </summary>
        public int Port { get; set; }


		public string _username => ApiKey;

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

        public AwsEmailServiceConfig()
        {
            // Set defaults:
            _smtpDeliveryMethod = null;
            _enableSsl = null;
        }
    }
}
