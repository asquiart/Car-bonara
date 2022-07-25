using CarbonaraWebAPI.Data;
using Microsoft.AspNetCore.Mvc;
using CarbonaraWebAPI.Model.DAO;
using Microsoft.EntityFrameworkCore;

namespace CarbonaraWebAPI.Services
{
    public class ExternalPartnerService
    {
        private AppDbContext Context;
        public ExternalPartnerService(AppDbContext context)
        {

            Context = context;
        }

        public int GenerateCardId()
        {
            bool newCard = false;
            int cardId = 0;
            User user;
            Random rd = new Random();

            do
            {
                // MaxValue doesn't get generated
                cardId = rd.Next(1, 1000000);
                user = Context.User.Where(u => u.CardId == cardId).SingleOrDefault();
                if (user == null)
                {
                    newCard = true;
                }
                else
                {
                    newCard = false;
                }

            } while (!(newCard == true));
            return cardId;
        }
    }
}
