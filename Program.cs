
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using System.Text.RegularExpressions;


namespace ReadMailsCoreConsoleApp
{
    public class Program
    {
        static void Main()
        {


            var recentOTPEmail = ReadInboxMails().First();

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


                imapClient.Inbox.Open(FolderAccess.ReadOnly);
                var uids = imapClient.Inbox.Search(SearchQuery.FromContains("axana_admin1@fastmail.com")
                    .And(SearchQuery.SubjectContains("NextGen MDT - Continue your login with this One Time Password"))
                    ).OrderByDescending(x => x.Id);
                foreach (var uid in uids)
                {
                    var mimeMessage = imapClient.Inbox.GetMessage(uid);
                    yield return mimeMessage;
                }
                imapClient.Disconnect(true);
            }
        }


        
    }
}