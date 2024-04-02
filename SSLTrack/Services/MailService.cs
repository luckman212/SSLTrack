namespace SSLTrack.Services;

public class MailService : MailProperties
{
    private readonly ILogger<MailService> _logger;
    private readonly RetryPolicy _retryPolicy = Policy.Handle<Exception>()
                .WaitAndRetry(3, retryCount => TimeSpan.FromSeconds(Math.Pow(2, retryCount)));

    public MailService(ILogger<MailService> logger, IConfiguration configuration)
    {
        _logger = logger;
        var mailProperties = configuration.GetSection("MailProperties").Get<MailProperties>()!;

        MailFrom = mailProperties.MailFrom;
        MailTo = mailProperties.MailTo;
        Bcc = mailProperties.Bcc;
        Subject = mailProperties.Subject;
        Body = mailProperties.Body;
        Attachment = mailProperties.Attachment;
        IsBodyHtml = mailProperties.IsBodyHtml;
        Name = mailProperties.Name;
        Username = mailProperties.Username;
        Password = mailProperties.Password;
        SmtpHost = mailProperties.SmtpHost;
        Port = mailProperties.Port;
        EnableSsl = mailProperties.EnableSsl;
    }

    public void SendMail()
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(Name, MailFrom));
        message.To.Add(new MailboxAddress(MailTo, MailTo));
        message.Subject = Subject;
        message.Body = new TextPart("html")
        {
            Text = Body
        };

        var smtpClient = new SmtpClient();
        try
        {
            smtpClient.Connect(SmtpHost, Port, EnableSsl ? SecureSocketOptions.Auto : SecureSocketOptions.None);
            if (Username != "")
            {
                smtpClient.Authenticate(Username, Password);
            }
            _retryPolicy.Execute(() => smtpClient.Send(message));
        }
        catch (Exception ex)
        {
            _logger.LogError("{ex}", ex.Message);
        }
        finally
        {
            smtpClient.Disconnect(true);
        }
    }
}