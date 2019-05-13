using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Models.Tokens.Repository;

namespace API.Auth
{
    internal class AuthDaemon : IHostedService, IDisposable
    {
        private const long PeriodInHours = 24;
        private readonly TokenRepository tokenRepository;
        private Timer timer;

        public AuthDaemon(IAuthenticator authenticator, TokenRepository tokenRepository)
        {
            this.tokenRepository = tokenRepository;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, 
                TimeSpan.FromHours(PeriodInHours));

            return Task.CompletedTask;
        }

        private  void DoWork(object state)
        {
            tokenRepository.DeleteExpiredTokes();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}