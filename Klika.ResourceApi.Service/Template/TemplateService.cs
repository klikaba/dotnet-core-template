using Klika.ResourceApi.Database.DbContexts;
using Klika.ResourceApi.Model.Entities.TemplateDbContext;
using Klika.ResourceApi.Model.Interfaces.Template;
using Klika.ResourceApi.Repository.EFCore;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Klika.ResourceApi.Service.Template
{
    public class TemplateService : ITemplateService
    {
        private readonly HttpClient _httpClient;
        IEFRepository<TemplateDbContext> _templateDbRepo;
        private readonly ILogger<TemplateService> _logger;

        public TemplateService(IEFRepository<TemplateDbContext> templateDbRepo, 
                               ILogger<TemplateService> logger, 
                               HttpClient httpClient)
        {
            _templateDbRepo = templateDbRepo;
            _logger = logger;
            _httpClient = httpClient;
        }

        public async Task Create()
        {
            try
            {
                await _templateDbRepo.CreateAsync<TemplateEntity>(new TemplateEntity()
                {
                    TemplateProperty1 = "test",
                    TemplateProperty2 = 1,
                    TemplateProperty3 = true,
                }).ConfigureAwait(false);

                await _templateDbRepo.SaveChangesAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Create));
                throw;
            }
        }
    }
}
