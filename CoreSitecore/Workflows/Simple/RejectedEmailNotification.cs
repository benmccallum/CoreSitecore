using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Workflows.Simple;
using System;
using System.Net.Mail;

namespace CoreSitecore.Workflows.Simple
{
    /// <summary>
    /// This class sends an notification back to the user who submitted the item for approval when the item is rejected.
    /// </summary>
    public class RejectedEmailNotification
    {
        /// <summary>
        /// Runs the processor.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public void Process(WorkflowPipelineArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            ProcessorItem processorItem = args.ProcessorItem;
            if (processorItem == null)
            {
                return;
            }
    
            var contentItem = args.DataItem;
            var contentWorkflow = contentItem.Database.WorkflowProvider.GetWorkflow(contentItem);
            var contentHistory = contentWorkflow.GetHistory(contentItem);

            if (contentHistory.Length > 0)
            {
                string username = contentItem[Sitecore.FieldIDs.UpdatedBy];
                Assert.IsFalse(string.IsNullOrEmpty(username), "updatedby user cannot be found");

                var submittingUser = Sitecore.Security.Accounts.User.FromName(username, false);
                Assert.ArgumentNotNull(submittingUser, "submittingUser");
                string userEmail = submittingUser.Profile.Email;

                var rejectingUser = Sitecore.Context.User;
                Assert.ArgumentNotNull(rejectingUser, "rejectingUser");

                string subject = String.Format("Item '{0}' has been rejected", contentItem.Paths.FullPath);
                string body = String.Format("Item '{0}' has been rejected by '{1}': {2}", contentItem.Paths.FullPath, rejectingUser.Profile.FullName, args.Comments);

                try
                {
                    System.Net.Mail.MailMessage mailMessage = new MailMessage(rejectingUser.Profile.Email, userEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    SmtpClient smtpClient = new SmtpClient(Sitecore.Configuration.Settings.MailServer);
                    smtpClient.Send(mailMessage);
                }
                catch (SmtpException ex)
                {
                    Log.Error(String.Format("Unable to send reject email to '{0}' error occurred {1}", submittingUser.Profile.FullName, ex.ToString()), this);
                }
            }
        }
    }
}
