﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using CharacterBuilder.Core.Model;
using CharacterBuilder.Core.Model.User;
using CharacterBuilder.Infrastructure.Data.Contexts;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CharacterBuilder.Infrastructure.Data
{
    public class CharacterSheetRepository
    {
        private readonly CharacterBuilderDbContext _db;

        public CharacterSheetRepository()
        {
            _db = new CharacterBuilderDbContext();
        }

        public void GetUserSheet(string userId)
        {
            var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_db));

            var currentUser = manager.FindById(userId);

            var userInfoId = currentUser.AppUserInfo.Id;

            var sheets = _db.AppUserInfo.Single(u => u.Id == userInfoId);
        }

        public IList<CharacterSheet> GetCharacterSheetByUserName(string userName)
        {
            return
                _db.CharacterSheets.Where(s => s.UserNameOwner == userName)
                    .Include(c => c.Class)
                    .Include(b => b.Background)
                    .ToList();
        }

        public CharacterSheet GetCharacterSheetById(int sheetId)
        {
            return _db.CharacterSheets.Single(s => s.Id == sheetId);           
        }

        public void CreateCharacterSheet(CharacterSheet sheetToCreate)
        {
            _db.CharacterSheets.Add(sheetToCreate);
            Save();
        }

        public void SaveClassSelection(int classId, int characterSheetId)
        {
            var clsFromDb = _db.Classes.Single(c => c.Id == classId);
            var sheetFromDb = _db.CharacterSheets.Single(s => s.Id == characterSheetId);
            sheetFromDb.Class = clsFromDb;
            Save();
        }

        public void Save()
        {
            _db.SaveChanges();
        }


    }
}
