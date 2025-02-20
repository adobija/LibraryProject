﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using LibraryProject.abstracts;
using LibraryProject.controllers;
using LibraryProject.exceptions;

namespace LibraryProject.classes
{
    public class User : UserAbstract
    {
        public User(string userName, string password) : base(userName, password) {
        }

        
        public User(string userName, string password, int id) : base(userName, password, id)
        {
        }

        public User() { }


    }
}
