using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SocialNetwork.BLL.Interfaces;
using SocialNetwork.BLL.Services;
using SocialNetwork.DAL;
using SocialNetwork.DAL.EF;
using SocialNetwork.DAL.Entities;
using SocialNetwork.DAL.Interfaces;

namespace SocialNetwork.BLL
{
    public class InjectionResolver
    {
        public static void ConfigureServices(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<NetworkContext>(config =>
                        config
                            .UseLazyLoadingProxies()
                            .UseSqlServer(connectionString)
                );

            
            services
                .AddIdentityCore<ApplicationUser>()
                .AddEntityFrameworkStores<NetworkContext>();

            services.AddScoped<INetworkUnitOfWork, NetworkUnitOfWork>();

            services.AddScoped<IPublicationService, PublicationService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ILikeService, LikeService>();
            services.AddScoped<IFriendshipService, FriendshipService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IRequestService, RequestService>();
        }
    }
}
