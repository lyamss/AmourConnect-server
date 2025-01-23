using API.Entities;



namespace API.Services.Email
{
    public class BodyEmail(SecretEnv SecretEnv) : IBodyEmail
    {
        private readonly string _requestUrlPageRequest = $"{SecretEnv.IP_Backend}/request";
        private readonly string _requestUrlWebSite = $"{SecretEnv.IP_Backend}";
        public string subjectRegister
        {
            get { return "Bienvenu chez AmourConnect ❤️"; }
        }

        public string subjectRequestFriend
        {
            get { return "Demande de match ❤️"; }
        }

        public string subjectAcceptFriend 
        {
            get { return " a accepté(e) le match ❤️"; }
        }

        public string _emailBodyRegister(string pseudo) =>
            $@"
    <!DOCTYPE html>
    <html lang=""fr"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Bienvenue chez AmourConnect 😍❤️</title>
    </head>
    <body style=""font-family: Arial, sans-serif; background-color: #f9e9f9; margin: 0; padding: 0;"">
        <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""min-width: 100%; background-color: #f9e9f9;"">
            <tr>
                <td align=""center"" style=""padding: 40px 0;"">
                    <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""600"" style=""max-width: 600px; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);"">
                        <tr>
                            <td style=""padding: 40px;"">
                                <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">
                                    <tr>
                                        <td>
                                            <h1 style=""color: #d33469; text-align: center; font-size: 28px; margin-bottom: 20px;"">Bonjour {pseudo},</h1>
                                            <p style=""color: #333333; font-size: 16px; line-height: 1.6; margin-bottom: 20px;"">
                                                Merci d'avoir validé ton inscription et bienvenue sur notre site de rencontre. Nous sommes ravis de t'accueillir dans notre communauté dédiée à l'amour et aux belles rencontres.
                                            </p>
                                            <p style=""color: #333333; font-size: 16px; line-height: 1.6; margin-bottom: 20px;"">
                                                N'hésite pas à compléter ton profil et à découvrir les profils des autres membres.
                                            </p>
                                            <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">
                                                <tr>
                                                    <td align=""center"">
                                                        <a href=""{_requestUrlWebSite}"" style=""display: inline-block; background-color: #d33469; color: #ffffff; text-decoration: none; font-weight: bold; padding: 12px 24px; border-radius: 4px; font-size: 16px;"">Commencer l'aventure</a>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""padding-top: 40px; border-top: 1px solid #eeeeee; margin-top: 40px;"">
                                            <p style=""color: #888888; font-size: 14px; text-align: center; margin: 0;"">
                                                Cordialement,<br>
                                                <a href=""{_requestUrlWebSite}"" style=""color: #d33469; text-decoration: none;"">L'équipe AmourConnect</a>
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </body>
    </html>
    ";

        public string _requestFriendBodyEmail(string pseudoReceiver, User dataUserIssuer) =>
            $@"
    <!DOCTYPE html>
    <html lang=""fr"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Demande de match ❤️</title>
    </head>
    <body style=""font-family: Arial, sans-serif; background-color: #f9e9f9; margin: 0; padding: 0;"">
        <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""min-width: 100%; background-color: #f9e9f9;"">
            <tr>
                <td align=""center"" style=""padding: 40px 0;"">
                    <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""600"" style=""max-width: 600px; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);"">
                        <tr>
                            <td style=""padding: 40px;"">
                                <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">
                                    <tr>
                                        <td>
                                            <h1 style=""color: #d33469; text-align: center; font-size: 28px; margin-bottom: 20px;"">Bonjour {pseudoReceiver},</h1>
                                            <p style=""color: #333333; font-size: 16px; line-height: 1.6; margin-bottom: 20px;"">
                                                <strong><span style=""color: #ff69b4;"">{dataUserIssuer.Pseudo}</span></strong> aimerait faire ta connaissance et te propose un match sur AmourConnect.
                                            </p>
                                            <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""margin-bottom: 20px;"">
                                                <tr>
                                                    <td align=""center"">
                                                        <a href=""{_requestUrlPageRequest}"" style=""display: inline-block; background-color: #d33469; color: #ffffff; text-decoration: none; font-weight: bold; padding: 12px 24px; border-radius: 4px; font-size: 16px;"">Accepter la demande</a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <p style=""color: #333333; font-size: 16px; line-height: 1.6; margin-bottom: 20px;"">
                                                Bonne chance dans tes rencontres et à bientôt sur AmourConnect !
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""padding-top: 40px; border-top: 1px solid #eeeeee; margin-top: 40px;"">
                                            <p style=""color: #888888; font-size: 14px; text-align: center; margin: 0;"">
                                                Cordialement,<br>
                                                <a href=""{_requestUrlWebSite}"" style=""color: #d33469; text-decoration: none;"">L'équipe AmourConnect</a>
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </body>
    </html>
    ";

        public string _acceptFriendBodyEmail(string pseudoReceiver, User dataUserIssuer) =>
            $@"
    <!DOCTYPE html>
    <html lang=""fr"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Demande de match acceptée ❤️</title>
    </head>
    <body style=""font-family: Arial, sans-serif; background-color: #f9e9f9; margin: 0; padding: 0;"">
        <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""min-width: 100%; background-color: #f9e9f9;"">
            <tr>
                <td align=""center"" style=""padding: 40px 0;"">
                    <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""600"" style=""max-width: 600px; background-color: #ffffff; border-radius: 8px; box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);"">
                        <tr>
                            <td style=""padding: 40px;"">
                                <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"">
                                    <tr>
                                        <td>
                                            <h1 style=""color: #d33469; text-align: center; font-size: 28px; margin-bottom: 20px;"">Bonjour {pseudoReceiver},</h1>
                                            <p style=""color: #333333; font-size: 16px; line-height: 1.6; margin-bottom: 20px;"">
                                                <strong><span style=""color: #ff69b4;"">{dataUserIssuer.Pseudo}</span></strong> est ravi(e) ! Ta demande de match a été acceptée sur AmourConnect. Tu peux maintenant discuter et faire connaissance avec {dataUserIssuer.Pseudo}.
                                            </p>
                                            <table cellpadding=""0"" cellspacing=""0"" border=""0"" width=""100%"" style=""margin-bottom: 20px;"">
                                                <tr>
                                                    <td align=""center"">
                                                        <a href=""{_requestUrlPageRequest}"" style=""display: inline-block; background-color: #d33469; color: #ffffff; text-decoration: none; font-weight: bold; padding: 12px 24px; border-radius: 4px; font-size: 16px;"">Commencer à discuter</a>
                                                    </td>
                                                </tr>
                                            </table>
                                            <p style=""color: #333333; font-size: 16px; line-height: 1.6; margin-bottom: 20px;"">
                                                Bonne chance dans tes rencontres et à bientôt sur AmourConnect !
                                            </p>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style=""padding-top: 40px; border-top: 1px solid #eeeeee; margin-top: 40px;"">
                                            <p style=""color: #888888; font-size: 14px; text-align: center; margin: 0;"">
                                                Cordialement,<br>
                                                <a href=""{_requestUrlWebSite}"" style=""color: #d33469; text-decoration: none;"">L'équipe AmourConnect</a>
                                            </p>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </body>
    </html>
    ";
    }


    internal interface IBodyEmail
    {
        string _acceptFriendBodyEmail(string pseudoReceiver, User dataUserIssuer);
        string _requestFriendBodyEmail(string pseudoReceiver, User dataUserIssuer);
        string _emailBodyRegister(string pseudo);

        public string subjectAcceptFriend { get; }
        public string subjectRequestFriend { get; }
        public string subjectRegister { get; }
    }
}