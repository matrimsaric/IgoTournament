using CompetitionDomain.Model;
using StoneLedger.Services.Api.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;

namespace StoneLedger.Services.Api
{
    public class SgfService : ISgfService
    {
        private readonly HttpClient _http;

        public SgfService(HttpClient http)
        {
            _http = http;
        }

        public async Task CreateSgfRecord(SgfRecord newSgfRecord)
        {
            var response = await _http.PostAsJsonAsync("api/content/sgf-records", newSgfRecord);

            response.EnsureSuccessStatusCode();
        }
    }
}
