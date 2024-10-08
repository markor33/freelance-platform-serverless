﻿using Amazon.DynamoDBv2.DataModel;
using Amazon.DynamoDBv2;
using FreelancerProfile.Domain.AggregatesModel.FreelancerAggregate.Entities;
using FreelancerProfile.Domain.Repositories;
using Amazon;
using ReadModel;

namespace ReadModelStore;

public class LanguageRepository : ILanguageRepository
{
    private readonly AmazonDynamoDBClient _client;
    private readonly DynamoDBContext _context;

    public LanguageRepository()
    {
        _client = new AmazonDynamoDBClient(RegionEndpoint.EUCentral1);
        _context = new DynamoDBContext(_client);
    }

    public async Task<List<Language>> Get()
    {
        var scanConditions = new List<ScanCondition>();
        var languageViewModels = await _context.ScanAsync<LanguageViewModel>(scanConditions).GetRemainingAsync();

        var languages = languageViewModels.Select(x => x.ToLanguage()).ToList();

        return languages;
    }

    public async Task<Language> GetByIdAsync(int id)
    {
        var languageViewModel = await _context.LoadAsync<LanguageViewModel>(id);

        return languageViewModel.ToLanguage();
    }
}
