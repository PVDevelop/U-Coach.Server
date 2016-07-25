﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PVDevelop.UCoach.Server.WebApi
{
    public class ActionResultBuilder : 
        IActionResultBuilder
    {
        private bool _disposed;
        private readonly HttpClient _client;
        private readonly HttpClientHandler _clientHandler;
        private readonly List<Tuple<string, string>> _parameters = new List<Tuple<string, string>>();

        public ActionResultBuilder(string baseAddress)
        {
            if(baseAddress == null)
            {
                throw new ArgumentNullException(nameof(baseAddress));
            }

            _clientHandler = new HttpClientHandler();
            _client = new HttpClient(_clientHandler);
            _client.BaseAddress = new Uri(baseAddress);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public IActionResultBuilder AddCookies(CookieCollection cookies)
        {
            if(cookies == null)
            {
                throw new ArgumentNullException(nameof(cookies));
            }

            _clientHandler.CookieContainer.Add(cookies);
            return this;
        }

        public IActionResultBuilder AddParameter(string key, string value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            _parameters.Add(new Tuple<string, string>(key, value));
            return this;
        }

        public async Task<HttpResponseMessage> BuildGetAsync(string resource)
        {
            return await _client.GetAsync(GetResource(resource));
        }

        public async Task<HttpResponseMessage> BuildPostAsync(string resource, HttpContent content)
        {
            return await _client.PostAsync(GetResource(resource), content);
        }

        public async Task<HttpResponseMessage> BuildPutAsync(string resource, HttpContent content)
        {
            return await _client.PutAsync(GetResource(resource), content);
        }

        public async Task<HttpResponseMessage> BuildDeleteAsync(string resource)
        {
            return await _client.DeleteAsync(GetResource(resource));
        }

        private string GetResource(string resource)
        {
            if (_parameters.Any())
            {
                var parametersStr = _parameters.Select(t => string.Format("{0}={1}", t.Item1, t.Item2));
                resource += string.Format("?{0}", string.Join("&", parametersStr));
            }

            return resource;
        }

        public void Dispose()
        {
            if(_disposed)
            {
                return;
            }

            _client.Dispose();
            _clientHandler.Dispose();

            _disposed = true;
        }
    }
}