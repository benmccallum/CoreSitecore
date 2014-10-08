using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Workflows.Simple;
using System;
using System.Net.Mail;

namespace CoreSitecore.Workflows.Simple
{
    /// <summary>
    /// This class sends an notificaion email to the user who submitted the item for approval after a content item is approved
    /// </summary>
    public class ApprovedEmailNotification
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

                var approvingUser = Sitecore.Context.User;
                Assert.ArgumentNotNull(approvingUser, "approvingUser");

                string subject = String.Format("Item '{0}' has been approved", contentItem.Paths.FullPath);
                string body = String.Format("Item '{0}' has been approved by '{1}'", contentItem.Paths.FullPath, approvingUser.Profile.FullName);

                try
                {
                    System.Net.Mail.MailMessage mailMessage = new MailMessage(approvingUser.Profile.Email, userEmail);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    SmtpClient smtpClient = new SmtpClient(Sitecore.Configuration.Settings.MailServer);
                    smtpClient.Send(mailMessage);
                }
                catch (SmtpException ex)
                {
                    Log.Error(String.Format("Unable to send approval email to '{0}' error occurred {1}", submittingUser.Profile.FullName, ex.ToString()), this);
                }
            }
        }
    }
}
