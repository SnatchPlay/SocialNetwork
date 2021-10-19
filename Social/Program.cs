using Social.Logic;
using Social.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Social
{
    class Program
    {
        static void Main(string[] args)
        {
                PostLogic postLogic = new PostLogic();
                UserLogic userLogic = new UserLogic();
            
                Console.WriteLine("Welcome to SuperPuperMegaSocialNetwork!");
                Console.WriteLine("Please login(1) or register(2):");
                int choose = Convert.ToInt32(Console.ReadLine());
                string email="",password;
                Users logged=new Users();
                if (choose == 1)
                {
                    Console.WriteLine("Email:");
                    email = Console.ReadLine();
                    Console.WriteLine("Password:");
                    password = Console.ReadLine();
                    if (userLogic.CheckIsUserInDatabase(email) == false||userLogic.CheckPassword(email, password) == false  )
                    {
                        Console.WriteLine("Something wrong.");
                    }
                    else
                    {
                    logged= userLogic.GetUser(email);
                    List<Posts> posts= postLogic.GetNewPosts(logged.Follows);
                    for( int i = 0; i < posts.Count(); i++)
                    {
                        Console.WriteLine($"{posts[i].Body}\n{posts[i].Date}\n");
                    }
                    }
                }
                else if (choose == 2)
                {
                    Console.WriteLine("Type email:");
                    email = Console.ReadLine();
                    Console.WriteLine("Type password:");
                    password = Console.ReadLine();
                    Console.WriteLine("Your name:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Your SurName:");

                    string surname = Console.ReadLine();
                    userLogic.InsertUser(name, surname, email, password);
                }

            Menu(email, logged,postLogic);

            }
        public static void Menu(string e, Users user, PostLogic postLogic)
        {
            UserLogic userLogic = new UserLogic();
            List<Posts>newposts = postLogic.GetNewPosts(user.Follows);
            foreach(Posts p in newposts)
            {
                Console.WriteLine($"{p.Body}\n{p.Date}\n");
            }
            Console.WriteLine("Do you want to:\n 1-Find new friend \n " +
                "2-Write a post\n" +
                "3-Look over s-bodies post\n" +
                "4-Go out");
            var x = Console.ReadLine();
            switch (x)
            {
                case "1":
                    Console.WriteLine("Name of searched person:");
                    string N = Console.ReadLine();
                    Console.WriteLine("Surname:");
                    string S = Console.ReadLine();
                    Console.WriteLine( userLogic.GetConnectingPathsNumber(user.Email,userLogic.GetUserEmail(N,S)));
                    userLogic.AddFollowing(user.Email,userLogic.GetUserEmail(N,S));
                    Menu(e, user, postLogic);

                    break;
                case "2":
                    Console.WriteLine("Post text:");
                    string text = Console.ReadLine();
                    string email = Console.ReadLine();
                    postLogic.InsertPost(text, email);
                    Menu(e, user, postLogic);
                    break;
                case "3":
                    Console.WriteLine("Write name of searched person ");
                    string Name = Console.ReadLine();
                    Console.WriteLine("Write surname now");
                    string Surname = Console.ReadLine();
                    
                    email=userLogic.GetUserEmail(Name, Surname);
                    postLogic.PostReaction(email);
                    Menu(e, user, postLogic);
                    break;

                case "4":
                    Console.WriteLine("Exit");
                    Thread.Sleep(1000);
                    System.Environment.Exit(20);
                    break;
            }

        }
    }
            
 }
    
