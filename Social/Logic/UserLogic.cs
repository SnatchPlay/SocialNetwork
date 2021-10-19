using MongoDB.Bson;
using Social.Models;
using Social.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Social.Logic
{
    public class UserLogic
    {
        RepositoryUser repository;
        GraphRepository graphRepository;
        public UserLogic()
        {
            repository = new RepositoryUser();
            graphRepository = new GraphRepository();
        }
        public bool CheckPassword(string email, string password)
        {

            Users user = new Users();
            user = repository.GetUser(email);
            if (user != null)
            {
                if (user.Password == password)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            return false;
        }
        public bool CheckIsUserInDatabase(string email)
        {
            Users user = new Users();
            user = repository.GetUser(email);
            if (user == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public void InsertUser(string name, string lastname, string email, string password)
        {
            Users user = new Users();
            user.Name = name;
            user.LastName = lastname;
            user.Email = email;
            user.Password = password;
            repository.Add(user);
            RelationUser relationUser = new RelationUser();
            relationUser.Name = name;
            relationUser.Surname = lastname;
            relationUser.EMail = email;
            graphRepository.CreatePerson(relationUser);
        }
        public void AddFollowing(string whofollowed, string newFollowing)
        {
            repository.AddFollowing(whofollowed, newFollowing);

            Users user1 = new Users();
            user1 = GetUser(whofollowed);

            Users user2 = new Users();
            user2 = GetUser(newFollowing);
            RelationUser ruser1 = new RelationUser();
            RelationUser ruser2 = new RelationUser();
            ruser1.Surname = user1.LastName;
            ruser1.Name = user1.Name;
            ruser1.EMail = user1.Email;
            ruser2.Surname = user2.LastName;
            ruser2.Name = user2.Name;
            ruser2.EMail = user2.Email;
            graphRepository.CreatRelationShip(ruser1,ruser2);


        }
        public ObjectId GetUserId(string email)
        {
            ObjectId Id = repository.GetUserId(email);
            return Id;
        }
        public Users GetUser(string email)
        {
            Users user = repository.GetUser(email);
            return user;
        }
        public string GetUserEmail(string name,string surname)
        {
            List<Users> users = repository.GetUsers();
            foreach( Users u in users)
            {
                if(u.Name==name && u.LastName == surname)
                {
                    return u.Email;
                }
            }
            return "Sorry";
        }

        public string GetConnectingPathsNumber(string email1,string emailofsearched)
        {
            try
            {
                List<string> res = new List<string>();

                Users user1 = new Users();
                user1 = GetUser(email1);

                Users user2 = new Users();
                user2 = GetUser(emailofsearched);

                RelationUser relation1 = new RelationUser();
                RelationUser relation2 = new RelationUser();
                relation1.Name = user1.Name;
                relation1.Surname = user1.LastName;
                relation1.EMail = user1.Email;
                relation2.Name = user2.Name;
                relation2.Surname = user2.LastName;
                relation2.EMail = user2.Email;
                var temp = graphRepository.ConnectingPaths(relation1,relation2);

                foreach (var elem in temp)
                {
                    res.Add(elem);
                }
                if (res.Count == 0)
                {
                    return "No connection";
                }
                else if (res.Count == 2)
                {
                    return "following";
                }
                else if (res.Count - 1 > 1)
                {
                    return "Connection : " + (res.Count - 1).ToString();
                }
                else
                {
                    return "No connection";
                }
            }
            catch (Exception e)
            {
                return " ";
            }
        }
    }
}
