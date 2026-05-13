using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Seamstress.VSA.Infrastructure.Email;

public class SmtpEmailSender<TUser>(
    IOptionsSnapshot<SmtpOptions> options,
    ILogger<SmtpEmailSender<TUser>> logger) 
    : IEmailSender<TUser> where TUser : class
{
    private readonly SmtpOptions _options = options.Value;

    public async Task SendConfirmationLinkAsync(TUser user, string email, string confirmationLink)
    {
        await SendEmailAsync(email, "Confirma tu correo electrónico", $"Por favor confirma tu cuenta haciendo clic aquí: <a href='{confirmationLink}'>Enlace</a>");
    }

    public async Task SendPasswordResetCodeAsync(TUser user, string email, string resetCode)
    {
        var subject = "Restablece tu contraseña";
        var body = $@"
            <h1>Recuperación de cuenta</h1>
            <p>Copia y pega este código en la aplicación: <b>{resetCode}</b></p>";

        await SendEmailAsync(email, subject, body);
    }

    public async Task SendPasswordResetLinkAsync(TUser user, string email, string resetLink)
    {
        await SendEmailAsync(email, "Restablece tu contraseña", $"Haz clic aquí: <a href='{resetLink}'>Enlace</a>");
    }

    private async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_options.SenderName, _options.SenderEmail));
            message.To.Add(new MailboxAddress("", email));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            // Conexión segura a Gmail
            await client.ConnectAsync(_options.SmtpServer, _options.SmtpPort, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_options.SenderEmail, _options.Password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);

            logger.LogInformation("Email enviado exitosamente a {Email}", email);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error crítico enviando correo a {Email}", email);
            throw;
        }
    }
}
