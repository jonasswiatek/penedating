using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Penedating.Data.MongoDB.Model.Contract;
using Penedating.Service.Model;
using Penedating.Service.Model.Contract;
using log4net;

namespace Penedating.Service.MongoService
{
    public class MongoHugService : IHugService
    {
        private readonly IHugRepository _hugRepository;
        private readonly IUserProfileService _userProfileService;
        private readonly ILog _logger;

        public MongoHugService(IHugRepository hugRepository, IUserProfileService userProfileService)
        {
            _hugRepository = hugRepository;
            _userProfileService = userProfileService;
            _logger = LogManager.GetLogger(this.GetType());
        }

        public IEnumerable<Hug> GetHugs(UserAccessToken userAccessToken)
        {
            _logger.Info("Getting hugs for: " + userAccessToken.Email);
            var mongoHugs = _hugRepository.GetHugs(userAccessToken.Ticket).ToList();
            var hugs = Mapper.Map<IList<Service.Model.Hug>>(mongoHugs);

            return hugs;
        }

        public void SendHug(UserAccessToken userAccessToken, string recipientUserId)
        {
            _logger.Info("Sending hug from: " + userAccessToken.Email + " to " + recipientUserId);
            var hug = new Hug()
                          {
                              Created = DateTime.Now,
                              SenderID = userAccessToken.Ticket
                          };

            var mongoHug = Mapper.Map<Data.MongoDB.Model.Hug>(hug);
            _hugRepository.InsertHug(recipientUserId, mongoHug);
        }

        public void DismissHugs(UserAccessToken userAccessToken)
        {
            _hugRepository.DismissHugs(userAccessToken.Ticket);
        }
    }
}
