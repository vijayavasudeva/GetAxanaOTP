
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Bcpg.OpenPgp;

namespace ReadMailsCoreConsoleApp
{
    public class Program
    {
        static void Main()
        {


            var recentOTPEmail = ReadInboxMails().First();

            //Console.WriteLine("=========================================");
            //Console.WriteLine($"From : {recentOTPEmail.From.ToString()}; Subject: {recentOTPEmail.Subject};  Timestamp: {recentOTPEmail.Date.DateTime.ToString()}  ");
            //Console.WriteLine($"\nText Body : \n {recentOTPEmail.TextBody}");
            //Console.WriteLine("=========================================");
            string regExSearchPattern = @"\d{6}";
            Match m = Regex.Match(recentOTPEmail.TextBody, regExSearchPattern);

            if (m.Success)
            {
                Console.WriteLine(m.Value);
            }

        }

        static IEnumerable <MimeKit.MimeMessage> ReadInboxMails()
        {
            using (var imapClient = new ImapClient())
            {
                imapClient.Connect("imap.fastmail.com", 993, true);
                imapClient.Authenticate("mailfrom@fastmail.com", "rnxdgeaaaq35fwhu");

                //imapClient.Connect("imap.fastmail.com", 993, true);
                //imapClient.Authenticate("axanatest@fastmail.com", "");
                ////imapClient.GetFolder("Inbox/Admin 1").Open(FolderAccess.ReadOnly);
                //var uids = imapClient.GetFolder("Inbox/Admin 1").Search(SearchQuery.All).OrderByDescending(x => x.Id);


                imapClient.Inbox.Open(FolderAccess.ReadOnly);
                var uids = imapClient.Inbox.Search(SearchQuery.FromContains("axana_admin1@fastmail.com")
                    .And(SearchQuery.SubjectContains("NextGen MDT - Continue your login with this One Time Password"))
                    ).OrderByDescending(x => x.Id);
                foreach (var uid in uids)
                {
                    var mimeMessage = imapClient.Inbox.GetMessage(uid);
                    // mimeMessage.WriteTo($"{uid}.eml"); // for testing
                    yield return mimeMessage;
                }
                imapClient.Disconnect(true);
            }
        }


        
    }
}