# Service written in .NETCore for sending emails from AWS Email Credentials.

## Install Package
### `dotnet add package Elarax.AwsMailService`

## Configure Startup

### Obtain SMTP Credentials from AWS
https://docs.aws.amazon.com/ses/latest/dg/smtp-credentials.html


### Update appSettings.json
```
"AwsKeys": {
	"ApiKey": "AWS_API_KEY",
	"ApiSecret": "AWS_API_SECRET_KEY",
	"Server": "EMAIL SERVER"
},
```

### Add to pipeline
```
builder.Services.AddEmailService(options =>
{
	options.ApiKey = builder.Configuration["AwsKeys:ApiKey"];
	options.ApiSecret = builder.Configuration["AwsKeys:ApiKey"];
	//options.From = Configuration["AwsKeys:From"]; // optional
	options.Server = builder.Configuration["AwsKeys:Server"];
});
```

## Send Email
```
using Elarax.AwsMailService;

private readonly IEmailService emailService;

public ConfigurationsController(IEmailService emailService)
{
	this.emailService = emailService;
}

public async Task<JsonResult> SendTestEmail(string sendermail)
{
	var toEmail = "mail@server.com";

	var mail = new System.Net.Mail.MailMessage();
	mail.To.Add(toEmail);

	mail.Subject = "Test Email";
	mail.Body = "<html><body style='font-family:Arial'<p>Hello from Elarax.AwsMailService</p>" +
							"<p>This is a test mail</p>" +
							"<br />"+
							"<p>*please ignore this mail</p></body></html>";

	bool isEmailSuccess = await emailService.SendEmailAsync(mail);
	return Json(new { Result = isEmailSuccess ? "success" : "failure" });
}

```



