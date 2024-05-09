using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using NotificationSvc.Core.Services.Interfaces;
using NotificationSvc.Core.ViewModels;
using Shared.Configuration;
using Shared.DataAccess.EfCore.UnitOfWork;
using Shared.DataAccess.Repository;
using Shared.Entities;
using Shared.FileStorage;
using Shared.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NotificationSvc.Core.Services
{
    public class EmailService : IEmailService
    {
        private IMailService _mailService;
        private readonly IRepository<Email, long> _emailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IFileStorageService _fileStorageService;
        private readonly IFileStorageService _documentService;
        private readonly AppSettingsConfiguration _appSettingsConfiguration = new AppSettingsConfiguration();

        public EmailService(IUnitOfWork unitOfWork,
            IMailService mailService,
            IWebHostEnvironment webHostEnvironment,
            IRepository<Email, long> emailRepository,
            ILogger<EmailService> logger,
            IConfiguration config,
            IFileStorageService documentService,
            IFileStorageService fileStorageService
            )
        {
            _unitOfWork = unitOfWork;
            _fileStorageService = fileStorageService;
            _emailRepository = emailRepository;
            _mailService = mailService;
            _logger = logger;
            _documentService = documentService;
            config.Bind(nameof(AppSettingsConfiguration), _appSettingsConfiguration);
        }

        public async Task<IEnumerable<EmailVM>> GetEmails(string recipient, string subject)
        {
            IQueryable<Email> query = null;
            query = _emailRepository.GetAll().OrderByDescending(x => x.Created);

            if (!string.IsNullOrEmpty(recipient))
                query = query.Where(x => x.Address.ToLower() == recipient.ToLower());

            if (!string.IsNullOrEmpty(subject))
                query = query.Where(x => x.Subject.ToLower() == subject.ToLower());

            if (query.Any())
                return query.Select(x => (EmailVM)x);
            return new List<EmailVM>();
        }

        public async Task SendEmail(string[] emailAddresses, string emailTemplate, Dictionary<string, string> replacements, string senderName, string emailPassword, IEnumerable<string> attachments = null)
        {
            var template = CoreConstants.EmailTemplates.FirstOrDefault(x => x.Name.Equals(emailTemplate, StringComparison.InvariantCultureIgnoreCase));

            if (template == null)
                throw new FileNotFoundException($"Email Template not found for {emailTemplate}");

            _logger.LogInformation($"email template {template.Name} {template.TemplatePath}");
            Mail mailBase;
            if (senderName == null)
            {
                mailBase = new Mail(_appSettingsConfiguration.SystemEmail, template.Subject, emailPassword, emailAddresses);
            }
            else
            {
                mailBase = new Mail(senderName, template.Subject, emailPassword, emailAddresses);
            }

            mailBase.IsBodyHtml = true;
            mailBase.BodyIsFile = true;
            mailBase.BodyPath = _documentService.GetFile(template.TemplatePath).PhysicalPath;
            
            using (StreamReader streamReader = File.OpenText(_documentService.GetFile(template.TemplatePath).PhysicalPath))
            {
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = streamReader.ReadToEnd();
            }

            if (attachments != null)
            {
                mailBase.Attachments = new List<Attachment>();
                foreach (var item in attachments)
                {
                    var fileInfo = _fileStorageService.GetFile(item);
                    mailBase.Attachments.Add(new Attachment(fileInfo.PhysicalPath));
                }
            }

            try
            {
                var email = _emailRepository.Insert(new Email
                {
                    Address = string.Join(';', mailBase.To),
                    Body = mailBase.IsBodyHtml ? mailBase.BodyPath : emailTemplate,
                    Created = DateTime.Now,
                    Subject = template.Subject,
                    JsonReplacements = DictionaryToJson(replacements)
                });
                await _mailService.SendMailAsync(mailBase, replacements);
                email.Sent = true;
                email.Modified = DateTime.Now;

                if (attachments != null)
                {
                    foreach (var attachment in attachments)
                    {
                        
                        _documentService.DeleteFile(attachment);
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message ?? e.InnerException.Message);
            }
            finally
            {
                _unitOfWork.SaveChanges();
            }
        }

        private string DictionaryToJson(Dictionary<string, string> dict)
        {
            var result = "{";
            foreach (var dic in dict)
            {
                result += string.Join(",", string.Format("\"{0}\": [{1}]", dic.Key, dic.Value));
            }
            return result + "}";
        }
    }
}
