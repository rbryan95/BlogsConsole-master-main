﻿using NLog;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;
using System.Collections.Generic;

// See https://aka.ms/new-console-template for more information
string path = Directory.GetCurrentDirectory() + "\\nlog.config";

// create instance of Logger
var logger = LogManager.LoadConfiguration(path).GetCurrentClassLogger();
logger.Info("Program started");

//user input 
 var db = new BloggingContext();
logger.Info("About to display menu");
        while (true)
        {
            Console.WriteLine("Menu:");
            Console.WriteLine("1. Display all blogs");
            Console.WriteLine("2. Add a blog");
            Console.WriteLine("3. Create a post");
            Console.WriteLine("4. Display posts for a selected blog");
            Console.WriteLine("5. Exit");

            Console.Write("Enter your choice: ");
            int choice;
            if (!int.TryParse(Console.ReadLine(), out choice))
            {
                Console.WriteLine("Invalid input. Please enter a number.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    DisplayAllBlogs(db);
                    break;

                case 2:
                    AddBlog(db);
                    break;

                case 3:
                    CreatePost(db);
                    break;

                case 4:
                    DisplayPostsForSelectedBlog(db);
                    break;

                case 5:
                    logger.Info("Program ended");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please select a valid option.");
                    break;
            }

        }


    
//adisplay blogs blogs 
    static void DisplayAllBlogs(BloggingContext db)
    {
        var query = db.Blogs.OrderBy(b => b.Name);

        Console.WriteLine("All blogs in the database:");
        foreach (var item in query)
        {
            Console.WriteLine(item.Name);
        }
    }


//adding blogs
    static void AddBlog(BloggingContext db)
    {
        Console.Write("Enter a name for a new Blog: ");
        var name = Console.ReadLine();

        var blog = new Blog { Name = name };

        db.AddBlog(blog);
        Console.WriteLine("Blog added - {0}", name);
    }


//create a post 
    static void CreatePost(BloggingContext db)
    {
        try
        {
            Console.WriteLine("Select the Blog you want to post to:");
            var blogName = Console.ReadLine();

            var selectedBlog = db.Blogs.FirstOrDefault(b => b.Name == blogName);

            if (selectedBlog == null)
            {
                Console.WriteLine("Blog not found. Please add the blog first or check the blog name.");
                return;
            }

            Console.Write("Enter the Post title: ");
            var postTitle = Console.ReadLine();

            Console.Write("Enter the Post content: ");
            var postContent = Console.ReadLine();

            var post = new Post
            {
                Title = postTitle,
                Content = postContent,
                BlogId = selectedBlog.BlogId
            };

            db.Posts.Add(post);
            db.SaveChanges();
            Console.WriteLine("Post added to Blog - {0} - Title: {1}", blogName, postTitle);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }


//display posts for selected blog
    static void DisplayPostsForSelectedBlog(BloggingContext db)
    {
        Console.WriteLine("Select the Blog to view its posts:");
        var blogName = Console.ReadLine();

        var selectedBlog = db.Blogs.FirstOrDefault(b => b.Name == blogName);

        if (selectedBlog == null)
        {
            Console.WriteLine("Blog not found. Please select an existing blog.");
            return;
        }

        var posts = db.Posts.Where(p => p.BlogId == selectedBlog.BlogId).ToList();

        Console.WriteLine($"Posts for '{selectedBlog.Name}' (Total: {posts.Count} Posts):");
        foreach (var post in posts)
        {
            Console.WriteLine($"Blog Name: {selectedBlog.Name}");
            Console.WriteLine($"Post Title: {post.Title}");
            Console.WriteLine($"Post Content: {post.Content}");
            Console.WriteLine();
        }
  
  
    }



// try
// {

//     // Create and save a new Blog
//     Console.Write("Enter a name for a new Blog: ");
//     var name = Console.ReadLine();

//     var blog = new Blog { Name = name };

//     var db = new BloggingContext();
//     db.AddBlog(blog);
//     logger.Info("Blog added - {name}", name);

//     // Display all Blogs from the database
//     var query = db.Blogs.OrderBy(b => b.Name);

//     Console.WriteLine("All blogs in the database:");
//     foreach (var item in query)
//     {
//         Console.WriteLine(item.Name);
//     }
// }
// catch (Exception ex) 
// {
//     logger.Error(ex.Message);
// }



