﻿namespace AmourConnect.App.Interfaces.Services.Email
{
    public interface IConfigEmail
    {
        Task configMail(string toEmail, string subject, string body);
    }
}