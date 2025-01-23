//using Microsoft.AspNetCore.SignalR;
//using Microsoft.AspNetCore.Mvc;
//using API.Features.Authentification.Filters;
//using API.Features.Users;
//using API.Services;
//using API.Entities;
//using API.Features.RequestFriend;
//
//
//
//namespace API.Features.Message;
//
//public class ChatHub
//(
//        IUserRepository userRepository, IRequestFriendsRepository RequestFriendsRepository, IMessageRepository MessageRepository, 
//        IHttpContextAccessor httpContextAccessor, IRegexUtils regexUtils, IJWTSessionUtils jWTSessionUtils,
//        IRepository<Entities.Message> repositoryM
//) 
//: Hub
//{
//
//    private readonly IUserRepository _userRepository = userRepository;
//    private readonly IRequestFriendsRepository _requestFriendsRepository = RequestFriendsRepository;
//    private readonly IMessageRepository _messageRepository = MessageRepository;
//    private readonly string token_session_user = jWTSessionUtils.GetValueClaimsCookieUser(httpContextAccessor.HttpContext);
//    private readonly IRegexUtils _regexUtils = regexUtils;
//    private readonly IRepository<Entities.Message> _repositoryM = repositoryM;
//    private async Task<User> _GetDataUserConnected(string token_session_user, CancellationToken cancellationToken) => await _userRepository.GetUserWithCookieAsync(token_session_user, cancellationToken);
//    
//    
//    public async Task SendMessage(CommandMessage commandMessage)
//    {
//        await this.Clients.All.SendAsync("ReceiveMessage", user, message);
//    }
//}