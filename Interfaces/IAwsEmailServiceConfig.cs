using System.Net.Mail;

namespace Elarax.AwsMailService
{
    /// <summary>
    /// Configuration for use in the Email service
    /// </summary>
    public interface IAwsEmailServiceConfig
    {
        /// <summary>
        /// From field for each Email
        /// </summary>
        string From { get; set; }
        /// <summary>
        /// SMTP Server
        /// </summary>
        string Server { get; set; }
        /// <summary>
        /// UserName for SMTP Server
        /// </summary>
        string ApiKey { get; set; }
        /// <summary>
        /// Password for SMTP Server
        /// </summary>
        string ApiSecret { get; set; }
        /// <summary>
        /// Port for Host
        /// </summary>
        int Port { get; set; }
				string _username {get;}


        /// <summary>
        /// (Optional) Override EnableSsl used by the Smtp Client.
        /// Default value used in EmailService is true.
        /// </summary>
        bool EnableSsl { get; set; }

        /// <summary>
        /// (Optional) Override SmtpDeliveryMethod used by the Smtp Client.
        /// Default value used in EmailService is SmtpDeliveryMethod.Network.
        /// </summary>
        SmtpDeliveryMethod SmtpDeliveryMethod { get; set; }

        /// <summary>
        /// (Optional if SmtpDeliveryMethod is not set to SpecifiedPickupDirectory)
        /// The directory to save the email to.
        /// </summary>
        string PickupDirectoryLocation { get; set; }

        ///// <summary>
        ///// Indicates whether the Email body could contain HTML
        ///// </summary>
        //bool AllowHtml { get; set; }
        ///// <summary>
        ///// Indicates whether the Email allows Attachments
        ///// </summary>
        //bool AllowAttachments { get; set; }
    }
}
